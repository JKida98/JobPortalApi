using System;
using System.ComponentModel.DataAnnotations;

namespace JobPortalApi.Models.Requests
{
    public class ReservationLineForCreation
    {
        [Required]
        public Guid OfferId { get; set; }
    }
}

