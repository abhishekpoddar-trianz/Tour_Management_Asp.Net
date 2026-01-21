using Xunit;
using TourManagement.Domain.Entities;
using System;

namespace TourManagement.Domain.Entities.Tests;

public class BookingTests
{
    [Fact]
    public void Booking_Constructor_CreatesInstance()
    {
        var booking = new Booking();

        Assert.NotNull(booking);
        Assert.Equal(0, booking.Id);
        Assert.Equal(0, booking.TourId);
        Assert.Null(booking.UserId);
        Assert.Equal(string.Empty, booking.TourName);
        Assert.Equal(string.Empty, booking.Place);
        Assert.Equal(string.Empty, booking.Email);
        Assert.Equal(string.Empty, booking.FirstName);
    }

    [Fact]
    public void Booking_SetId_SetsValueCorrectly()
    {
        var booking = new Booking { Id = 1 };

        Assert.Equal(1, booking.Id);
    }

    [Fact]
    public void Booking_SetTourId_SetsValueCorrectly()
    {
        var booking = new Booking { TourId = 5 };

        Assert.Equal(5, booking.TourId);
    }

    [Fact]
    public void Booking_SetUserId_SetsValueCorrectly()
    {
        var booking = new Booking { UserId = 10 };

        Assert.Equal(10, booking.UserId);
    }

    [Fact]
    public void Booking_SetUserId_AllowsNull()
    {
        var booking = new Booking { UserId = null };

        Assert.Null(booking.UserId);
    }

    [Fact]
    public void Booking_SetTourName_SetsValueCorrectly()
    {
        var booking = new Booking { TourName = "Beach Tour" };

        Assert.Equal("Beach Tour", booking.TourName);
    }

    [Fact]
    public void Booking_SetPlace_SetsValueCorrectly()
    {
        var booking = new Booking { Place = "Hawaii" };

        Assert.Equal("Hawaii", booking.Place);
    }

    [Fact]
    public void Booking_SetEmail_SetsValueCorrectly()
    {
        var booking = new Booking { Email = "user@example.com" };

        Assert.Equal("user@example.com", booking.Email);
    }

    [Fact]
    public void Booking_SetFirstName_SetsValueCorrectly()
    {
        var booking = new Booking { FirstName = "Jane" };

        Assert.Equal("Jane", booking.FirstName);
    }

    [Fact]
    public void Booking_SetBookingDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var booking = new Booking { BookingDate = date };

        Assert.Equal(date, booking.BookingDate);
    }

    [Fact]
    public void Booking_SetCreatedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var booking = new Booking { CreatedDate = date };

        Assert.Equal(date, booking.CreatedDate);
    }

    [Fact]
    public void Booking_SetModifiedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var booking = new Booking { ModifiedDate = date };

        Assert.Equal(date, booking.ModifiedDate);
    }

    [Fact]
    public void Booking_SetModifiedDate_AllowsNull()
    {
        var booking = new Booking { ModifiedDate = null };

        Assert.Null(booking.ModifiedDate);
    }

    [Fact]
    public void Booking_SetIsActive_SetsValueCorrectly()
    {
        var booking = new Booking { IsActive = true };

        Assert.True(booking.IsActive);
    }

    [Fact]
    public void Booking_SetCreatedBy_SetsValueCorrectly()
    {
        var booking = new Booking { CreatedBy = "user@example.com" };

        Assert.Equal("user@example.com", booking.CreatedBy);
    }

    [Fact]
    public void Booking_SetModifiedBy_SetsValueCorrectly()
    {
        var booking = new Booking { ModifiedBy = "admin@example.com" };

        Assert.Equal("admin@example.com", booking.ModifiedBy);
    }

    [Fact]
    public void Booking_SetModifiedBy_AllowsNull()
    {
        var booking = new Booking { ModifiedBy = null };

        Assert.Null(booking.ModifiedBy);
    }

    [Fact]
    public void Booking_SetTour_SetsValueCorrectly()
    {
        var tour = new Tour { Id = 1, TourName = "Test Tour" };
        var booking = new Booking { Tour = tour };

        Assert.NotNull(booking.Tour);
        Assert.Equal(1, booking.Tour.Id);
    }

    [Fact]
    public void Booking_SetTour_AllowsNull()
    {
        var booking = new Booking { Tour = null };

        Assert.Null(booking.Tour);
    }

    [Fact]
    public void Booking_SetUser_SetsValueCorrectly()
    {
        var user = new User { Id = 1, Email = "test@example.com" };
        var booking = new Booking { User = user };

        Assert.NotNull(booking.User);
        Assert.Equal(1, booking.User.Id);
    }

    [Fact]
    public void Booking_SetUser_AllowsNull()
    {
        var booking = new Booking { User = null };

        Assert.Null(booking.User);
    }

    [Fact]
    public void Booking_AllProperties_CanBeSetTogether()
    {
        var date = DateTime.UtcNow;
        var tour = new Tour { Id = 1 };
        var user = new User { Id = 2 };

        var booking = new Booking
        {
            Id = 1,
            TourId = 5,
            UserId = 10,
            TourName = "Beach Tour",
            Place = "Hawaii",
            Email = "user@example.com",
            FirstName = "Jane",
            BookingDate = date,
            CreatedDate = date,
            ModifiedDate = date,
            IsActive = true,
            CreatedBy = "user@example.com",
            ModifiedBy = "admin@example.com",
            Tour = tour,
            User = user
        };

        Assert.Equal(1, booking.Id);
        Assert.Equal(5, booking.TourId);
        Assert.Equal(10, booking.UserId);
        Assert.Equal("Beach Tour", booking.TourName);
        Assert.Equal("Hawaii", booking.Place);
        Assert.Equal("user@example.com", booking.Email);
        Assert.Equal("Jane", booking.FirstName);
        Assert.Equal(date, booking.BookingDate);
        Assert.Equal(date, booking.CreatedDate);
        Assert.Equal(date, booking.ModifiedDate);
        Assert.True(booking.IsActive);
        Assert.Equal("user@example.com", booking.CreatedBy);
        Assert.Equal("admin@example.com", booking.ModifiedBy);
        Assert.NotNull(booking.Tour);
        Assert.NotNull(booking.User);
    }

    [Fact]
    public void Booking_IsActive_DefaultsToFalse()
    {
        var booking = new Booking();

        Assert.False(booking.IsActive);
    }

    [Fact]
    public void Booking_TourId_SupportsZero()
    {
        var booking = new Booking { TourId = 0 };

        Assert.Equal(0, booking.TourId);
    }
}
