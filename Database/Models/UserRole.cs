using System;
using Microsoft.AspNetCore.Identity;

namespace JobPortalApi.Database.Models
{

    public class UserRole: IdentityRole<Guid>
    {
        public UserRole()
        {
        }

        public UserRole(string role): base(role)
        {
            
        }
    }
}
