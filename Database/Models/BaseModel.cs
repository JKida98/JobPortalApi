using System;

namespace JobPortalApi.Database.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? DeletedAt { get; set; }
    }
}
