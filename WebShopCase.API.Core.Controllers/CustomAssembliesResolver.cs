using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

using WebShopCase.API.Core.Controllers;

namespace WebShopCase.API.Core
{
    /// <summary>
    /// http://www.strathweb.com/2012/06/using-controllers-from-an-external-assembly-in-asp-net-web-api/
    /// </summary>
    public class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var baseAssemblies = base.GetAssemblies().ToList();
            var assemblies = new List<Assembly>(baseAssemblies) { typeof(ProductsController).Assembly };
            baseAssemblies.AddRange(assemblies);

            return baseAssemblies.Distinct().ToList();
        }
    }
}
