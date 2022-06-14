using FreeSql;
using NetCoreBBS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBBS.Infrastructure
{
    public static class FreeSqlExtension
    {

        public static T Find<T>(this ISelect<T> selectquery, int id) where T : IEntity, new()
        {
            return selectquery.Where(d => d.Id == id).First();
        }

        public static Task<T> FindAsync<T>(this ISelect<T> selectquery, int id) where T : IEntity, new()
        {
            return selectquery.Where(d => d.Id == id).FirstAsync();
        }

        public static async Task<T> FindAsync<T>(this ISelect<T> selectquery, int? id) where T : class, IEntity, new()
        {
            if (id.HasValue)
                return await selectquery.Where(d => d.Id == id).FirstAsync();
            else
                return null;
        }
        public static T FirstOrDefault<T>(this ISelect<T> selectquery) where T : class, IEntity, new()
        {
            return selectquery.First();
        }
        public static T FirstOrDefault<T>(this ISelect<T> selectquery, Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            return selectquery.Where(predicate).First();
        }
        public static Task<T> FirstOrDefaultAsync<T>(this ISelect<T> selectquery, Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            return selectquery.Where(predicate).FirstAsync();
        }

        public static Task<T> FirstOrDefaultAsync<T>(this ISelect<T> selectquery) where T : IEntity, new()
        {
            return selectquery.FirstAsync();
        }

        public static ISelect<T> FilterByIds<T>(this ISelect<T> selectquery, Guid[] ids) where T : IEntity, new()
        {
            return selectquery.WhereDynamic(ids);
        }

        public static Task<List<T>> FindByIds<T>(this ISelect<T> selectquery, Guid[] ids) where T : IEntity, new()
        {
            return selectquery.WhereDynamic(ids).ToListAsync();
        }

        //https://github.com/dotnetcore/FreeSql/issues/278
        public static ISelect<T> OrderByField<T>(this ISelect<T> source, string property, bool isAscdening) where T : class
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            PropertyInfo pi = type.GetProperty(property);
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result;
            if (true == isAscdening)
            {
                result = typeof(ISelect<T>).GetMethods().
                    Single(method => method.Name == "OrderBy" && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1 && method.GetParameters().Length == 1).
                    MakeGenericMethod(type)
                    .Invoke(source, new object[] { lambda });
            }
            else
            {
                result = typeof(ISelect<T>).GetMethods().
                    Single(method => method.Name == "OrderByDescending" && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1 && method.GetParameters().Length == 1).
                       MakeGenericMethod(type)
                    .Invoke(source, new object[] { lambda });
            }
            return (ISelect<T>)result;
        }



        //DbSet<T>
        public static T FirstOrDefault<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select.First();
        }
        public static Task<T> FirstOrDefaultAsync<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select.FirstOrDefaultAsync();
        }
        public static T FirstOrDefault<T>(this DbSet<T> source, Expression<Func<T, bool>> whereexpr) where T : class, IEntity, new()
        {
            return source.Select.FirstOrDefault(whereexpr);
        }
        public static Task<T> FirstOrDefaultAsync<T>(this DbSet<T> source, Expression<Func<T, bool>> whereexpr) where T : class, IEntity, new()
        {
            return source.Select.FirstOrDefaultAsync(whereexpr);
        }
        public static T Find<T>(this DbSet<T> selectquery, int id) where T : class, IEntity, new()
        {
            return selectquery.Where(d => d.Id == id).First();
        }
        public static Task<T> FindAsync<T>(this DbSet<T> source, int id) where T : class, IEntity, new()
        {
            return source.Select.FindAsync(id);
        }

        public static Task<bool> AnyAsync<T>(this DbSet<T> source, Expression<Func<T, bool>> whereexpr) where T : class, IEntity, new()
        {
            return source.Select.AnyAsync(whereexpr);
        }
        public static List<T> ToList<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select.ToList();
        }
        public static Task<List<T>> ToListAsync<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select.ToListAsync();
        }
        public static ISelect<T> Include<T, TNavigate>(this DbSet<T> source, Expression<Func<T, TNavigate>> navigateSelector) where T : class, IEntity, new() where TNavigate : class
        {
            return source.Select.Include<TNavigate>(navigateSelector);
        }
        public static ISelect<T> AsNoTracking<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select;
        }
        public static ISelect<T> AsQueryable<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select;
        }
        public static long Count<T>(this DbSet<T> source) where T : class, IEntity, new()
        {
            return source.Select.Count();
        }

    }
}
