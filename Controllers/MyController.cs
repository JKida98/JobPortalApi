using System;
using System.Threading.Tasks;
using JobPortalApi.Providers;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApi.Controllers;

public class MyController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public MyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Route("offers")]
    public async Task<IActionResult> GetMyOffers()
    {
        try
        {
            var userId = AuthenticatedUserId();
            var result = await _unitOfWork.GetOffersForUserAsync(userId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorResponse(e.Message, 404);
        }
    }

    [HttpGet]
    [Route("reservations/bought")]
    public async Task<IActionResult> GetMyBoughtReservations()
    {
        try
        {
            var userId = AuthenticatedUserId();
            var result = await _unitOfWork.GetBoughtReservationsForUserAsync(userId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorResponse(e.Message, 404);
        }
    }
    
    [HttpGet]
    [Route("reservations/sold")]
    public async Task<IActionResult> GetMySoldReservations()
    {
        try
        {
            var userId = AuthenticatedUserId();
            var result = await _unitOfWork.GetSoldReservationsForUserAsync(userId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorResponse(e.Message, 404);
        }
    }
}