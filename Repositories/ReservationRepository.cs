using System;
using System.Linq;
using System.Threading.Tasks;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApi.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task FinishReservation(Guid reservationId);
}

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    private readonly DatabaseContext _context;

    public ReservationRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    public async Task FinishReservation(Guid reservationId)
    {
        var reservation = await _context.Reservations
            .Where(x => x.Id == reservationId)
            .FirstOrDefaultAsync();

        if (reservation != null)
        {
            reservation.IsFinished = true;
        }
    }
}