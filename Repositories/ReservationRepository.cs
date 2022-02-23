using JobPortalApi.Database;
using JobPortalApi.Database.Models;

namespace JobPortalApi.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    
}

public class ReservationRepository: GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(DatabaseContext context) : base(context)
    {
        
    }
}