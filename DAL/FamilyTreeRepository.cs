using DAL.Models;
using Newtonsoft.Json;

public class FamilyTreeRepository
{
    private readonly List<Person> _people = new(); 

    public void AddPerson(Person person) => _people.Add(person);

    public Person? FindPersonByShortId(string shortId)
    {
        return _people.FirstOrDefault(p => p.ShortId == shortId);
    }

    public List<Person> GetAllPeople() => _people;

    public void ClearTree() => _people.Clear();

    public void SaveTree(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(_people, settings);
        File.WriteAllText(filePath, json);
    }

    public void LoadTree(string filePath)
    {
        if (File.Exists(filePath))
        {
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = File.ReadAllText(filePath);
            var people = JsonConvert.DeserializeObject<List<Person>>(json, settings);

            if (people != null)
            {
                _people.Clear();
                _people.AddRange(people);

                foreach (var person in _people)
                {
                    person.Parents = person.ParentsIds
                        .Select(id => _people.FirstOrDefault(p => p.Id == id))
                        .Where(p => p != null)
                        .ToList();

                    person.Children = person.ChildrenIds
                        .Select(id => _people.FirstOrDefault(p => p.Id == id))
                        .Where(c => c != null)
                        .ToList();

                    person.Spouse = _people.FirstOrDefault(p => p.Id == person.SpouseId);
                }
            }
        }
    }
}
