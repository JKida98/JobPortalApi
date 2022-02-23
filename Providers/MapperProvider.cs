using AutoMapper;
using JobPortalApi.Database.Models;
using JobPortalApi.Models.Requests;
using JobPortalApi.Models.Responses;

namespace JobPortalApi.Providers
{
    public class MapperProvider : Profile
    {
        public MapperProvider()
        {
            CreateMap<User, UserDto>();
            CreateMap<Offer, OfferDto>();
            CreateMap<OfferForCreation, Offer>();
            CreateMap<ReservationLineForCreation, ReservationLine>();
            CreateMap<Reservation, ReservationDto>();
            CreateMap<ReservationLine, ReservationLineDto>();
        }
    }
}