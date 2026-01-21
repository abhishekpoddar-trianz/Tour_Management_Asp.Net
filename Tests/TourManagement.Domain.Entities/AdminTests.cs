using Xunit;
using TourManagement.Domain.Entities;
using System;

namespace TourManagement.Domain.Entities.Tests;

public class AdminTests
{
    [Fact]
    public void Admin_Constructor_CreatesInstance()
    {
        var admin = new Admin();

        Assert.NotNull(admin);
        Assert.Equal(0, admin.Id);
        Assert.Equal(string.Empty, admin.Username);
        Assert.Equal(string.Empty, admin.PasswordHash);
        Assert.Equal(string.Empty, admin.Email);
    }

    [Fact]
    public void Admin_SetId_SetsValueCorrectly()
    {
        var admin = new Admin { Id = 1 };

        Assert.Equal(1, admin.Id);
    }

    [Fact]
    public void Admin_SetUsername_SetsValueCorrectly()
    {
        var admin = new Admin { Username = "admin1" };

        Assert.Equal("admin1", admin.Username);
    }

    [Fact]
    public void Admin_SetPasswordHash_SetsValueCorrectly()
    {
        var admin = new Admin { PasswordHash = "hashed_admin_password" };

        Assert.Equal("hashed_admin_password", admin.PasswordHash);
    }

    [Fact]
    public void Admin_SetEmail_SetsValueCorrectly()
    {
        var admin = new Admin { Email = "admin@example.com" };

        Assert.Equal("admin@example.com", admin.Email);
    }

    [Fact]
    public void Admin_SetCreatedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var admin = new Admin { CreatedDate = date };

        Assert.Equal(date, admin.CreatedDate);
    }

    [Fact]
    public void Admin_SetModifiedDate_SetsValueCorrectly()
    {
        var date = DateTime.UtcNow;
        var admin = new Admin { ModifiedDate = date };

        Assert.Equal(date, admin.ModifiedDate);
    }

    [Fact]
    public void Admin_SetModifiedDate_AllowsNull()
    {
        var admin = new Admin { ModifiedDate = null };

        Assert.Null(admin.ModifiedDate);
    }

    [Fact]
    public void Admin_SetIsActive_SetsValueCorrectly()
    {
        var admin = new Admin { IsActive = true };

        Assert.True(admin.IsActive);
    }

    [Fact]
    public void Admin_SetCreatedBy_SetsValueCorrectly()
    {
        var admin = new Admin { CreatedBy = "System" };

        Assert.Equal("System", admin.CreatedBy);
    }

    [Fact]
    public void Admin_SetModifiedBy_SetsValueCorrectly()
    {
        var admin = new Admin { ModifiedBy = "SuperAdmin" };

        Assert.Equal("SuperAdmin", admin.ModifiedBy);
    }

    [Fact]
    public void Admin_SetModifiedBy_AllowsNull()
    {
        var admin = new Admin { ModifiedBy = null };

        Assert.Null(admin.ModifiedBy);
    }

    [Fact]
    public void Admin_AllProperties_CanBeSetTogether()
    {
        var date = DateTime.UtcNow;
        var admin = new Admin
        {
            Id = 1,
            Username = "admin1",
            PasswordHash = "hashed_admin_password",
            Email = "admin@example.com",
            CreatedDate = date,
            ModifiedDate = date,
            IsActive = true,
            CreatedBy = "System",
            ModifiedBy = "SuperAdmin"
        };

        Assert.Equal(1, admin.Id);
        Assert.Equal("admin1", admin.Username);
        Assert.Equal("hashed_admin_password", admin.PasswordHash);
        Assert.Equal("admin@example.com", admin.Email);
        Assert.Equal(date, admin.CreatedDate);
        Assert.Equal(date, admin.ModifiedDate);
        Assert.True(admin.IsActive);
        Assert.Equal("System", admin.CreatedBy);
        Assert.Equal("SuperAdmin", admin.ModifiedBy);
    }

    [Fact]
    public void Admin_Username_SupportsEmptyString()
    {
        var admin = new Admin { Username = string.Empty };

        Assert.Equal(string.Empty, admin.Username);
    }

    [Fact]
    public void Admin_Email_SupportsEmptyString()
    {
        var admin = new Admin { Email = string.Empty };

        Assert.Equal(string.Empty, admin.Email);
    }

    [Fact]
    public void Admin_IsActive_DefaultsToFalse()
    {
        var admin = new Admin();

        Assert.False(admin.IsActive);
    }

    [Fact]
    public void Admin_Username_SupportsSpecialCharacters()
    {
        var admin = new Admin { Username = "admin_123!@#" };

        Assert.Equal("admin_123!@#", admin.Username);
    }

    [Fact]
    public void Admin_Email_SupportsValidEmailFormat()
    {
        var admin = new Admin { Email = "admin.test+tag@example.com" };

        Assert.Equal("admin.test+tag@example.com", admin.Email);
    }
}
