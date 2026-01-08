using AutoMapper;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Tour, TourDto>();
        CreateMap<TourCreateDto, Tour>();
        CreateMap<TourUpdateDto, Tour>();

        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        CreateMap<Booking, BookingDto>();
        CreateMap<BookingCreateDto, Booking>();
        CreateMap<BookingUpdateDto, Booking>();
    }
}
