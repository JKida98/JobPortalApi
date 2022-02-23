using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApi.Repositories;

public interface IReservationLineRepository : IGenericRepository<ReservationLine>
{
    Task<IList<ReservationLine>> GetReservationLinesForReservationAsync(Guid reservationId);
    Task<IList<ReservationLine>> GetBoughtReservationLinesForUserAsync(Guid userId);
    Task<IList<ReservationLine>> GetSoldReservationLinesForUserAsync(Guid userId);
}

public class ReservationLineRepository : GenericRepository<ReservationLine>, IReservationLineRepository
{
    private readonly DatabaseContext _context;

    public ReservationLineRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<IList<ReservationLine>> GetReservationLinesForReservationAsync(Guid reservationId)
    {
        var result = await _context.ReservationLines
            .Where(x => x.ReservationId.Equals(reservationId))
            .Include(x => x.Offer)
            .Include(x => x.Seller)
            .Include(x => x.Buyer)
            .Include(x => x.Reservation)
            .ToListAsync();
        return result;
    }

    public async Task<IList<ReservationLine>> GetBoughtReservationLinesForUserAsync(Guid userId)
    {
        var result = await _context.ReservationLines
            .Where(x => x.BuyerId.Equals(userId))
            .Include(x => x.Offer)
            .Include(x => x.Seller)
            .Include(x => x.Buyer)
            .Include(x => x.Reservation)
            .ToListAsync();
        return result;
    }

    public async Task<IList<ReservationLine>> GetSoldReservationLinesForUserAsync(Guid userId)
    {
        var result = await _context.ReservationLines
            .Where(x => x.SellerId.Equals(userId))
            .Include(x => x.Offer)
            .Include(x => x.Seller)
            .Include(x => x.Buyer)
            .Include(x => x.Reservation)
            .ToListAsync();
        return result;
    }
}