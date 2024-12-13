using Spectre.Console;
using BLL;
using DAL.Models;
using Microsoft.Extensions.Configuration;

namespace Presentation

{
    public class FamilyTreeApp
    {
        private readonly FamilyTreeService _service;
        private readonly string _filePath;
        private static bool isTreeVisualizationOpen = false;

        public FamilyTreeApp(FamilyTreeService service, IConfiguration configuration)
        {
            _service = service;
            _filePath = configuration["FilePath"];
        }

        public void Run()
        {
            _service.LoadTree(_filePath);

            while (true)
            {
                AnsiConsole.Write(new Rule("Генеалогическое древо").RuleStyle("green"));
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Выберите действие:[/]")
                        .AddChoices(
                            "Добавить человека",
                            "Установить отношения",
                            "Показать ближайших родственников",
                            "Вывести дерево текстом",
                            "Показать схему дерева",
                            "Вычислить возраст предка при рождении потомка",
                            "Очистить дерево",
                            "Сохранить дерево",
                            "Выйти"));

                switch (choice)
                {
                    case "Добавить человека":
                        AddPerson();
                        break;
                    case "Установить отношения":
                        AddRelation();
                        break;
                    case "Показать ближайших родственников":
                        ShowClosestRelatives();
                        break;
                    case "Вычислить возраст предка при рождении потомка":
                        CalculateAgeAtBirth();
                        break;
                    case "Вывести дерево текстом":
                        ShowTree();
                        break;
                    case "Показать схему дерева":
                        ShowTreeVisualization();
                        break;
                    case "Очистить дерево":
                        ClearTree();
                        break;
                    case "Сохранить дерево":
                        SaveTree();
                        break;
                    case "Выйти":
                        _service.SaveTree(_filePath);
                        return;
                }
            }
        }

        private void AddPerson()
        {
            var name = AnsiConsole.Ask<string>("Введите ФИО:");
            var dob = AnsiConsole.Ask<DateTime>("Введите дату рождения (yyyy-MM-dd):");
            var gender = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Выберите пол:")
                    .AddChoices("М", "Ж"));

            var person = new Person(name, dob, gender); 
            _service.AddPerson(person);
            AnsiConsole.MarkupLine($"[green]Человек добавлен успешно! Short ID: {person.ShortId}[/]");
        }

        private void CalculateAgeAtBirth()
        {
            var parentId = AnsiConsole.Ask<string>("Введите Short ID предка:");
            var childId = AnsiConsole.Ask<string>("Введите Short ID потомка:");

            try
            {
                var age = _service.CalculateAgeAtBirth(parentId, childId);
                AnsiConsole.MarkupLine($"[green]Возраст предка при рождении потомка: {age} лет.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        }

        private void AddRelation()
        {
            var relationType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Выберите тип отношения:")
                    .AddChoices("Родитель", "Партнер"));

            string shortId1, shortId2;

            if (relationType == "Родитель")
            {
                shortId1 = AnsiConsole.Ask<string>("Введите Short ID родителя: ");
                shortId2 = AnsiConsole.Ask<string>("Введите Short ID ребенка: ");
            }
            else if (relationType == "Партнер")
            {
                shortId1 = AnsiConsole.Ask<string>("Введите Short ID первого партнера: ");
                shortId2 = AnsiConsole.Ask<string>("Введите Short ID второго партнера: ");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Неподдерживаемый тип отношения![/]");
                return;
            }

            try
            {
                _service.AddRelation(shortId1, shortId2, relationType);
                AnsiConsole.MarkupLine("[green]Отношение успешно установлено![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        }



        private void ShowClosestRelatives()
        {
            var shortId = AnsiConsole.Ask<string>("Введите Short ID человека:");

            try
            {
                var relatives = _service.GetClosestRelatives(shortId);
                if (relatives.Any())
                {
                    AnsiConsole.MarkupLine("[green]Ближайшие родственники:[/]");
                    foreach (var relative in relatives)
                    {
                        AnsiConsole.MarkupLine($"- {relative.FullName} (Short ID: {relative.ShortId})");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Нет ближайших родственников.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }
        }

        private void ShowTreeVisualization()
        {
            isTreeVisualizationOpen = true;

            try
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                var form = new WinFormsTree.TreeVisualizationForm(_service);
                form.FormClosed += (sender, args) =>
                {
                    isTreeVisualizationOpen = false;
                };

                System.Windows.Forms.Application.Run(form); 
            }
            catch (Exception ex)
            {
                isTreeVisualizationOpen = false;
                System.Windows.Forms.MessageBox.Show(
                    $"Перезапустите код для повторного построения дерева! При необходимости сохраните данные.",
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error
                );
            }
        }


        private void ShowTree()
        {
            var tree = _service.GetFamilyTreeAsText();
            AnsiConsole.WriteLine(tree);
        }

        private void SaveTree()
        {
            Console.WriteLine($"Сохранение в файл: {_filePath}");
            _service.SaveTree(_filePath);
            AnsiConsole.MarkupLine("[green]Древо сохранено.[/]");
        }

        private void ClearTree()
        {
            _service.ClearTree();
            AnsiConsole.MarkupLine("[green]Древо очищено.[/]");
        }
    }
}
