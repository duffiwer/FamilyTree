
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

            public Person(string fullName, DateTime dateOfBirth, string gender)
            {
                Id = Guid.NewGuid();
                FullName = fullName;
                DateOfBirth = dateOfBirth;
                Gender = gender;
            }
        }

    }





