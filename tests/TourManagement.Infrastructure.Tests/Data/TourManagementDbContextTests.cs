using Xunit;
using Microsoft.EntityFrameworkCore;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Tests.Data;

public class TourManagementDbContextTests
{
    private readonly DbContextOptions<TourManagementDbContext> _dbContextOptions;

    public TourManagementDbContextTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private TourManagementDbContext CreateContext()
    {
        return new TourManagementDbContext(_dbContextOptions);
    }

    [Fact]
    public void DbContext_Constructor_ShouldCreateContext()
    {
        // Act
        using var context = CreateContext();

        // Assert
        Assert.NotNull(context);
        Assert.NotNull(context.Tours);
        Assert.NotNull(context.Users);
        Assert.NotNull(context.Bookings);
    }

    [Fact]
    public async Task DbContext_CanAddAndRetrieveTour()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour
        {
            TourName = "Test Tour",
            Place = "Paris",
            Days = 7,
            Price = 1500.00m,
            Locations = "France",
            TourInfo = "Test",
            IsActive = true
        };

        // Act
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        // Assert
        var retrievedTour = await context.Tours.FirstOrDefaultAsync(t => t.TourName == "Test Tour");
        Assert.NotNull(retrievedTour);
        Assert.Equal("Test Tour", retrievedTour.TourName);
        Assert.Equal("Paris", retrievedTour.Place);
    }

    [Fact]
    public async Task DbContext_CanAddAndRetrieveUser()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = "Password123",
            IsActive = true
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Assert
        var retrievedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(retrievedUser);
        Assert.Equal("test@example.com", retrievedUser.Email);
        Assert.Equal("Test", retrievedUser.FirstName);
    }

    [Fact]
    public async Task DbContext_CanAddAndRetrieveBooking()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking
        {
            TourName = "Test Booking",
            Email = "customer@example.com",
            FirstName = "Customer",
            Place = "Paris",
            IsActive = true
        };

        // Act
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        // Assert
        var retrievedBooking = await context.Bookings.FirstOrDefaultAsync(b => b.Email == "customer@example.com");
        Assert.NotNull(retrievedBooking);
        Assert.Equal("customer@example.com", retrievedBooking.Email);
        Assert.Equal("Test Booking", retrievedBooking.TourName);
    }

    [Fact]
    public async Task DbContext_CanUpdateTour()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { TourName = "Original", IsActive = true };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        // Act
        tour.TourName = "Updated";
        await context.SaveChangesAsync();

        // Assert
        var updatedTour = await context.Tours.FirstOrDefaultAsync(t => t.Id == tour.Id);
        Assert.NotNull(updatedTour);
        Assert.Equal("Updated", updatedTour.TourName);
    }

    [Fact]
    public async Task DbContext_CanDeleteTour()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { TourName = "To Delete", IsActive = true };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        // Act
        context.Tours.Remove(tour);
        await context.SaveChangesAsync();

        // Assert
        var deletedTour = await context.Tours.FirstOrDefaultAsync(t => t.Id == tour.Id);
        Assert.Null(deletedTour);
    }

    [Fact]
    public async Task DbContext_Tours_ShouldTrackChanges()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { TourName = "Test", IsActive = true };

        // Act
        context.Tours.Add(tour);
        var addedState = context.Entry(tour).State;
        await context.SaveChangesAsync();
        var unchangedState = context.Entry(tour).State;

        // Assert
        Assert.Equal(EntityState.Added, addedState);
        Assert.Equal(EntityState.Unchanged, unchangedState);
    }

    [Fact]
    public async Task DbContext_Users_ShouldTrackChanges()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User { Email = "test@test.com", FirstName = "Test", IsActive = true };

        // Act
        context.Users.Add(user);
        var addedState = context.Entry(user).State;
        await context.SaveChangesAsync();
        var unchangedState = context.Entry(user).State;

        // Assert
        Assert.Equal(EntityState.Added, addedState);
        Assert.Equal(EntityState.Unchanged, unchangedState);
    }

    [Fact]
    public async Task DbContext_Bookings_ShouldTrackChanges()
    {
        // Arrange
        using var context = CreateContext();
        var booking = new Booking { Email = "test@test.com", TourName = "Test", IsActive = true };

        // Act
        context.Bookings.Add(booking);
        var addedState = context.Entry(booking).State;
        await context.SaveChangesAsync();
        var unchangedState = context.Entry(booking).State;

        // Assert
        Assert.Equal(EntityState.Added, addedState);
        Assert.Equal(EntityState.Unchanged, unchangedState);
    }

    [Fact]
    public async Task DbContext_CanAddMultipleTours()
    {
        // Arrange
        using var context = CreateContext();
        var tours = new List<Tour>
        {
            new Tour { TourName = "Tour1", IsActive = true },
            new Tour { TourName = "Tour2", IsActive = true }
        };

        // Act
        context.Tours.AddRange(tours);
        await context.SaveChangesAsync();

        // Assert
        var count = await context.Tours.CountAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task DbContext_CanQueryToursWithLinq()
    {
        // Arrange
        using var context = CreateContext();
        context.Tours.AddRange(
            new Tour { TourName = "European Tour", IsActive = true },
            new Tour { TourName = "Asian Tour", IsActive = true }
        );
        await context.SaveChangesAsync();

        // Act
        var europeanTours = await context.Tours
            .Where(t => t.TourName.Contains("European"))
            .ToListAsync();

        // Assert
        Assert.Single(europeanTours);
        Assert.Equal("European Tour", europeanTours[0].TourName);
    }
}
