using System;
using System.Threading.Tasks;
using JobPortalApi.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace JobPortalApi.Controllers
{
    public class UsersController : BaseApiController
    {

        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _unitOfWork.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 500);
            }
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var result = await _unitOfWork.GetUserByIdAsync(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 404);
            }
        }
      
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            try
            {
                var result = await _unitOfWork.RemoveUserAsync(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 404);

            }
        }        

        [HttpPatch]        
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] JsonPatchDocument patch)
        {
            if(patch == null)
            {
                return ErrorResponse("The patch document is null, no update was done", 500);
            }
            try
            {
                var result = await _unitOfWork.UpdateUserAsync(id, patch);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 404);
            }
        }
    }
}
