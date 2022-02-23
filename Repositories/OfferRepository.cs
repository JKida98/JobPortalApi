using System;
using System.Threading.Tasks;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace JobPortalApi.Repositories
{

    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task<bool> UpdateOfferAsync(Guid id, JsonPatchDocument patch);
    }

    public class OfferRepository: GenericRepository<Offer>, IOfferRepository
    {
        public OfferRepository(DatabaseContext context): base(context)
        {
            
            
        }

        public async Task<bool> UpdateOfferAsync(Guid id, JsonPatchDocument patch)
        {
            var found = await base.GetByIdAsync(id);
            patch.ApplyTo(found);
            return true;
        }
    }
}
