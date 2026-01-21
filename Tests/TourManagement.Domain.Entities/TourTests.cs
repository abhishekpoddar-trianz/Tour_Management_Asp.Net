using Xunit;
using TourManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace TourManagement.Domain.Entities.Tests;

public class TourTests
{
    [Fact]
    public void Tour_Constructor_CreatesInstance()
    {
        var tour = new Tour();

        Assert.NotNull(tour);
        Assert.Equal(0, tour.Id);
        Assert.Equal(string.Empty, tour.TourName);
        Assert.Equal(string.Empty, tour.Place);
        Assert.Equal(0, tour.Days);
        Assert.Equal(0, tour.Price);
        Assert.NotNull(tour.Bookings);
    }

    [Fact]
    public void Tour_SetId_SetsValueCorrectly()
    {
        var tour = new Tour();
        tour.Id = 1;

        Assert.Equal(1, tour.Id);
    }

    [Fact]
    public void Tour_SetTourName_SetsValueCorrectly()
    {
        var tour = new Tour { TourName = "Test Tour" };

        Assert.Equal("Test Tour", tour.TourName);
    }

    [Fact]
    public void Tour_SetPlace_SetsValueCorrectly()
    {
        var tour = new Tour { Place = "New York" };

        Assert.Equal("New York", tour.Place);
    }

    [Fact]
    public void Tour_SetDays_SetsValueCorrectly()
    {
        var tour = new Tour { Days = 5 };

        Assert.Equal(5, tour.Days);
    }

    [Fact]
    public void Tour_SetPrice_SetsValueCorrectly()
    {
        var tour = new Tour { Price = 999.99m };

        Assert.Equal(999.99m, tour.Price);
    }

    [Fact]
    public void Tour_SetLocations_SetsValueCorrectly()
    {
        var tour = new Tour { Locations = "NYC, LA, SF" };

        Assert.Equal("NYC, LA, SF", tour.Locations);
    }

    [Fact]
    public void Tour_SetTourInfo_SetsValueCorrectly()
    {
        var tour = new Tour { TourInfo = "Amazing tour" };

        Assert.Equal("Amazing tour", tour.TourInfo);
    }

    [Fact]
    public void Tour_SetPicturePath_SetsValueCorrectly()
    {
        var tour = new Tour { PicturePath = "/images/tour.jpg" };

        Assert.Equal("/images/tour.jpg", tour.PicturePath);
    }

    [Fact]
    public void Tour_SetPicturePath_AllowsNull()
    {
        var tour = new Tour { PicturePath = null };

        Assert.Null(tour.PicturePath);
    }

    [Fact]
    public void Tour_SetCreatedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var tour = new Tour { CreatedDate = date };

        Assert.Equal(date, tour.CreatedDate);
    }

    [Fact]
    public void Tour_SetModifiedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var tour = new Tour { ModifiedDate = date };

        Assert.Equal(date, tour.ModifiedDate);
    }

    [Fact]
    public void Tour_SetModifiedDate_AllowsNull()
    {
        var tour = new Tour { ModifiedDate = null };

        Assert.Null(tour.ModifiedDate);
    }

    [Fact]
    public void Tour_SetIsActive_SetsValueCorrectly()
    {
        var tour = new Tour { IsActive = true };

        Assert.True(tour.IsActive);
    }

    [Fact]
    public void Tour_SetCreatedBy_SetsValueCorrectly()
    {
        var tour = new Tour { CreatedBy = "Admin" };

        Assert.Equal("Admin", tour.CreatedBy);
    }

    [Fact]
    public void Tour_SetModifiedBy_SetsValueCorrectly()
    {
        var tour = new Tour { ModifiedBy = "Admin" };

        Assert.Equal("Admin", tour.ModifiedBy);
    }

    [Fact]
    public void Tour_SetModifiedBy_AllowsNull()
    {
        var tour = new Tour { ModifiedBy = null };

        Assert.Null(tour.ModifiedBy);
    }

    [Fact]
    public void Tour_Bookings_InitializesAsEmptyList()
    {
        var tour = new Tour();

        Assert.NotNull(tour.Bookings);
        Assert.Empty(tour.Bookings);
    }

    [Fact]
    public void Tour_Bookings_CanAddBooking()
    {
        var tour = new Tour();
        var booking = new Booking { Id = 1 };
        tour.Bookings.Add(booking);

        Assert.Single(tour.Bookings);
        Assert.Contains(booking, tour.Bookings);
    }

    [Fact]
    public void Tour_AllProperties_CanBeSetTogether()
    {
        var date = DateTime.UtcNow;
        var tour = new Tour
        {
            Id = 1,
            TourName = "Test Tour",
            Place = "New York",
            Days = 5,
            Price = 999.99m,
            Locations = "NYC, LA, SF",
            TourInfo = "Amazing tour",
            PicturePath = "/images/tour.jpg",
            CreatedDate = date,
            ModifiedDate = date,
            IsActive = true,
            CreatedBy = "Admin",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, tour.Id);
        Assert.Equal("Test Tour", tour.TourName);
        Assert.Equal("New York", tour.Place);
        Assert.Equal(5, tour.Days);
        Assert.Equal(999.99m, tour.Price);
        Assert.Equal("NYC, LA, SF", tour.Locations);
        Assert.Equal("Amazing tour", tour.TourInfo);
        Assert.Equal("/images/tour.jpg", tour.PicturePath);
        Assert.Equal(date, tour.CreatedDate);
        Assert.Equal(date, tour.ModifiedDate);
        Assert.True(tour.IsActive);
        Assert.Equal("Admin", tour.CreatedBy);
        Assert.Equal("Admin", tour.ModifiedBy);
    }

    [Fact]
    public void Tour_Price_SupportsZeroValue()
    {
        var tour = new Tour { Price = 0 };

        Assert.Equal(0, tour.Price);
    }

    [Fact]
    public void Tour_Days_SupportsZeroValue()
    {
        var tour = new Tour { Days = 0 };

        Assert.Equal(0, tour.Days);
    }

    [Fact]
    public void Tour_Price_SupportsNegativeValue()
    {
        var tour = new Tour { Price = -100m };

        Assert.Equal(-100m, tour.Price);
    }
}
