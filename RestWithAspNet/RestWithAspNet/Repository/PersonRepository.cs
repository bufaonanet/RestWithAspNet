using RestWithAspNet.Data;
using RestWithAspNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithAspNet.Repository
{
    public interface IPersonRepository : IDisposable
    {
        Person Create(Person person);
        Person FindById(long id);
        List<Person> FindAll();
        Person Update(Person person);
        void Delete(long id);
        bool Exists(long id);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly MyDbContext _context;

        public PersonRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Person> FindAll()
        {
            return _context.People.ToList();
        }

        public Person FindById(long id)
        {
            return _context.People.Find(id);
        }

        public Person Create(Person person)
        {
            try
            {
                _context.Add(person);
                _context.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }

            return person;
        }


        public void Delete(long id)
        {
            var result = FindById(id);

            if (result != null)
            {
                try
                {
                    _context.People.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public Person Update(Person person)
        {
            if (!Exists(person.Id)) return null;

            var result = FindById(person.Id);

            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(person);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return person;
        }

        public bool Exists(long id)
        {
            return _context.People.Any(p => p.Id == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
