using System;
using System.Threading.Tasks;
using JobPortalApi.Models.Requests;
using JobPortalApi.Providers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApi.Controllers
{
    public class OffersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OffersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOffers()
        {
            try
            {
                var userId = AuthenticatedUserId();
                var result = await _unitOfWork.GetAllOffersAsync(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 404);
            }
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetOfferById(Guid id)
        {
            try
            {
                var result = await _unitOfWork.GetOfferByIdAsync(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 404);
            }
        }

        [HttpGet]
        [Route("user/{userId:guid}")]
        public async Task<IActionResult> GetOffersForUser(Guid userId)
        {
            try
            {
                var result = await _unitOfWork.GetOffersForUserAsync(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 404);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddOffer([FromBody] OfferForCreation offer)
        {
            try
            {
                var userId = AuthenticatedUserId();
                var result = await _unitOfWork.AddOfferAsync(offer, userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 400);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> RemoveOffer(Guid id)
        {
            try
            {
                var result = await _unitOfWork.RemoveOfferAsync(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 400);
            }
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateOffer(Guid id, [FromBody] JsonPatchDocument patch)
        {
            try
            {
                var result = await _unitOfWork.UpdateOfferAsync(id, patch);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, 400);
            }
        }
    }
}