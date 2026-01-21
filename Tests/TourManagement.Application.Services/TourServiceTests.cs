using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TourManagement.Application.Services.Tests;

public class TourServiceTests
{
    private readonly Mock<ITourRepository> _mockTourRepository;
    private readonly Mock<ILogger<TourService>> _mockLogger;
    private readonly TourService _tourService;

    public TourServiceTests()
    {
        _mockTourRepository = new Mock<ITourRepository>();
        _mockLogger = new Mock<ILogger<TourService>>();
        _tourService = new TourService(_mockTourRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTours()
    {
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "Tour 1" },
            new Tour { Id = 2, TourName = "Tour 2" }
        };
        _mockTourRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tours);

        var result = await _tourService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<Tour>)result).Count);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsException_PropagatesException()
    {
        _mockTourRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _tourService.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsTour()
    {
        var tour = new Tour { Id = 1, TourName = "Test Tour" };
        _mockTourRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(tour);

        var result = await _tourService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Tour", result.TourName);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        _mockTourRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Tour?)null);

        var result = await _tourService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsException_PropagatesException()
    {
        _mockTourRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _tourService.GetByIdAsync(1));
    }

    [Fact]
    public async Task CreateAsync_ValidTour_CreatesTourSuccessfully()
    {
        var tour = new Tour { TourName = "New Tour", Place = "Paris", Days = 5, Price = 1000 };
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(tour);

        var result = await _tourService.CreateAsync(tour);

        Assert.NotNull(result);
        Assert.Equal("New Tour", result.TourName);
        Assert.True(result.IsActive);
        Assert.Equal("Admin", result.CreatedBy);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
    }

    [Fact]
    public async Task CreateAsync_SetsDefaultValues()
    {
        var tour = new Tour { TourName = "Test Tour" };
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(tour);

        var result = await _tourService.CreateAsync(tour);

        Assert.True(result.IsActive);
        Assert.Equal("Admin", result.CreatedBy);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_PropagatesException()
    {
        var tour = new Tour { TourName = "Test Tour" };
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _tourService.CreateAsync(tour));
    }

    [Fact]
    public async Task UpdateAsync_ValidTour_UpdatesTourSuccessfully()
    {
        var existingTour = new Tour { Id = 1, TourName = "Old Tour", Place = "Paris" };
        var updatedTourData = new Tour { TourName = "Updated Tour", Place = "London", Days = 7, Price = 1500 };
        _mockTourRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingTour);
        _mockTourRepository.Setup(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingTour);

        var result = await _tourService.UpdateAsync(1, updatedTourData);

        Assert.NotNull(result);
        Assert.Equal("Updated Tour", result.TourName);
        Assert.Equal("London", result.Place);
        Assert.Equal(7, result.Days);
        Assert.Equal("Admin", result.ModifiedBy);
    }

    [Fact]
    public async Task UpdateAsync_TourNotFound_ThrowsKeyNotFoundException()
    {
        var updatedTourData = new Tour { TourName = "Updated Tour" };
        _mockTourRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Tour?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _tourService.UpdateAsync(999, updatedTourData));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesModifiedDate()
    {
        var existingTour = new Tour { Id = 1, TourName = "Test Tour" };
        var updatedTourData = new Tour { TourName = "Updated Tour" };
        _mockTourRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingTour);
        _mockTourRepository.Setup(r => r.UpdateAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingTour);

        var result = await _tourService.UpdateAsync(1, updatedTourData);

        Assert.NotNull(result.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_DeletesTourSuccessfully()
    {
        _mockTourRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockTourRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _tourService.DeleteAsync(1);

        _mockTourRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_TourNotFound_ThrowsKeyNotFoundException()
    {
        _mockTourRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _tourService.DeleteAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_PropagatesException()
    {
        _mockTourRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _tourService.DeleteAsync(1));
    }

    [Fact]
    public async Task SearchAsync_ValidSearchTerm_ReturnsMatchingTours()
    {
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "Paris Tour" },
            new Tour { Id = 2, TourName = "Paris Adventure" }
        };
        _mockTourRepository.Setup(r => r.SearchAsync("Paris", It.IsAny<CancellationToken>())).ReturnsAsync(tours);

        var result = await _tourService.SearchAsync("Paris");

        Assert.NotNull(result);
        Assert.Equal(2, ((List<Tour>)result).Count);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmptyList()
    {
        _mockTourRepository.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Tour>());

        var result = await _tourService.SearchAsync("NonExistent");

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ThrowsException_PropagatesException()
    {
        _mockTourRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _tourService.SearchAsync("Test"));
    }

    [Fact]
    public void TourService_Constructor_InitializesCorrectly()
    {
        Assert.NotNull(_tourService);
    }

    [Fact]
    public async Task CreateAsync_NullTourName_StillCreates()
    {
        var tour = new Tour { TourName = null! };
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(tour);

        var result = await _tourService.CreateAsync(tour);

        Assert.NotNull(result);
    }
}
