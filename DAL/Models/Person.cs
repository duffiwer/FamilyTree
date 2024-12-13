using Newtonsoft.Json;

namespace DAL.Models
{
    public class Person
    {
        public Guid Id { get; private set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public string Gender { get; set; } 

        public List<Person> Parents { get; set; } = new(); 
        public List<Person> Children { get; set; } = new(); 
        public Person? Spouse { get; set; } 

        public string ShortId => Id.ToString().Substring(0, 5); 

        [JsonIgnore]
        public List<Guid> ParentsIds => Parents.Select(p => p.Id).ToList();

        [JsonIgnore]
        public List<Guid> ChildrenIds => Children.Select(c => c.Id).ToList();

        [JsonIgnore]
        public Guid? SpouseId => Spouse?.Id;

        // Конструктор для восстановления из JSON
        [JsonConstructor]
        public Person(Guid id, string fullName, DateTime dateOfBirth, string gender)
        {
            Id = id;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }

        // Конструктор для создания нового объекта
        public Person(string fullName, DateTime dateOfBirth, string gender)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }
    }
}
