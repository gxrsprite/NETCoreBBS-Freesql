using FreeSql;
using NetCoreBBS.Entities;
using NetCoreBBS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCoreBBS.Infrastructure.Repositorys
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DataContext _dbContext;

        public Repository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual ISelect<T> List()
        {
            return _dbContext.Set<T>().Select;
        }

        public virtual ISelect<T> List(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                   .Where(predicate);
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Edit(T entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
