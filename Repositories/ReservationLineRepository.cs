using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobPortalApi.Repositories;

public interface IReservationLineRepository : IGenericRepository<ReservationLine>
{
    Task<ReservationLine> ChangeReservationLineStatusAsync(Guid reservationLineId, int currentStatus);
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

    public async Task<ReservationLine> ChangeReservationLineStatusAsync(Guid reservationLineId, int currentStatus)
    {
        var foundReservationLine = await _context.ReservationLines
            .Where(x => x.Id.Equals(reservationLineId) && x.Status == (ReservationStatus) currentStatus)
            .Include(x => x.Offer)
            .Include(x => x.Seller)
            .Include(x => x.Buyer)
            .Include(x => x.Reservation)
            .FirstAsync();

        if (foundReservationLine.Status == ReservationStatus.Paid) return foundReservationLine;

        foundReservationLine.Status = (ReservationStatus) currentStatus + 1;
        return foundReservationLine;
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