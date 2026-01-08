using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Infrastructure.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Tests.Repositories;

public class BookingRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _dbContextOptions;
    private readonly Mock<ILogger<BookingRepository>> _mockLogger;

    public BookingRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<BookingRepository>>();
    }

    private TourManagementDbContext CreateContext()
    {
        return new TourManagementDbContext(_dbContextOptions);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyActiveBookings()
    {
        // Arrange
        using var context = CreateContext();
        context.Bookings.AddRange(
            new Booking { Id = 1, TourName = "Tour1", Email = "user1@test.com", IsActive = true },
            new Booking { Id = 2, TourName = "Tour2", Email = "user2@test.com", IsActive = true },
            new Booking { Id = 3, TourName = "Tour3", Email = "user3@test.com", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, b => Assert.True(b.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnBooking()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            Id = 1,
            TourName = "Test Tour",
            Email = "test@example.com",
            IsActive = true
        };
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Tour", result.TourName);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithInactiveBooking_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            Id = 1,
            TourName = "Inactive Booking",
            Email = "test@example.com",
            IsActive = false
        };
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserEmailAsync_ShouldReturnUserBookings()
    {
        // Arrange
        using var context = CreateContext();
        var email = "user@example.com";
        context.Bookings.AddRange(
            new Booking { Id = 1, Email = email, TourName = "Tour1", IsActive = true },
            new Booking { Id = 2, Email = email, TourName = "Tour2", IsActive = true },
            new Booking { Id = 3, Email = "other@example.com", TourName = "Tour3", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByUserEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, b => Assert.Equal(email, b.Email));
    }

    [Fact]
    public async Task GetByUserEmailAsync_WithNoBookings_ShouldReturnEmptyList()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByUserEmailAsync("nonexistent@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddBookingToDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BookingRepository(context, _mockLogger.Object);
        var booking = new Booking
        {
            TourName = "New Booking",
            Email = "user@example.com",
            FirstName = "John",
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(booking);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Booking", result.TourName);

        var savedBooking = await context.Bookings.FindAsync(result.Id);
        Assert.NotNull(savedBooking);
        Assert.Equal("New Booking", savedBooking.TourName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBooking()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            Id = 1,
            TourName = "Old Booking",
            Email = "user@example.com",
            IsActive = true
        };
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var repository = new BookingRepository(context, _mockLogger.Object);
        booking.TourName = "Updated Booking";

        // Act
        await repository.UpdateAsync(booking);

        // Assert
        var updatedBooking = await context.Bookings.FindAsync(1);
        Assert.NotNull(updatedBooking);
        Assert.Equal("Updated Booking", updatedBooking.TourName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveToFalse()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            Id = 1,
            TourName = "Booking to Delete",
            Email = "user@example.com",
            IsActive = true
        };
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deletedBooking = await context.Bookings.FindAsync(1);
        Assert.NotNull(deletedBooking);
        Assert.False(deletedBooking.IsActive);
        Assert.NotNull(deletedBooking.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingId_ShouldNotThrowException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act & Assert
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingActiveBooking_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            Id = 1,
            TourName = "Existing Booking",
            Email = "user@example.com",
            IsActive = true
        };
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingBooking_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveBooking_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            Id = 1,
            TourName = "Inactive Booking",
            Email = "user@example.com",
            IsActive = false
        };
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetByUserEmailAsync_ShouldOnlyReturnActiveBookings()
    {
        // Arrange
        using var context = CreateContext();
        var email = "user@example.com";
        context.Bookings.AddRange(
            new Booking { Id = 1, Email = email, TourName = "Tour1", IsActive = true },
            new Booking { Id = 2, Email = email, TourName = "Tour2", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new BookingRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByUserEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.True(result.First().IsActive);
    }
}
