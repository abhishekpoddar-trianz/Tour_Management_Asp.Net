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

public class TourRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _options;
    private readonly Mock<ILogger<TourRepository>> _mockLogger;

    public TourRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<TourRepository>>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveToursOnly()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "Active Tour", IsActive = true, CreatedDate = DateTime.UtcNow },
            new Tour { Id = 2, TourName = "Inactive Tour", IsActive = false, CreatedDate = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Active Tour", result.First().TourName);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOrderedByCreatedDateDescending()
    {
        using var context = new TourManagementDbContext(_options);
        var oldDate = DateTime.UtcNow.AddDays(-5);
        var newDate = DateTime.UtcNow;
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "Old Tour", IsActive = true, CreatedDate = oldDate },
            new Tour { Id = 2, TourName = "New Tour", IsActive = true, CreatedDate = newDate }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = (await repository.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("New Tour", result[0].TourName);
        Assert.Equal("Old Tour", result[1].TourName);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsTour()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Test Tour", IsActive = true, CreatedDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Tour", result.TourName);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new TourRepository(context, _mockLogger.Object);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_InactiveTour_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Inactive Tour", IsActive = false, CreatedDate = DateTime.UtcNow });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ValidTour_AddsTourSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new TourRepository(context, _mockLogger.Object);
        var tour = new Tour { TourName = "New Tour", Place = "Paris", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" };

        var result = await repository.AddAsync(tour);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("New Tour", result.TourName);
    }

    [Fact]
    public async Task UpdateAsync_ValidTour_UpdatesTourSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Old Name", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var tour = await context.Tours.FindAsync(1);
        tour!.TourName = "Updated Name";
        var result = await repository.UpdateAsync(tour);

        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.TourName);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_SoftDeletesTour()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "To Delete", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        await repository.DeleteAsync(1);

        var tour = await context.Tours.FindAsync(1);
        Assert.NotNull(tour);
        Assert.False(tour.IsActive);
        Assert.NotNull(tour.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_InvalidId_DoesNotThrow()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new TourRepository(context, _mockLogger.Object);

        await repository.DeleteAsync(999);

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ValidId_ReturnsTrue()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Test Tour", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_InvalidId_ReturnsFalse()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new TourRepository(context, _mockLogger.Object);

        var result = await repository.ExistsAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_InactiveTour_ReturnsFalse()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Inactive", IsActive = false, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_MatchingTourName_ReturnsTours()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.AddRange(
            new Tour { Id = 1, TourName = "Paris Tour", Place = "France", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin", Locations = "Paris" },
            new Tour { Id = 2, TourName = "London Tour", Place = "UK", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin", Locations = "London" }
        );
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Paris");

        Assert.Single(result);
        Assert.Equal("Paris Tour", result.First().TourName);
    }

    [Fact]
    public async Task SearchAsync_MatchingPlace_ReturnsTours()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Test Tour", Place = "France", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin", Locations = "Paris" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("France");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_MatchingLocations_ReturnsTours()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Multi City", Place = "Europe", Locations = "Paris, Rome, Berlin", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Rome");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmpty()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Test Tour", Place = "Paris", IsActive = true, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin", Locations = "France" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("NonExistent");

        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ExcludesInactiveTours()
    {
        using var context = new TourManagementDbContext(_options);
        context.Tours.Add(new Tour { Id = 1, TourName = "Paris Tour", IsActive = false, CreatedDate = DateTime.UtcNow, CreatedBy = "Admin", Place = "France", Locations = "Paris" });
        await context.SaveChangesAsync();

        var repository = new TourRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Paris");

        Assert.Empty(result);
    }
}
