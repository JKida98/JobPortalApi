using System;

namespace JobPortalApi.Database.Models
{
    public class Offer : BaseModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double HourlyPrice { get; set; }

        public Guid UserId { get; set; }        
    }
}
