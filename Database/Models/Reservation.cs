using System;

namespace JobPortalApi.Database.Models
{
    public class Reservation: BaseModel
    {
        public ReservationStatus Status { get; set; }
        
        public double TotalPrice { get; set; }
    }
}
