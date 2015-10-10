using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopCase.API.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        NorthwindEntities dbContext;

        public NorthwindEntities Init()
        {
            return dbContext ?? (dbContext = new NorthwindEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
