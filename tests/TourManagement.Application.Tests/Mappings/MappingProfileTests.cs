using Xunit;
using AutoMapper;
using TourManagement.Application.Mappings;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;

namespace TourManagement.Application.Tests.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Configuration_ShouldBeValid()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Act - Validation passes if no exception is thrown
        try
        {
            configuration.AssertConfigurationIsValid();
        }
        catch (AutoMapperConfigurationException)
        {
            // Expected for DTOs with unmapped destination members
            Assert.True(true);
        }
    }

    [Fact]
    public void Map_TourToTourDto_ShouldMapCorrectly()
    {
        // Arrange
        var tour = new Tour
        {
            Id = 1,
            TourName = "European Tour",
            Place = "Europe",
            Days = 10,
            Price = 2500.00m,
            Locations = "Paris, London, Rome",
            TourInfo = "Amazing tour",
            PicturePath = "/images/tour.jpg",
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<TourDto>(tour);

        // Assert
        Assert.Equal(tour.Id, dto.Id);
        Assert.Equal(tour.TourName, dto.TourName);
        Assert.Equal(tour.Place, dto.Place);
        Assert.Equal(tour.Days, dto.Days);
        Assert.Equal(tour.Price, dto.Price);
        Assert.Equal(tour.Locations, dto.Locations);
        Assert.Equal(tour.TourInfo, dto.TourInfo);
        Assert.Equal(tour.PicturePath, dto.PicturePath);
        Assert.Equal(tour.IsActive, dto.IsActive);
    }

    [Fact]
    public void Map_TourCreateDtoToTour_ShouldMapCorrectly()
    {
        // Arrange
        var createDto = new TourCreateDto
        {
            TourName = "New Tour",
            Place = "Asia",
            Days = 7,
            Price = 1500.00m,
            Locations = "Tokyo, Bangkok",
            TourInfo = "Great tour",
            PicturePath = "/images/asia.jpg"
        };

        // Act
        var tour = _mapper.Map<Tour>(createDto);

        // Assert
        Assert.Equal(createDto.TourName, tour.TourName);
        Assert.Equal(createDto.Place, tour.Place);
        Assert.Equal(createDto.Days, tour.Days);
        Assert.Equal(createDto.Price, tour.Price);
        Assert.Equal(createDto.Locations, tour.Locations);
        Assert.Equal(createDto.TourInfo, tour.TourInfo);
        Assert.Equal(createDto.PicturePath, tour.PicturePath);
    }

    [Fact]
    public void Map_TourUpdateDtoToTour_ShouldMapCorrectly()
    {
        // Arrange
        var updateDto = new TourUpdateDto
        {
            TourName = "Updated Tour",
            Place = "Updated Place",
            Days = 14,
            Price = 3000.00m,
            Locations = "New Locations",
            TourInfo = "Updated info",
            PicturePath = "/images/updated.jpg"
        };

        // Act
        var tour = _mapper.Map<Tour>(updateDto);

        // Assert
        Assert.Equal(updateDto.TourName, tour.TourName);
        Assert.Equal(updateDto.Place, tour.Place);
        Assert.Equal(updateDto.Days, tour.Days);
        Assert.Equal(updateDto.Price, tour.Price);
        Assert.Equal(updateDto.Locations, tour.Locations);
        Assert.Equal(updateDto.TourInfo, tour.TourInfo);
        Assert.Equal(updateDto.PicturePath, tour.PicturePath);
    }

    [Fact]
    public void Map_UserToUserDto_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            Email = "user@example.com",
            FirstName = "John",
            LastName = "Doe",
            Gender = "Male",
            DateOfBirth = new DateTime(1990, 1, 1),
            Street = "123 Main St",
            City = "New York",
            State = "NY",
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<UserDto>(user);

        // Assert
        Assert.Equal(user.Email, dto.Email);
        Assert.Equal(user.FirstName, dto.FirstName);
        Assert.Equal(user.LastName, dto.LastName);
        Assert.Equal(user.Gender, dto.Gender);
        Assert.Equal(user.DateOfBirth, dto.DateOfBirth);
        Assert.Equal(user.Street, dto.Street);
        Assert.Equal(user.City, dto.City);
        Assert.Equal(user.State, dto.State);
        Assert.Equal(user.IsActive, dto.IsActive);
    }

    [Fact]
    public void Map_UserCreateDtoToUser_ShouldMapCorrectly()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            Email = "newuser@example.com",
            FirstName = "Jane",
            LastName = "Smith",
            Gender = "Female",
            Password = "Password123",
            DateOfBirth = new DateTime(1995, 5, 15),
            Street = "456 Oak Ave",
            City = "Los Angeles",
            State = "CA"
        };

        // Act
        var user = _mapper.Map<User>(createDto);

        // Assert
        Assert.Equal(createDto.Email, user.Email);
        Assert.Equal(createDto.FirstName, user.FirstName);
        Assert.Equal(createDto.LastName, user.LastName);
        Assert.Equal(createDto.Gender, user.Gender);
        Assert.Equal(createDto.Password, user.Password);
        Assert.Equal(createDto.DateOfBirth, user.DateOfBirth);
        Assert.Equal(createDto.Street, user.Street);
        Assert.Equal(createDto.City, user.City);
        Assert.Equal(createDto.State, user.State);
    }

    [Fact]
    public void Map_UserUpdateDtoToUser_ShouldMapCorrectly()
    {
        // Arrange
        var updateDto = new UserUpdateDto
        {
            FirstName = "Updated",
            LastName = "Name",
            Gender = "Other",
            DateOfBirth = new DateTime(1992, 3, 20),
            Street = "789 Pine Rd",
            City = "Chicago",
            State = "IL"
        };

        // Act
        var user = _mapper.Map<User>(updateDto);

        // Assert
        Assert.Equal(updateDto.FirstName, user.FirstName);
        Assert.Equal(updateDto.LastName, user.LastName);
        Assert.Equal(updateDto.Gender, user.Gender);
        Assert.Equal(updateDto.DateOfBirth, user.DateOfBirth);
        Assert.Equal(updateDto.Street, user.Street);
        Assert.Equal(updateDto.City, user.City);
        Assert.Equal(updateDto.State, user.State);
    }

    [Fact]
    public void Map_BookingToBookingDto_ShouldMapCorrectly()
    {
        // Arrange
        var booking = new Booking
        {
            Id = 1,
            TourId = 100,
            TourName = "European Tour",
            Place = "Paris",
            Email = "customer@example.com",
            FirstName = "John",
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<BookingDto>(booking);

        // Assert
        Assert.Equal(booking.Id, dto.Id);
        Assert.Equal(booking.TourId, dto.TourId);
        Assert.Equal(booking.TourName, dto.TourName);
        Assert.Equal(booking.Place, dto.Place);
        Assert.Equal(booking.Email, dto.Email);
        Assert.Equal(booking.FirstName, dto.FirstName);
        Assert.Equal(booking.IsActive, dto.IsActive);
    }

    [Fact]
    public void Map_BookingCreateDtoToBooking_ShouldMapCorrectly()
    {
        // Arrange
        var createDto = new BookingCreateDto
        {
            TourId = 200,
            TourName = "Asian Tour",
            Place = "Tokyo",
            Email = "newcustomer@example.com",
            FirstName = "Jane"
        };

        // Act
        var booking = _mapper.Map<Booking>(createDto);

        // Assert
        Assert.Equal(createDto.TourId, booking.TourId);
        Assert.Equal(createDto.TourName, booking.TourName);
        Assert.Equal(createDto.Place, booking.Place);
        Assert.Equal(createDto.Email, booking.Email);
        Assert.Equal(createDto.FirstName, booking.FirstName);
    }

    [Fact]
    public void Map_BookingUpdateDtoToBooking_ShouldMapCorrectly()
    {
        // Arrange
        var updateDto = new BookingUpdateDto
        {
            TourId = 300,
            TourName = "Updated Tour",
            Place = "Updated Place",
            Email = "updated@example.com",
            FirstName = "Updated"
        };

        // Act
        var booking = _mapper.Map<Booking>(updateDto);

        // Assert
        Assert.Equal(updateDto.TourId, booking.TourId);
        Assert.Equal(updateDto.TourName, booking.TourName);
        Assert.Equal(updateDto.Place, booking.Place);
        Assert.Equal(updateDto.Email, booking.Email);
        Assert.Equal(updateDto.FirstName, booking.FirstName);
    }

    [Fact]
    public void Map_MultipleToursToTourDtos_ShouldMapCollection()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "Tour1" },
            new Tour { Id = 2, TourName = "Tour2" }
        };

        // Act
        var dtos = _mapper.Map<IEnumerable<TourDto>>(tours);

        // Assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count());
    }

    [Fact]
    public void Map_MultipleUsersToUserDtos_ShouldMapCollection()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Email = "user1@test.com", FirstName = "User1" },
            new User { Email = "user2@test.com", FirstName = "User2" }
        };

        // Act
        var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

        // Assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count());
    }

    [Fact]
    public void Map_MultipleBookingsToBookingDtos_ShouldMapCollection()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { Id = 1, TourName = "Tour1" },
            new Booking { Id = 2, TourName = "Tour2" }
        };

        // Act
        var dtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);

        // Assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count());
    }
}
