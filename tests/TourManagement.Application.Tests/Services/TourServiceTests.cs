using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.Application.Tests.Services;

public class TourServiceTests
{
    private readonly Mock<ITourRepository> _mockTourRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TourService>> _mockLogger;
    private readonly TourService _tourService;

    public TourServiceTests()
    {
        _mockTourRepository = new Mock<ITourRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TourService>>();
        _tourService = new TourService(_mockTourRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTours()
    {
        // Arrange
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "Tour1" },
            new Tour { Id = 2, TourName = "Tour2" }
        };
        var tourDtos = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour1" },
            new TourDto { Id = 2, TourName = "Tour2" }
        };

        _mockTourRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tours);
        _mockMapper.Setup(m => m.Map<IEnumerable<TourDto>>(tours))
            .Returns(tourDtos);

        // Act
        var result = await _tourService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTourRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnTour()
    {
        // Arrange
        var tourId = 1;
        var tour = new Tour { Id = tourId, TourName = "Test Tour" };
        var tourDto = new TourDto { Id = tourId, TourName = "Test Tour" };

        _mockTourRepository.Setup(r => r.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tour);
        _mockMapper.Setup(m => m.Map<TourDto>(tour))
            .Returns(tourDto);

        // Act
        var result = await _tourService.GetByIdAsync(tourId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tourId, result.Id);
        _mockTourRepository.Verify(r => r.GetByIdAsync(tourId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var tourId = 999;
        _mockTourRepository.Setup(r => r.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tour?)null);

        // Act
        var result = await _tourService.GetByIdAsync(tourId);

        // Assert
        Assert.Null(result);
        _mockTourRepository.Verify(r => r.GetByIdAsync(tourId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidTour_ShouldCreateTour()
    {
        // Arrange
        var createDto = new TourCreateDto
        {
            TourName = "New Tour",
            Place = "Paris",
            Days = 7,
            Price = 1500.00m
        };
        var tour = new Tour { Id = 1, TourName = createDto.TourName, Place = createDto.Place };
        var tourDto = new TourDto { Id = 1, TourName = createDto.TourName, Place = createDto.Place };

        _mockMapper.Setup(m => m.Map<Tour>(createDto))
            .Returns(tour);
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tour);
        _mockMapper.Setup(m => m.Map<TourDto>(tour))
            .Returns(tourDto);

        // Act
        var result = await _tourService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.TourName, result.TourName);
        _mockTourRepository.Verify(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingTour_ShouldUpdateTour()
    {
        // Arrange
        var tourId = 1;
        var updateDto = new TourUpdateDto { TourName = "Updated Tour", Price = 2000.00m };
        var existingTour = new Tour { Id = tourId, TourName = "Old Tour" };

        _mockTourRepository.Setup(r => r.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTour);
        _mockMapper.Setup(m => m.Map(updateDto, existingTour))
            .Returns(existingTour);

        // Act
        await _tourService.UpdateAsync(tourId, updateDto);

        // Assert
        _mockTourRepository.Verify(r => r.UpdateAsync(existingTour, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingTour_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var tourId = 999;
        var updateDto = new TourUpdateDto { TourName = "Updated Tour" };

        _mockTourRepository.Setup(r => r.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tour?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _tourService.UpdateAsync(tourId, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithExistingTour_ShouldDeleteTour()
    {
        // Arrange
        var tourId = 1;
        _mockTourRepository.Setup(r => r.ExistsAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _tourService.DeleteAsync(tourId);

        // Assert
        _mockTourRepository.Verify(r => r.DeleteAsync(tourId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingTour_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var tourId = 999;
        _mockTourRepository.Setup(r => r.ExistsAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _tourService.DeleteAsync(tourId));
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingTours()
    {
        // Arrange
        var searchTerm = "europe";
        var tours = new List<Tour>
        {
            new Tour { Id = 1, TourName = "European Tour", Place = "Europe" }
        };
        var tourDtos = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "European Tour", Place = "Europe" }
        };

        _mockTourRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tours);
        _mockMapper.Setup(m => m.Map<IEnumerable<TourDto>>(tours))
            .Returns(tourDtos);

        // Act
        var result = await _tourService.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockTourRepository.Verify(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ShouldCallRepository()
    {
        // Arrange
        var searchTerm = "";
        var tours = new List<Tour>();
        var tourDtos = new List<TourDto>();

        _mockTourRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tours);
        _mockMapper.Setup(m => m.Map<IEnumerable<TourDto>>(tours))
            .Returns(tourDtos);

        // Act
        var result = await _tourService.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockTourRepository.Verify(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithZeroDays_ShouldCreateTour()
    {
        // Arrange
        var createDto = new TourCreateDto { TourName = "Short Tour", Days = 0 };
        var tour = new Tour { Id = 1, TourName = "Short Tour", Days = 0 };
        var tourDto = new TourDto { Id = 1, TourName = "Short Tour", Days = 0 };

        _mockMapper.Setup(m => m.Map<Tour>(createDto)).Returns(tour);
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(tour);
        _mockMapper.Setup(m => m.Map<TourDto>(tour)).Returns(tourDto);

        // Act
        var result = await _tourService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Days);
    }

    [Fact]
    public async Task CreateAsync_WithNegativePrice_ShouldCreateTour()
    {
        // Arrange
        var createDto = new TourCreateDto { TourName = "Free Tour", Price = -100m };
        var tour = new Tour { Id = 1, TourName = "Free Tour", Price = -100m };
        var tourDto = new TourDto { Id = 1, TourName = "Free Tour", Price = -100m };

        _mockMapper.Setup(m => m.Map<Tour>(createDto)).Returns(tour);
        _mockTourRepository.Setup(r => r.AddAsync(It.IsAny<Tour>(), It.IsAny<CancellationToken>())).ReturnsAsync(tour);
        _mockMapper.Setup(m => m.Map<TourDto>(tour)).Returns(tourDto);

        // Act
        var result = await _tourService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
    }
}
