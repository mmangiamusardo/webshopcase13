using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using WebShopCase.API.Domain;
using WebShopCase.API.Service;

namespace WebShopCase.API.Core.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private IOrderService _orderService;

        public OrdersController(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateResponse(
                     HttpStatusCode.BadRequest, ModelState.GetErrorStrings()));
            }

            try
            {
                order.RequiredDate = DateTime.Now;
                order.OrderDate = DateTime.Now;
                order.ShippedDate = DateTime.Now;

                _orderService.CreateOrder(order);

                //return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
                return Ok(order);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
            
        }
    }
}
