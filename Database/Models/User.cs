using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace JobPortalApi.Database.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? DeletedAt { get; set; }

        public string FullName { get; set; }

        public string ProfileDescription { get; set; }
    }
}
