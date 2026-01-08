using Xunit;
using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Tests.Entities;

public class BookingTests
{
    [Fact]
    public void Booking_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var booking = new Booking();

        // Assert
        Assert.NotNull(booking);
        Assert.Equal(0, booking.Id);
        Assert.Equal(0, booking.TourId);
        Assert.Equal(string.Empty, booking.TourName);
        Assert.Equal(string.Empty, booking.Place);
        Assert.Equal(string.Empty, booking.Email);
        Assert.Equal(string.Empty, booking.FirstName);
        Assert.Equal(string.Empty, booking.CreatedBy);
        Assert.Null(booking.ModifiedBy);
        Assert.Null(booking.ModifiedDate);
        Assert.Null(booking.Tour);
        Assert.Null(booking.User);
    }

    [Fact]
    public void Booking_SetId_ShouldSetIdProperty()
    {
        // Arrange
        var booking = new Booking();
        var id = 1;

        // Act
        booking.Id = id;

        // Assert
        Assert.Equal(id, booking.Id);
    }

    [Fact]
    public void Booking_SetTourId_ShouldSetTourIdProperty()
    {
        // Arrange
        var booking = new Booking();
        var tourId = 100;

        // Act
        booking.TourId = tourId;

        // Assert
        Assert.Equal(tourId, booking.TourId);
    }

    [Fact]
    public void Booking_SetTourName_ShouldSetTourNameProperty()
    {
        // Arrange
        var booking = new Booking();
        var tourName = "European Adventure";

        // Act
        booking.TourName = tourName;

        // Assert
        Assert.Equal(tourName, booking.TourName);
    }

    [Fact]
    public void Booking_SetPlace_ShouldSetPlaceProperty()
    {
        // Arrange
        var booking = new Booking();
        var place = "Paris";

        // Act
        booking.Place = place;

        // Assert
        Assert.Equal(place, booking.Place);
    }

    [Fact]
    public void Booking_SetEmail_ShouldSetEmailProperty()
    {
        // Arrange
        var booking = new Booking();
        var email = "customer@example.com";

        // Act
        booking.Email = email;

        // Assert
        Assert.Equal(email, booking.Email);
    }

    [Fact]
    public void Booking_SetFirstName_ShouldSetFirstNameProperty()
    {
        // Arrange
        var booking = new Booking();
        var firstName = "Jane";

        // Act
        booking.FirstName = firstName;

        // Assert
        Assert.Equal(firstName, booking.FirstName);
    }

    [Fact]
    public void Booking_SetCreatedDate_ShouldSetCreatedDateProperty()
    {
        // Arrange
        var booking = new Booking();
        var createdDate = DateTime.UtcNow;

        // Act
        booking.CreatedDate = createdDate;

        // Assert
        Assert.Equal(createdDate, booking.CreatedDate);
    }

    [Fact]
    public void Booking_SetModifiedDate_ShouldSetModifiedDateProperty()
    {
        // Arrange
        var booking = new Booking();
        var modifiedDate = DateTime.UtcNow;

        // Act
        booking.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, booking.ModifiedDate);
    }

    [Fact]
    public void Booking_SetIsActive_ShouldSetIsActiveProperty()
    {
        // Arrange
        var booking = new Booking();

        // Act
        booking.IsActive = true;

        // Assert
        Assert.True(booking.IsActive);
    }

    [Fact]
    public void Booking_SetCreatedBy_ShouldSetCreatedByProperty()
    {
        // Arrange
        var booking = new Booking();
        var createdBy = "system";

        // Act
        booking.CreatedBy = createdBy;

        // Assert
        Assert.Equal(createdBy, booking.CreatedBy);
    }

    [Fact]
    public void Booking_SetModifiedBy_ShouldSetModifiedByProperty()
    {
        // Arrange
        var booking = new Booking();
        var modifiedBy = "admin";

        // Act
        booking.ModifiedBy = modifiedBy;

        // Assert
        Assert.Equal(modifiedBy, booking.ModifiedBy);
    }

    [Fact]
    public void Booking_SetTour_ShouldSetTourNavigationProperty()
    {
        // Arrange
        var booking = new Booking();
        var tour = new Tour { Id = 1, TourName = "Test Tour" };

        // Act
        booking.Tour = tour;

        // Assert
        Assert.NotNull(booking.Tour);
        Assert.Equal(tour, booking.Tour);
    }

    [Fact]
    public void Booking_SetUser_ShouldSetUserNavigationProperty()
    {
        // Arrange
        var booking = new Booking();
        var user = new User { Email = "test@example.com" };

        // Act
        booking.User = user;

        // Assert
        Assert.NotNull(booking.User);
        Assert.Equal(user, booking.User);
    }

    [Fact]
    public void Booking_WithAllProperties_ShouldHoldAllValues()
    {
        // Arrange & Act
        var booking = new Booking
        {
            Id = 1,
            TourId = 10,
            TourName = "Amazing Tour",
            Place = "London",
            Email = "test@test.com",
            FirstName = "John",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "admin",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, booking.Id);
        Assert.Equal(10, booking.TourId);
        Assert.Equal("Amazing Tour", booking.TourName);
        Assert.Equal("London", booking.Place);
        Assert.Equal("test@test.com", booking.Email);
        Assert.Equal("John", booking.FirstName);
        Assert.True(booking.IsActive);
        Assert.Equal("admin", booking.CreatedBy);
        Assert.Equal("admin", booking.ModifiedBy);
    }
}
