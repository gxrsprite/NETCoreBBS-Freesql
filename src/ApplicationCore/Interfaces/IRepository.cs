using FreeSql;
using NetCoreBBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetCoreBBS.Interfaces
{
    public interface IRepository<T> where T: IEntity
    {
        T GetById(int id);
        ISelect<T> List();
        ISelect<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
    }
}
