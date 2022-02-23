using System;
using System.Threading.Tasks;
using JobPortalApi.Models;
using JobPortalApi.Models.Requests;
using JobPortalApi.Models.Responses;
using JobPortalApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApi.Controllers
{
    public class AuthController: BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegistration user)
        {
            
            try
            {
                ThrowIfInvalid();
                var result = await _unitOfWork.RegisterUserAsync(user);
                return Ok(new TokenDto { Token = result });
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 400);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserForLogin user)
        {
            try
            {
                ThrowIfInvalid();
                var result = await _unitOfWork.LoginUserAsync(user);
                return Ok(new TokenDto { Token = result });
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 400);
            }
            
        }       
    }
}
