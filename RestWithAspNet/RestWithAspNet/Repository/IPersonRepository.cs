using Microsoft.EntityFrameworkCore;
using RestWithAspNet.Data;
using RestWithAspNet.Model;
using RestWithAspNet.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithAspNet.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disabled(long id);
        List<Person> FindPersonByName(string firstName, string lastName);
        List<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);

    }

    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(MyDbContext context) : base(context) { }

        public Person Disabled(long id)
        {
            if (!_context.People.Any(p => p.Id == id)) return null;

            var user = _context.People.FirstOrDefault(p => p.Id.Equals(id));

            if (user != null)
            {
                user.Enabled = false;

                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return user;
        }

        public List<Person> FindPersonByName(string firstName, string lastName)
        {
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                return _context.People
                    .Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName))
                    .ToList();
            }
            else if (!string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {
                return _context.People
                    .Where(p => p.FirstName.Contains(firstName))
                    .ToList();
            }
            else if (string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                return _context.People
                    .Where(p => p.LastName.Contains(lastName))
                    .ToList();
            }
            else
            {
                return new List<Person>();
            }
        }

        public List<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var query = _context.People.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.FirstName.Contains(name));
            }

            if (sortDirection.Contains("desc"))
            {
                query = query.OrderByDescending(p => p.FirstName);
            }
            else
            {
                query = query.OrderBy(p => p.FirstName);
            }

            query = query.Skip(page);
            query = query.Take(pageSize);

            //var sql = query.ToQueryString();

            return query.ToList();
        }
    }

}
