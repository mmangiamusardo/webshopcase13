using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        #region Properties

            private NorthwindEntities dataContext;
            private readonly IDbSet<T> dbSet;

            protected IDbFactory DbFactory
            {
                get;
                private set;
            }

            protected NorthwindEntities DbContext
            {
                get { 
                    return dataContext ?? (dataContext = DbFactory.Init()); 
                }
            }

        #endregion

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation

            public virtual void Add(T entity)
            {
                dbSet.Add(entity);
            }

            public virtual void Update(T entity)
            {
                dbSet.Attach(entity);
                dataContext.Entry(entity).State = EntityState.Modified;
            }

            public virtual void Delete(T entity)
            {
                dbSet.Remove(entity);
            }

            public virtual void Delete(Expression<Func<T, bool>> where)
            {
                IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
                foreach (T obj in objects)
                    dbSet.Remove(obj);
            }

            public virtual IList<T> GetAll(
                params Expression<Func<T, object>>[] navigationProperties)
            {
                return dbSet
                    .IncludeMultiple(navigationProperties)
                    .AsNoTracking()
                    .ToList();
            }

            public virtual IList<T> GetMany(Expression<Func<T, bool>> where,
             params Expression<Func<T, object>>[] navigationProperties)
            {
                return dbSet
                      .IncludeMultiple(navigationProperties)
                      .AsNoTracking()
                      .Where(where)
                      .ToList();
            }

            public T GetSingle(Expression<Func<T, bool>> where,
              params Expression<Func<T, object>>[] navigationProperties)
            {
                return dbSet.IncludeMultiple(navigationProperties)
                    .AsNoTracking() 
                    .FirstOrDefault(where);
            }

        #endregion
    
    }
}
