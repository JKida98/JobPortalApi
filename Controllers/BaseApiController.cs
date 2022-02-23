using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseApiController : ControllerBase
    {
        protected void ThrowIfInvalid()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid payload");
            }
        }

        protected JsonResult ErrorResponse(string msg, int status)
        {
            return new JsonResult(msg) { StatusCode = status};
        }

        protected Guid AuthenticatedUserId()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claimsIdentity) return Guid.Empty;
            var userId = claimsIdentity.Claims.First(x => x.Type == "Id").Value;
            return string.IsNullOrEmpty(userId) ? Guid.Empty : new Guid(userId);
        }
    }
}