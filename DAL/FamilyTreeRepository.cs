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
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            var json = File.ReadAllText(filePath);
            var people = JsonConvert.DeserializeObject<List<Person>>(json, settings);
            if (people != null) _people.AddRange(people);
        }
    }
}
