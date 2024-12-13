using BLL;
using DAL.Models;

namespace WinFormsTree
{
    public partial class TreeVisualizationForm : Form
    {
        private readonly FamilyTreeService _service;

        public TreeVisualizationForm(FamilyTreeService service)
        {
            _service = service;
            InitializeComponent();
        }

        private void TreeVisualizationForm_Load(object sender, EventArgs e)
        {
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            var people = _service.GetAllPeople();
            treeView.Nodes.Clear();

            var processedPeople = new HashSet<Guid>();

            foreach (var person in people)
            {
                if (!processedPeople.Contains(person.Id)) 
                {
                    var rootNode = CreatePersonNode(person);
                    treeView.Nodes.Add(rootNode);

                    AddFamilyToNode(rootNode, person, processedPeople);

                    if (person.Spouse != null)
                    {
                        var spouseNode = CreatePersonNode(person.Spouse);
                        rootNode.Nodes.Add(new TreeNode("Партнер") { Nodes = { spouseNode } });
                        AddFamilyToNode(spouseNode, person.Spouse, processedPeople);

                        if (!processedPeople.Contains(person.Spouse.Id))
                        {
                            var spouseTreeRootNode = CreatePersonNode(person.Spouse);
                            treeView.Nodes.Add(spouseTreeRootNode);
                            AddFamilyToNode(spouseTreeRootNode, person.Spouse, processedPeople);
                        }
                    }
                }
            }

            treeView.ExpandAll();
        }


        private void AddFamilyToNode(TreeNode parentNode, Person person, HashSet<Guid> processedPeople)
        {
            if (processedPeople.Contains(person.Id))
            {
                return;
            }

            processedPeople.Add(person.Id);

            if (person.Parents != null && person.Parents.Count > 0)
            {
                var parentsNode = new TreeNode("Родители");
                parentNode.Nodes.Add(parentsNode);

                foreach (var parent in person.Parents)
                {
                    var parentNodeForPerson = CreatePersonNode(parent);
                    parentsNode.Nodes.Add(parentNodeForPerson);
                    AddFamilyToNode(parentNodeForPerson, parent, processedPeople);
                }
            }

            if (person.Children != null && person.Children.Count > 0)
            {
                var childrenNode = new TreeNode("Дети");
                parentNode.Nodes.Add(childrenNode);

                foreach (var child in person.Children)
                {
                    var childNode = CreatePersonNode(child);
                    childrenNode.Nodes.Add(childNode);
                    AddFamilyToNode(childNode, child, processedPeople);
                }
            }

            if (person.Spouse != null && !processedPeople.Contains(person.Spouse.Id))
            {
                var partnerNode = new TreeNode("Партнер");
                parentNode.Nodes.Add(partnerNode);

                var spouseNode = CreatePersonNode(person.Spouse);
                partnerNode.Nodes.Add(spouseNode);

                AddFamilyToNode(spouseNode, person.Spouse, processedPeople);
            }
        }




        private TreeNode CreatePersonNode(Person person)
        {
            return new TreeNode($"{person.FullName} ({person.DateOfBirth:yyyy-MM-dd}, {person.Gender})")
            {
                Tag = person
            };
        }


    }
}
