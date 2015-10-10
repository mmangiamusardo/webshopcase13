using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WebShopCase.API.Core.Controllers
{
    public static class Extensions
    {
        public static IEnumerable<string> GetErrorStrings(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors)
                                    .Select(v => v.ErrorMessage + " " + v.Exception).ToList();

        }

        public static IEnumerable<Error> GetErrors(this ModelStateDictionary modelState)
        {
            var result = from ms in modelState
                         where ms.Value.Errors.Any()
                         let fieldKey = ms.Key
                         let errors = ms.Value.Errors
                         from error in errors
                         select new Error(fieldKey, error.ErrorMessage);

            return result;
        }
    }

    public class Error
    {
        public Error(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }
    }
}
