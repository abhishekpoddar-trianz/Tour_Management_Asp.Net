using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourManagement.Infrastructure.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TourManagement.Infrastructure.Repositories.Tests;

public class BookingRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _options;
    private readonly Mock<ILogger<BookingRepository>> _mockLogger;

    public BookingRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<BookingRepository>>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveBookingsOnly()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.AddRange(
            new Booking { Id = 1, TourName = "Active Booking", Email = "active@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Active", Place = "Paris", CreatedBy = "User" },
            new Booking { Id = 2, TourName = "Inactive Booking", Email = "inactive@test.com", IsActive = false, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Inactive", Place = "London", CreatedBy = "User" }
        );
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Active Booking", result.First().TourName);
    }

    [Fact]
    public async Task GetAllAsync_IncludesRelatedEntities()
    {
        using var context = new TourManagementDbContext(_options);
        var tour = new Tour { Id = 1, TourName = "Test Tour", IsActive = true, CreatedDate = DateTime.UtcNow, Place = "Paris", CreatedBy = "Admin", Locations = "Paris", Days = 5, Price = 100 };
        var user = new User { Id = 1, Email = "user@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, FirstName = "Test", PasswordHash = "hash", CreatedBy = "System" };
        context.Tours.Add(tour);
        context.Users.Add(user);
        context.Bookings.Add(new Booking { Id = 1, TourId = 1, UserId = 1, TourName = "Test Booking", Email = "user@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Test", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = (await repository.GetAllAsync()).ToList();

        Assert.Single(result);
        Assert.NotNull(result[0].Tour);
        Assert.NotNull(result[0].User);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsBooking()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.Add(new Booking { Id = 1, TourName = "Test Booking", Email = "test@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Test", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Booking", result.TourName);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new BookingRepository(context, _mockLogger.Object);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserEmailAsync_ValidEmail_ReturnsBookings()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.AddRange(
            new Booking { Id = 1, Email = "user@test.com", TourName = "Booking 1", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "User", Place = "Paris", CreatedBy = "User" },
            new Booking { Id = 2, Email = "user@test.com", TourName = "Booking 2", IsActive = true, BookingDate = DateTime.UtcNow.AddDays(-1), CreatedDate = DateTime.UtcNow, FirstName = "User", Place = "London", CreatedBy = "User" }
        );
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.GetByUserEmailAsync("user@test.com");

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByUserEmailAsync_NoBookings_ReturnsEmpty()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new BookingRepository(context, _mockLogger.Object);

        var result = await repository.GetByUserEmailAsync("nonexistent@test.com");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_ValidBooking_AddsBookingSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new BookingRepository(context, _mockLogger.Object);
        var booking = new Booking { TourName = "New Booking", Email = "new@test.com", FirstName = "New", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, Place = "Paris", CreatedBy = "User" };

        var result = await repository.AddAsync(booking);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Booking", result.TourName);
    }

    [Fact]
    public async Task UpdateAsync_ValidBooking_UpdatesBookingSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.Add(new Booking { Id = 1, TourName = "Old Booking", Email = "old@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Old", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var booking = await context.Bookings.FindAsync(1);
        booking!.TourName = "Updated Booking";
        var result = await repository.UpdateAsync(booking);

        Assert.NotNull(result);
        Assert.Equal("Updated Booking", result.TourName);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_SoftDeletesBooking()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.Add(new Booking { Id = 1, TourName = "Delete Booking", Email = "delete@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Delete", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        await repository.DeleteAsync(1);

        var booking = await context.Bookings.FindAsync(1);
        Assert.NotNull(booking);
        Assert.False(booking.IsActive);
        Assert.NotNull(booking.ModifiedDate);
    }

    [Fact]
    public async Task ExistsAsync_ValidId_ReturnsTrue()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.Add(new Booking { Id = 1, TourName = "Exists", Email = "exists@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "Exists", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_InvalidId_ReturnsFalse()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new BookingRepository(context, _mockLogger.Object);

        var result = await repository.ExistsAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_MatchingTourName_ReturnsBookings()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.AddRange(
            new Booking { Id = 1, TourName = "Paris Tour", Email = "user1@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "User1", Place = "Paris", CreatedBy = "User" },
            new Booking { Id = 2, TourName = "London Tour", Email = "user2@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "User2", Place = "London", CreatedBy = "User" }
        );
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Paris");

        Assert.Single(result);
        Assert.Equal("Paris Tour", result.First().TourName);
    }

    [Fact]
    public async Task SearchAsync_MatchingEmail_ReturnsBookings()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.Add(new Booking { Id = 1, TourName = "Test", Email = "search@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "User", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("search");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmpty()
    {
        using var context = new TourManagementDbContext(_options);
        context.Bookings.Add(new Booking { Id = 1, TourName = "Test", Email = "test@test.com", IsActive = true, BookingDate = DateTime.UtcNow, CreatedDate = DateTime.UtcNow, FirstName = "User", Place = "Paris", CreatedBy = "User" });
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("NonExistent");

        Assert.Empty(result);
    }
}
