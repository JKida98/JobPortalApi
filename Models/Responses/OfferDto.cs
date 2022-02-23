using System;

namespace JobPortalApi.Models.Responses
{
    public class OfferDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double HourlyPrice { get; set; }

        public Guid UserId { get; set; }
    }
}
