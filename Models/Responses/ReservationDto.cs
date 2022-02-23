using System;
using JobPortalApi.Database.Models;

namespace JobPortalApi.Models.Responses;

public class ReservationDto
{
    public Guid Id { get; set;  }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public ReservationStatus Status { get; set; }
        
    public double TotalPrice { get; set; }
}