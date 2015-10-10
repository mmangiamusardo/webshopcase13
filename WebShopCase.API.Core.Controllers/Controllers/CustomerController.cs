using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http.Results;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using WebShopCase.API.Domain;
using WebShopCase.API.Service;

namespace WebShopCase.API.Core.Controllers
{
    [Authorize]
    public class CustomersController : ApiController
    {
        private ICustomerService _customerService;

        public CustomersController(ICustomerService customerService) 
        {
            _customerService = customerService;
        }

        [AcceptVerbs("GET")]
        [Route("api/customers/{username}")]
        public IHttpActionResult GetCustomerByUserName(string username)
        {
            var cus = _customerService.GetCustomerByUserName(username);
            if (cus == null)
            {
                return NotFound();
            }

            return Ok(cus);
        }

        [AcceptVerbs("PUT")]
        [Route("api/customers/")]
        public IHttpActionResult PutCustomer(CustomerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateResponse(
                    HttpStatusCode.BadRequest, ModelState.GetErrorStrings()));
            }

            var cus = new Customer()
            {
                CustomerID = dto.CustomerID,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                City = dto.City,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                AppUserID = dto.AppUserID
            };
                        

            _customerService.UpdateCustomer(cus);

            try
            {
                _customerService.SaveCustomer();
            }
            catch (DbUpdateConcurrencyException)
            {
               throw;
            }

            return Ok(dto.UserName);
            //return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
