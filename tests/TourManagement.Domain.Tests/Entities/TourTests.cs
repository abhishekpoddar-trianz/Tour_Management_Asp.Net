using Xunit;
using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Tests.Entities;

public class TourTests
{
    [Fact]
    public void Tour_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var tour = new Tour();

        // Assert
        Assert.NotNull(tour);
        Assert.Equal(0, tour.Id);
        Assert.Equal(string.Empty, tour.TourName);
        Assert.Equal(string.Empty, tour.Place);
        Assert.Equal(0, tour.Days);
        Assert.Equal(0m, tour.Price);
        Assert.Equal(string.Empty, tour.Locations);
        Assert.Equal(string.Empty, tour.TourInfo);
        Assert.Null(tour.PicturePath);
        Assert.Equal(string.Empty, tour.CreatedBy);
        Assert.Null(tour.ModifiedBy);
        Assert.Null(tour.ModifiedDate);
        Assert.NotNull(tour.Bookings);
        Assert.Empty(tour.Bookings);
    }

    [Fact]
    public void Tour_SetId_ShouldSetIdProperty()
    {
        // Arrange
        var tour = new Tour();
        var id = 1;

        // Act
        tour.Id = id;

        // Assert
        Assert.Equal(id, tour.Id);
    }

    [Fact]
    public void Tour_SetTourName_ShouldSetTourNameProperty()
    {
        // Arrange
        var tour = new Tour();
        var tourName = "European Adventure";

        // Act
        tour.TourName = tourName;

        // Assert
        Assert.Equal(tourName, tour.TourName);
    }

    [Fact]
    public void Tour_SetPlace_ShouldSetPlaceProperty()
    {
        // Arrange
        var tour = new Tour();
        var place = "Europe";

        // Act
        tour.Place = place;

        // Assert
        Assert.Equal(place, tour.Place);
    }

    [Fact]
    public void Tour_SetDays_ShouldSetDaysProperty()
    {
        // Arrange
        var tour = new Tour();
        var days = 7;

        // Act
        tour.Days = days;

        // Assert
        Assert.Equal(days, tour.Days);
    }

    [Fact]
    public void Tour_SetPrice_ShouldSetPriceProperty()
    {
        // Arrange
        var tour = new Tour();
        var price = 1500.50m;

        // Act
        tour.Price = price;

        // Assert
        Assert.Equal(price, tour.Price);
    }

    [Fact]
    public void Tour_SetLocations_ShouldSetLocationsProperty()
    {
        // Arrange
        var tour = new Tour();
        var locations = "Paris, London, Rome";

        // Act
        tour.Locations = locations;

        // Assert
        Assert.Equal(locations, tour.Locations);
    }

    [Fact]
    public void Tour_SetTourInfo_ShouldSetTourInfoProperty()
    {
        // Arrange
        var tour = new Tour();
        var tourInfo = "Amazing tour across Europe";

        // Act
        tour.TourInfo = tourInfo;

        // Assert
        Assert.Equal(tourInfo, tour.TourInfo);
    }

    [Fact]
    public void Tour_SetPicturePath_ShouldSetPicturePathProperty()
    {
        // Arrange
        var tour = new Tour();
        var picturePath = "/images/tour1.jpg";

        // Act
        tour.PicturePath = picturePath;

        // Assert
        Assert.Equal(picturePath, tour.PicturePath);
    }

    [Fact]
    public void Tour_SetCreatedDate_ShouldSetCreatedDateProperty()
    {
        // Arrange
        var tour = new Tour();
        var createdDate = DateTime.UtcNow;

        // Act
        tour.CreatedDate = createdDate;

        // Assert
        Assert.Equal(createdDate, tour.CreatedDate);
    }

    [Fact]
    public void Tour_SetModifiedDate_ShouldSetModifiedDateProperty()
    {
        // Arrange
        var tour = new Tour();
        var modifiedDate = DateTime.UtcNow;

        // Act
        tour.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, tour.ModifiedDate);
    }

    [Fact]
    public void Tour_SetIsActive_ShouldSetIsActiveProperty()
    {
        // Arrange
        var tour = new Tour();

        // Act
        tour.IsActive = true;

        // Assert
        Assert.True(tour.IsActive);
    }

    [Fact]
    public void Tour_SetCreatedBy_ShouldSetCreatedByProperty()
    {
        // Arrange
        var tour = new Tour();
        var createdBy = "admin";

        // Act
        tour.CreatedBy = createdBy;

        // Assert
        Assert.Equal(createdBy, tour.CreatedBy);
    }

    [Fact]
    public void Tour_SetModifiedBy_ShouldSetModifiedByProperty()
    {
        // Arrange
        var tour = new Tour();
        var modifiedBy = "admin";

        // Act
        tour.ModifiedBy = modifiedBy;

        // Assert
        Assert.Equal(modifiedBy, tour.ModifiedBy);
    }

    [Fact]
    public void Tour_AddBooking_ShouldAddBookingToCollection()
    {
        // Arrange
        var tour = new Tour();
        var booking = new Booking { Id = 1, Email = "test@example.com" };

        // Act
        tour.Bookings.Add(booking);

        // Assert
        Assert.Single(tour.Bookings);
        Assert.Contains(booking, tour.Bookings);
    }

    [Fact]
    public void Tour_MultipleBookings_ShouldMaintainAllBookings()
    {
        // Arrange
        var tour = new Tour();
        var booking1 = new Booking { Id = 1, Email = "test1@example.com" };
        var booking2 = new Booking { Id = 2, Email = "test2@example.com" };

        // Act
        tour.Bookings.Add(booking1);
        tour.Bookings.Add(booking2);

        // Assert
        Assert.Equal(2, tour.Bookings.Count);
        Assert.Contains(booking1, tour.Bookings);
        Assert.Contains(booking2, tour.Bookings);
    }

    [Fact]
    public void Tour_WithAllProperties_ShouldHoldAllValues()
    {
        // Arrange & Act
        var tour = new Tour
        {
            Id = 1,
            TourName = "Asia Tour",
            Place = "Asia",
            Days = 14,
            Price = 2500.00m,
            Locations = "Tokyo, Bangkok, Singapore",
            TourInfo = "Comprehensive Asia tour",
            PicturePath = "/images/asia.jpg",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = "system",
            ModifiedBy = "admin"
        };

        // Assert
        Assert.Equal(1, tour.Id);
        Assert.Equal("Asia Tour", tour.TourName);
        Assert.Equal("Asia", tour.Place);
        Assert.Equal(14, tour.Days);
        Assert.Equal(2500.00m, tour.Price);
        Assert.Equal("Tokyo, Bangkok, Singapore", tour.Locations);
        Assert.Equal("Comprehensive Asia tour", tour.TourInfo);
        Assert.Equal("/images/asia.jpg", tour.PicturePath);
        Assert.True(tour.IsActive);
    }

    [Fact]
    public void Tour_SetZeroDays_ShouldAllowZero()
    {
        // Arrange
        var tour = new Tour();

        // Act
        tour.Days = 0;

        // Assert
        Assert.Equal(0, tour.Days);
    }

    [Fact]
    public void Tour_SetNegativePrice_ShouldAllowNegative()
    {
        // Arrange
        var tour = new Tour();

        // Act
        tour.Price = -100m;

        // Assert
        Assert.Equal(-100m, tour.Price);
    }
}
