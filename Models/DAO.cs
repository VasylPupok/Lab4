
using Lab4.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Lab4.Models
{
    internal class DAO
    {

        private readonly static string DB_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CStorage", "users");
        private ulong _freeId = 0;

        public DAO()
        {
            if (!Directory.Exists(DB_PATH))
            {
                Directory.CreateDirectory(DB_PATH);
                for (int i = 0; i < 50; i++)
                {
                    DateTime birthday = new DateTime(1970 + i, (i % 12) + 1, (i % 12) + 1);
                    string firstName = i < 10 ? "CustomUser0" + i : "CustomUser" + i;
                    string lastName = i < 10 ? "LastName0" + i : "LastName" + i;
                    addPerson(new Person(firstName, lastName, i + "@gmail.com", birthday));
                }
            }
            getPeople();
        }

        public IEnumerable<Pair<ulong, Person>> getPeople()
        {
            List<Pair<ulong, Person>> persons = new List<Pair<ulong, Person>>();
            foreach (var file in Directory.EnumerateFiles(DB_PATH))
            {
                string personInString = null;
                using (var reader = new StreamReader(file))
                {
                    personInString = reader.ReadToEnd();
                }
                string filename = Path.GetFileName(file);
                persons.Add(new Pair<ulong, Person>(
                    ulong.Parse(filename), 
                    JsonSerializer.Deserialize<Person>(personInString)
                    ));
            }
            return persons.OrderBy(elem => elem.First);
        }

        public void addPerson(Person person)
        {
            while (File.Exists(Path.Combine(DB_PATH, _freeId.ToString())))
            {
                ++_freeId;
            }
            var personInString = JsonSerializer.Serialize(person);
            using (var writer = new StreamWriter(Path.Combine(DB_PATH, _freeId++.ToString()), false))
            {
                writer.Write(personInString);
            }
        }

        public void deletePerson(ulong id)
        {
            File.Delete(Path.Combine(DB_PATH, id.ToString()));
        }

        public void updatePerson(ulong id, Person newPerson)
        {
            var personInString = JsonSerializer.Serialize(newPerson);
            using (var writer = new StreamWriter(Path.Combine(DB_PATH, id.ToString()), false))
            {
                writer.Write(personInString);
            }
        }

    }
}
