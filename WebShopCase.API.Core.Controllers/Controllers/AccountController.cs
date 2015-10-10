using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using WebShopCase.API.Domain;
using WebShopCase.API.Service;


namespace WebShopCase.API.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AppUserManager _userManager;
        private ICustomerService _customerService;

        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public AccountController(
            AppUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateResponse(
                    HttpStatusCode.BadRequest, ModelState.GetErrorStrings()));
            }

            var user = new AppUser() { UserName = model.Email, Email = model.Email };
            
            IdentityResult result = UserManager.Create(user, model.Password);
            if (result.Succeeded) 
            {
                _customerService.CreateCustomer(new Customer() { AppUserID = user.Id });
            }

            if (!result.Succeeded)
            {
                //return GetErrorResult(result);
                return ResponseMessage(Request.CreateResponse(
                    HttpStatusCode.BadRequest, result.Errors));
            }

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

            private IAuthenticationManager Authentication
            {
                get { return Request.GetOwinContext().Authentication; }
            }

            private IHttpActionResult GetErrorResult(IdentityResult result)
            {
                if (result == null)
                {
                    return InternalServerError();
                }

                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        // No ModelState errors are available to send, so just return an empty BadRequest.
                        return BadRequest();
                    }

                    return BadRequest(ModelState);
                }

                return null;
            }

        #endregion
    }
}