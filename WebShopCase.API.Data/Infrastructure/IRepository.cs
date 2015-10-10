using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        /*
        void Add(params T[] items);
        void Update(params T[] items);
        void Remove(params T[] items);
         */

        // Marks an entity as new
        void Add(T entity);
        
        // Marks an entity as modified
        void Update(T entity);
        
        // Marks an entity to be removed
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        
        // Gets all entities of type T
        //IEnumerable<T> GetAll();
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        
        // Gets entities using delegate
        //IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IList<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

        // Get an entity using delegate
        //T Get(Expression<Func<T, bool>> where);
        T GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
    }
}
