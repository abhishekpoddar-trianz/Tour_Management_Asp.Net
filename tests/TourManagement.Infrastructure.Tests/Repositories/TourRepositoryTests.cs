using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Infrastructure.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Tests.Repositories;

public class TourRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _dbContextOptions;
    private readonly Mock<ILogger<TourRepository>> _mockLogger;

    public TourRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<TourRepository>>();
    }

    private TourManagementDbContext CreateContext()
    {
        return new TourManagementDbContext(_dbContextOptions);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyActiveTours()
    {
        // Arrange
        using var context = CreateContext();
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "Tour1", IsActive = true },
            new Tour { Id = 2, TourName = "Tour2", IsActive = true },
            new Tour { Id = 3, TourName = "Tour3", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.True(t.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnTour()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { Id = 1, TourName = "Test Tour", IsActive = true };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

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
        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithInactiveTour_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { Id = 1, TourName = "Inactive Tour", IsActive = false };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTourToDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TourRepository(context, _mockLogger.Object);
        var tour = new Tour
        {
            TourName = "New Tour",
            Place = "Paris",
            Days = 7,
            Price = 1500.00m,
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(tour);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Tour", result.TourName);

        var savedTour = await context.Tours.FindAsync(result.Id);
        Assert.NotNull(savedTour);
        Assert.Equal("New Tour", savedTour.TourName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTour()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { Id = 1, TourName = "Old Tour", IsActive = true };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var repository = new TourRepository(context, _mockLogger.Object);
        tour.TourName = "Updated Tour";

        // Act
        await repository.UpdateAsync(tour);

        // Assert
        var updatedTour = await context.Tours.FindAsync(1);
        Assert.NotNull(updatedTour);
        Assert.Equal("Updated Tour", updatedTour.TourName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveToFalse()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { Id = 1, TourName = "Tour to Delete", IsActive = true };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deletedTour = await context.Tours.FindAsync(1);
        Assert.NotNull(deletedTour);
        Assert.False(deletedTour.IsActive);
        Assert.NotNull(deletedTour.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingId_ShouldNotThrowException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TourRepository(context, _mockLogger.Object);

        // Act & Assert
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingActiveTour_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { Id = 1, TourName = "Existing Tour", IsActive = true };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingTour_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveTour_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var tour = new Tour { Id = 1, TourName = "Inactive Tour", IsActive = false };
        context.Tours.Add(tour);
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTours()
    {
        // Arrange
        using var context = CreateContext();
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "European Adventure", Place = "Europe", IsActive = true },
            new Tour { Id = 2, TourName = "Asian Tour", Place = "Asia", IsActive = true },
            new Tour { Id = 3, TourName = "European Getaway", Place = "Europe", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("European");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, t => Assert.Contains("European", t.TourName));
    }

    [Fact]
    public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        using var context = CreateContext();
        context.Tours.Add(new Tour { Id = 1, TourName = "Asian Tour", IsActive = true });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("European");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldSearchInPlaceAndLocations()
    {
        // Arrange
        using var context = CreateContext();
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "Tour1", Place = "Paris", Locations = "France", IsActive = true },
            new Tour { Id = 2, TourName = "Tour2", Place = "London", Locations = "Paris", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("Paris");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_ShouldOnlyReturnActiveTours()
    {
        // Arrange
        using var context = CreateContext();
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "European Tour", IsActive = true },
            new Tour { Id = 2, TourName = "European Getaway", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("European");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.True(result.First().IsActive);
    }
}
