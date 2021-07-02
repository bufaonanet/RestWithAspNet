using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RestWithAspNet.Data;
using RestWithAspNet.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestWithAspNet.Repository.Generic
{
    public interface IRepository<T> : IDisposable where T : Entity
    {
        T Create(T item);
        T FindByID(long id);
        List<T> FindAll();
        T Update(T item);
        void Delete(long id);
        bool Exists(long id);

        List<T> FindWithPagedSearch(string query);
        int GetCount(string query);

        int CountTotal(Expression<Func<T, bool>> expression);

        List<T> GetData(
            Expression<Func<T, bool>> expression = null,            
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int? skip = null,
            int? take = null);
    }

    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly MyDbContext _context;
        private readonly DbSet<T> dataset;

        public Repository(MyDbContext context)
        {
            _context = context;
            dataset = _context.Set<T>();
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindByID(long id)
        {
            return dataset.FirstOrDefault(p => p.Id.Equals(id));
        }

        public T Create(T item)
        {
            try
            {
                dataset.Add(item);
                _context.SaveChanges();
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T Update(T item)
        {
            var result = dataset.FirstOrDefault(p => p.Id.Equals(item.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public void Delete(long id)
        {
            var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    dataset.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool Exists(long id)
        {
            return dataset.Any(p => p.Id.Equals(id));
        }

        public List<T> FindWithPagedSearch(string query)
        {
            return dataset.FromSqlRaw<T>(query).AsNoTracking().ToList();
        }

        public List<T> GetData(
            Expression<Func<T, bool>> expression = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, 
            int? skip = null, 
            int? take = null)
        {
            var query = dataset.AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (skip != null && skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null && take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.ToList();
        }

        public int CountTotal(Expression<Func<T, bool>> expression)
        {
            return dataset.Count(expression);
        }

        public int GetCount(string query)
        {
            var result = "";
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }

            return int.Parse(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }        
    }
}

