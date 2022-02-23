using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JobPortalApi.Database;
using JobPortalApi.Database.Models;
using JobPortalApi.Models.Requests;
using JobPortalApi.Models.Responses;
using JobPortalApi.Repositories;
using JobPortalApi.Services;
using Microsoft.AspNetCore.JsonPatch;

namespace JobPortalApi.Providers
{
    public interface IUnitOfWork
    {
        // Users
        Task<IList<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<bool> RemoveUserAsync(Guid id);
        Task<UserDto> UpdateUserAsync(Guid id, JsonPatchDocument patch);
        // Authorization
        Task<string> RegisterUserAsync(UserForRegistration user);
        Task<string> LoginUserAsync(UserForLogin user);
        // Offers
        Task<IList<OfferDto>> GetAllOffersAsync(Guid userId);
        Task<IList<OfferDto>> GetOffersForUserAsync(Guid userId);
        Task<OfferDto> GetOfferByIdAsync(Guid id);
        Task<OfferDto> AddOfferAsync(OfferForCreation offer, Guid userId);
        Task<bool> RemoveOfferAsync(Guid id);
        Task<Offer> UpdateOfferAsync(Guid id, JsonPatchDocument patch);
        // Reservations
        Task<ReservationDto> CreateReservationAsync(List<ReservationLineForCreation> reservationLines, Guid userId);
        Task<ReservationDto> GetReservationAsync(Guid reservationId);
        Task<IList<ReservationDto>> GetBoughtReservationsForUserAsync(Guid userId);
        Task<IList<ReservationDto>> GetSoldReservationsForUserAsync(Guid userId);
        // ReservationLines
        Task<IList<ReservationLineDto>> GetReservationLinesForReservationAsync(Guid reservationId);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        private readonly IMapper _mapper;

        private readonly IAuthService _authService;
        private IUnitOfWork _unitOfWorkImplementation;

        private IUserRepository Users { get; }

        private IOfferRepository Offers { get; }
        
        private IReservationLineRepository ReservationLines { get; }
        
        private IReservationRepository Reservations { get; }

        public UnitOfWork(DatabaseContext context, IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
            Users = new UserRepository(context);
            Offers = new OfferRepository(context);
            Reservations = new ReservationRepository(context);
            ReservationLines = new ReservationLineRepository(context);
        }

        private async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IList<UserDto>> GetAllUsersAsync()
        {
            var fetched = await Users.GetAllAsync();
            var filteredList = fetched.Where(x => !x.DeletedAt.HasValue).ToList();
            var mappedList = _mapper.Map<List<UserDto>>(filteredList);
            return mappedList;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var fetched = await Users.FindAsync(id);
            var mapped = _mapper.Map<UserDto>(fetched);
            return mapped;
        }

        public async Task<bool> RemoveUserAsync(Guid id)
        {
            await Users.DeleteAsync(id);
            await CompleteAsync();
            return true;
        }

        public async Task<UserDto> UpdateUserAsync(Guid id, JsonPatchDocument patch)
        {
            var result = await Users.UpdateAsync(id, patch);
            var mapped = _mapper.Map<UserDto>(result);
            await CompleteAsync();
            return mapped;
        }

        public async Task<IList<OfferDto>> GetAllOffersAsync(Guid userId)
        {
            var found = await Offers.FindAsync(x=> !x.DeletedAt.HasValue && x.UserId != userId);
            var mappedList = _mapper.Map<List<OfferDto>>(found);
            return mappedList;
        }

        public async Task<OfferDto> GetOfferByIdAsync(Guid id)
        {
            var found = await Offers.GetByIdAsync(id);
            var mapped = _mapper.Map<OfferDto>(found);
            return mapped;
        }

        public async Task<OfferDto> AddOfferAsync(OfferForCreation offer, Guid userId)
        {
            var mappedOffer = _mapper.Map<Offer>(offer);
            mappedOffer.UserId = userId;
            var result = await Offers.AddAsync(mappedOffer);
            var mapped = _mapper.Map<OfferDto>(result);
            await CompleteAsync();
            return mapped;
        }

        public async Task<bool> RemoveOfferAsync(Guid id)
        {
            await Offers.RemoveAsync(id);
            await CompleteAsync();
            return true;
        }

        public async Task<Offer> UpdateOfferAsync(Guid id, JsonPatchDocument patch)
        {
            await Offers.UpdateOfferAsync(id, patch);
            await CompleteAsync();
            var updated = await Offers.GetByIdAsync(id);
            return updated;
        }

        public async Task<string> RegisterUserAsync(UserForRegistration user)
        {
            return await _authService.RegisterUserAsync(user);
            
        }

        public async Task<string> LoginUserAsync(UserForLogin user)
        {
            return await _authService.LoginUserAsync(user);
        }

        public async Task<IList<OfferDto>> GetOffersForUserAsync(Guid userId)
        {
            var result = await Offers.FindAsync(x => x.UserId == userId && !x.DeletedAt.HasValue);
            return _mapper.Map<IList<OfferDto>>(result);
        }
        

        public async Task<ReservationDto> CreateReservationAsync(List<ReservationLineForCreation> reservationLines, Guid userId)
        {
            var listOfOffers = new List<Offer>();
            
            foreach (var element in reservationLines)
            {
                var offer = await Offers.GetByIdAsync(element.OfferId);
                listOfOffers.Add(offer);
            }

            var totalPrice = listOfOffers.Select(x => x.HourlyPrice).Sum();
            
            var reservation = new Reservation()
            {
                Status = ReservationStatus.Created,
                TotalPrice = totalPrice
            };

            var added = await Reservations.AddAsync(reservation);
            await CompleteAsync();
            
            foreach(var offer in listOfOffers)
            {
                var reservationLine = new ReservationLine()
                {
                    OfferId = offer.Id,
                    BuyerId = userId,
                    SellerId = offer.UserId,
                    Price = offer.HourlyPrice,
                    ReservationId = added.Id
                };
                await ReservationLines.AddAsync(reservationLine);
            }
            
            await CompleteAsync();
            var result = _mapper.Map<ReservationDto>(added);
            return result;

        }

        public async Task<ReservationDto> GetReservationAsync(Guid reservationId)
        {
            var result = await Reservations.GetByIdAsync(reservationId);
            return _mapper.Map<ReservationDto>(result);
        }

        public async Task<IList<ReservationDto>> GetBoughtReservationsForUserAsync(Guid userId)
        {
            var reservationLines = await ReservationLines.GetBoughtReservationLinesForUserAsync(userId);
            var result = reservationLines.Select(x => x.Reservation).Distinct().ToList();
            return _mapper.Map<IList<ReservationDto>>(result);
        }

        public async Task<IList<ReservationDto>> GetSoldReservationsForUserAsync(Guid userId)
        {
            var reservationLines = await ReservationLines.GetSoldReservationLinesForUserAsync(userId);
            var result = reservationLines.Select(x => x.Reservation).Distinct().ToList();
            return _mapper.Map<IList<ReservationDto>>(result);
        }

        public async Task<IList<ReservationLineDto>> GetReservationLinesForReservationAsync(Guid reservationId)
        {
            var result = await ReservationLines.GetReservationLinesForReservationAsync(reservationId);
            return _mapper.Map<IList<ReservationLineDto>>(result);
        }
    }
}