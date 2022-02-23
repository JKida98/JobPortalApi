using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobPortalApi.Models.Requests;
using JobPortalApi.Providers;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApi.Controllers;

public class ReservationsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(List<ReservationLineForCreation> reservationLines)
    {
        try
        {
            var userId = AuthenticatedUserId();
            var result = await _unitOfWork.CreateReservationAsync(reservationLines, userId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorResponse(e.Message, 400);
        }
    }
}