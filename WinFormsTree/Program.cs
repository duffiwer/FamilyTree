using BLL;

namespace WinFormsTree
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            var familyTreeService = new FamilyTreeService(new FamilyTreeRepository());

            var form = new TreeVisualizationForm(familyTreeService);
            Application.Run(form);
        }
    }
}
