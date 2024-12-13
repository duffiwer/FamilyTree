using DAL.Models;
using System.Text;

namespace BLL
{
    public class FamilyTreeService
    {
        private readonly FamilyTreeRepository _repository;

        public FamilyTreeService(FamilyTreeRepository repository)
        {
            _repository = repository;
        }

        public void AddPerson(Person person)
        {
            _repository.AddPerson(person);
        }

        public void AddRelation(string shortId1, string shortId2, string relationType)
        {
            var person1 = _repository.FindPersonByShortId(shortId1);
            var person2 = _repository.FindPersonByShortId(shortId2);

            if (person1 == null || person2 == null)
                throw new Exception("Один из указанных людей не найден.");

            switch (relationType.ToLower())
            {
                case "родитель":
                    if (!person1.Children.Contains(person2)) person1.Children.Add(person2);
                    if (!person2.Parents.Contains(person1)) person2.Parents.Add(person1);
                    break;
                

                case "партнер":
                    if (person1.Spouse != null || person2.Spouse != null)
                        throw new Exception("Один из людей уже состоит в браке.");
                    person1.Spouse = person2;
                    person2.Spouse = person1;
                    break;

            }
        }


        public List<Person> GetClosestRelatives(string shortId)
        {
            var person = _repository.FindPersonByShortId(shortId);
            if (person == null) throw new Exception("Человек не найден");

            var relatives = person.Parents.Concat(person.Children).ToList();
            return relatives;
        }


        public void ClearTree() => _repository.ClearTree();

        public string GetFamilyTreeAsText()
        {
            var people = _repository.GetAllPeople();
            if (!people.Any()) return "Генеалогическое древо пусто.";

            var result = new StringBuilder();
            foreach (var person in people)
            {
                result.AppendLine($"  {person.ShortId} - {person.FullName} ({person.DateOfBirth:yyyy-MM-dd}, {person.Gender})");

                if (person.Parents != null && person.Parents.Any())
                {
                    result.AppendLine("    Родители:");
                    foreach (var parent in person.Parents)
                    {

                        if (parent != null)
                        {
                            result.AppendLine($"      {parent.ShortId} - {parent.FullName} ({parent.DateOfBirth:yyyy-MM-dd}, {parent.Gender})");
                        }
                        else
                        {
                            result.AppendLine("      [Ошибка: Данные о родителях отсутствуют]");
                        }
                    }
                }

                if (person.Children != null && person.Children.Any())
                {
                    result.AppendLine("    Дети:");
                    foreach (var child in person.Children)
                    {
                        if (child != null)
                        {
                            result.AppendLine($"      {child.ShortId} - {child.FullName} ({child.DateOfBirth:yyyy-MM-dd}, {child.Gender})");
                        }
                        else
                        {
                            result.AppendLine("      [Ошибка: Данные о ребенке отсутствуют]");
                        }
                    }
                }

                if (person.Spouse != null)
                {
                    result.AppendLine("    Партнёр:");
                    result.AppendLine($"      {person.Spouse.ShortId} - {person.Spouse.FullName} ({person.Spouse.DateOfBirth:yyyy-MM-dd}, {person.Spouse.Gender})");
                }
            }

            return result.ToString();
        }


        public int CalculateAgeAtBirth(string parentId, string childId)
        {
            var parent = _repository.FindPersonByShortId(parentId);
            var child = _repository.FindPersonByShortId(childId);
            if (parent == null || child == null) throw new Exception("Человек не найден");

            return child.DateOfBirth.Year - parent.DateOfBirth.Year;
        }

        public void SaveTree(string filePath)
        {
            _repository.SaveTree(filePath);
        }

        public void LoadTree(string filePath)
        {
            _repository.LoadTree(filePath);
        }
    }
}
