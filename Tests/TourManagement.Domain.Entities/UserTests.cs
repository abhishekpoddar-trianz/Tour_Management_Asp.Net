using Xunit;
using TourManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace TourManagement.Domain.Entities.Tests;

public class UserTests
{
    [Fact]
    public void User_Constructor_CreatesInstance()
    {
        var user = new User();

        Assert.NotNull(user);
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Equal(string.Empty, user.FirstName);
        Assert.NotNull(user.Bookings);
    }

    [Fact]
    public void User_SetId_SetsValueCorrectly()
    {
        var user = new User { Id = 1 };

        Assert.Equal(1, user.Id);
    }

    [Fact]
    public void User_SetEmail_SetsValueCorrectly()
    {
        var user = new User { Email = "test@example.com" };

        Assert.Equal("test@example.com", user.Email);
    }

    [Fact]
    public void User_SetPasswordHash_SetsValueCorrectly()
    {
        var user = new User { PasswordHash = "hashed_password" };

        Assert.Equal("hashed_password", user.PasswordHash);
    }

    [Fact]
    public void User_SetFirstName_SetsValueCorrectly()
    {
        var user = new User { FirstName = "John" };

        Assert.Equal("John", user.FirstName);
    }

    [Fact]
    public void User_SetLastName_SetsValueCorrectly()
    {
        var user = new User { LastName = "Doe" };

        Assert.Equal("Doe", user.LastName);
    }

    [Fact]
    public void User_SetLastName_AllowsNull()
    {
        var user = new User { LastName = null };

        Assert.Null(user.LastName);
    }

    [Fact]
    public void User_SetPhoneNumber_SetsValueCorrectly()
    {
        var user = new User { PhoneNumber = "123-456-7890" };

        Assert.Equal("123-456-7890", user.PhoneNumber);
    }

    [Fact]
    public void User_SetPhoneNumber_AllowsNull()
    {
        var user = new User { PhoneNumber = null };

        Assert.Null(user.PhoneNumber);
    }

    [Fact]
    public void User_SetCreatedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var user = new User { CreatedDate = date };

        Assert.Equal(date, user.CreatedDate);
    }

    [Fact]
    public void User_SetModifiedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var user = new User { ModifiedDate = date };

        Assert.Equal(date, user.ModifiedDate);
    }

    [Fact]
    public void User_SetModifiedDate_AllowsNull()
    {
        var user = new User { ModifiedDate = null };

        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void User_SetIsActive_SetsValueCorrectly()
    {
        var user = new User { IsActive = true };

        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_SetCreatedBy_SetsValueCorrectly()
    {
        var user = new User { CreatedBy = "System" };

        Assert.Equal("System", user.CreatedBy);
    }

    [Fact]
    public void User_SetModifiedBy_SetsValueCorrectly()
    {
        var user = new User { ModifiedBy = "Admin" };

        Assert.Equal("Admin", user.ModifiedBy);
    }

    [Fact]
    public void User_SetModifiedBy_AllowsNull()
    {
        var user = new User { ModifiedBy = null };

        Assert.Null(user.ModifiedBy);
    }

    [Fact]
    public void User_Bookings_InitializesAsEmptyList()
    {
        var user = new User();

        Assert.NotNull(user.Bookings);
        Assert.Empty(user.Bookings);
    }

    [Fact]
    public void User_Bookings_CanAddBooking()
    {
        var user = new User();
        var booking = new Booking { Id = 1 };
        user.Bookings.Add(booking);

        Assert.Single(user.Bookings);
        Assert.Contains(booking, user.Bookings);
    }

    [Fact]
    public void User_AllProperties_CanBeSetTogether()
    {
        var date = DateTime.UtcNow;
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "123-456-7890",
            CreatedDate = date,
            ModifiedDate = date,
            IsActive = true,
            CreatedBy = "System",
            ModifiedBy = "Admin"
        };

        Assert.Equal(1, user.Id);
        Assert.Equal("test@example.com", user.Email);
        Assert.Equal("hashed_password", user.PasswordHash);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("123-456-7890", user.PhoneNumber);
        Assert.Equal(date, user.CreatedDate);
        Assert.Equal(date, user.ModifiedDate);
        Assert.True(user.IsActive);
        Assert.Equal("System", user.CreatedBy);
        Assert.Equal("Admin", user.ModifiedBy);
    }

    [Fact]
    public void User_Email_SupportsEmptyString()
    {
        var user = new User { Email = string.Empty };

        Assert.Equal(string.Empty, user.Email);
    }

    [Fact]
    public void User_FirstName_SupportsEmptyString()
    {
        var user = new User { FirstName = string.Empty };

        Assert.Equal(string.Empty, user.FirstName);
    }

    [Fact]
    public void User_IsActive_DefaultsToFalse()
    {
        var user = new User();

        Assert.False(user.IsActive);
    }
}
