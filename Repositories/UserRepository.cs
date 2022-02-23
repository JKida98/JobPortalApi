

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApi.Repositories
{

    public interface IUserRepository
    {
        Task<bool> DeleteAsync(Guid id);
        Task<IList<User>> GetAllAsync();
        Task<User> UpdateAsync(Guid id, JsonPatchDocument patch);
        Task<User> FindAsync(Guid id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }
        
        public async Task<bool> DeleteAsync(Guid id)
        {
            var found = await FindAsync(id);
            found.DeletedAt = DateTimeOffset.Now;
            return true;
        }

        public async Task<User> FindAsync(Guid id)
        {
            var found = await _context.Users.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (found == null)
            {
                throw new NullReferenceException($"User with id: {id} was not found");
            }
            return found;
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> UpdateAsync(Guid id, JsonPatchDocument patch)
        {
            var found = await FindAsync(id);
            patch.ApplyTo(found);
            return found;
        }
    }

}