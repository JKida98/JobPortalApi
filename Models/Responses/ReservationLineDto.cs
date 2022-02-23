using System;
using JobPortalApi.Database.Models;

namespace JobPortalApi.Models.Responses;

public class ReservationLineDto
{
    public Guid Id { get; set; }
    
    public Guid OfferId { get; set; }
    
    public OfferDto Offer { get; set; }
    
    public Guid BuyerId { get; set; }
    
    public UserDto Buyer { get; set; }
    
    public Guid SellerId { get; set; }
    
    public UserDto Seller { get; set; }
    
    public Guid ReservationId { get; set; }
    
    public ReservationDto Reservation { get; set; }
    
    public double Price { get; set; }
    
    public ReservationStatus Status { get; set; }
}