using System;

namespace JobPortalApi.Database.Models;

public class ReservationLine: BaseModel
{
    public Guid OfferId { get; set; }
    
    public Offer Offer { get; set; }
    
    public Guid BuyerId { get; set; }
    
    public User Buyer { get; set; }
    
    public Guid SellerId { get; set; }
    
    public User Seller { get; set; }
    
    public Guid ReservationId { get; set; }
    
    public Reservation Reservation { get; set; }
    
    public double Price { get; set; }
    
    public ReservationStatus Status { get; set; }
}