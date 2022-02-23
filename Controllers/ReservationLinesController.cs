using System;
using System.Threading.Tasks;
using JobPortalApi.Providers;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApi.Controllers;

public class ReservationLinesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationLinesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Route("{reservationId:guid}")]
    public async Task<IActionResult> GetReservationLinesForReservation(Guid reservationId)
    {
        try
        {
            var result = await _unitOfWork.GetReservationLinesForReservationAsync(reservationId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return ErrorResponse(e.Message, 400);
        }
    }
}