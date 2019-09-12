using MongoDB.Driver;
using PhoneBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBookAPI.BusinessLayer
{
    public class PhoneBookService
    {
        private readonly IMongoCollection<Person> _person;

        public PhoneBookService(IMongoConnection settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _person = database.GetCollection<Person>(settings.Name);
        }

        public List<Person> Get() =>
            _person.Find(person => true).ToList();

        public Person Get(string id) =>
            _person.Find<Person>(person => person.Id == id).FirstOrDefault();

        public Person Create(Person person)
        {
            _person.InsertOne(person);
            return person;
        }

        public void Update(string id, Person personIn) =>
            _person.ReplaceOne(person => person.Id == id, personIn);

        public void Remove(Person personIn) =>
            _person.DeleteOne(person => person.Id == personIn.Id);

        public void Remove(string id) =>
            _person.DeleteOne(person => person.Id == id);
    }
}
