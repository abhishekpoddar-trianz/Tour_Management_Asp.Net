using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.Application.Tests.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<BookingService>>();
        _bookingService = new BookingService(_mockBookingRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBookings()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { Id = 1, TourName = "Tour1" },
            new Booking { Id = 2, TourName = "Tour2" }
        };
        var bookingDtos = new List<BookingDto>
        {
            new BookingDto { Id = 1, TourName = "Tour1" },
            new BookingDto { Id = 2, TourName = "Tour2" }
        };

        _mockBookingRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bookings);
        _mockMapper.Setup(m => m.Map<IEnumerable<BookingDto>>(bookings))
            .Returns(bookingDtos);

        // Act
        var result = await _bookingService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockBookingRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnBooking()
    {
        // Arrange
        var bookingId = 1;
        var booking = new Booking { Id = bookingId, TourName = "Test Tour" };
        var bookingDto = new BookingDto { Id = bookingId, TourName = "Test Tour" };

        _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);
        _mockMapper.Setup(m => m.Map<BookingDto>(booking))
            .Returns(bookingDto);

        // Act
        var result = await _bookingService.GetByIdAsync(bookingId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookingId, result.Id);
        _mockBookingRepository.Verify(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var bookingId = 999;
        _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        // Act
        var result = await _bookingService.GetByIdAsync(bookingId);

        // Assert
        Assert.Null(result);
        _mockBookingRepository.Verify(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByUserEmailAsync_ShouldReturnUserBookings()
    {
        // Arrange
        var email = "user@example.com";
        var bookings = new List<Booking>
        {
            new Booking { Id = 1, Email = email, TourName = "Tour1" },
            new Booking { Id = 2, Email = email, TourName = "Tour2" }
        };
        var bookingDtos = new List<BookingDto>
        {
            new BookingDto { Id = 1, Email = email, TourName = "Tour1" },
            new BookingDto { Id = 2, Email = email, TourName = "Tour2" }
        };

        _mockBookingRepository.Setup(r => r.GetByUserEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bookings);
        _mockMapper.Setup(m => m.Map<IEnumerable<BookingDto>>(bookings))
            .Returns(bookingDtos);

        // Act
        var result = await _bookingService.GetByUserEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockBookingRepository.Verify(r => r.GetByUserEmailAsync(email, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidBooking_ShouldCreateBooking()
    {
        // Arrange
        var createDto = new BookingCreateDto
        {
            TourId = 1,
            TourName = "New Tour",
            Email = "user@example.com",
            FirstName = "John"
        };
        var booking = new Booking { Id = 1, TourId = createDto.TourId, TourName = createDto.TourName };
        var bookingDto = new BookingDto { Id = 1, TourId = createDto.TourId, TourName = createDto.TourName };

        _mockMapper.Setup(m => m.Map<Booking>(createDto))
            .Returns(booking);
        _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);
        _mockMapper.Setup(m => m.Map<BookingDto>(booking))
            .Returns(bookingDto);

        // Act
        var result = await _bookingService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.TourName, result.TourName);
        _mockBookingRepository.Verify(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingBooking_ShouldUpdateBooking()
    {
        // Arrange
        var bookingId = 1;
        var updateDto = new BookingUpdateDto { TourName = "Updated Tour" };
        var existingBooking = new Booking { Id = bookingId, TourName = "Old Tour" };

        _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBooking);
        _mockMapper.Setup(m => m.Map(updateDto, existingBooking))
            .Returns(existingBooking);

        // Act
        await _bookingService.UpdateAsync(bookingId, updateDto);

        // Assert
        _mockBookingRepository.Verify(r => r.UpdateAsync(existingBooking, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingBooking_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var bookingId = 999;
        var updateDto = new BookingUpdateDto { TourName = "Updated Tour" };

        _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookingService.UpdateAsync(bookingId, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithExistingBooking_ShouldDeleteBooking()
    {
        // Arrange
        var bookingId = 1;
        _mockBookingRepository.Setup(r => r.ExistsAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _bookingService.DeleteAsync(bookingId);

        // Assert
        _mockBookingRepository.Verify(r => r.DeleteAsync(bookingId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingBooking_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var bookingId = 999;
        _mockBookingRepository.Setup(r => r.ExistsAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookingService.DeleteAsync(bookingId));
    }

    [Fact]
    public async Task CreateAsync_WithZeroTourId_ShouldCreateBooking()
    {
        // Arrange
        var createDto = new BookingCreateDto { TourId = 0 };
        var booking = new Booking { Id = 1, TourId = 0 };
        var bookingDto = new BookingDto { Id = 1, TourId = 0 };

        _mockMapper.Setup(m => m.Map<Booking>(createDto)).Returns(booking);
        _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(booking);
        _mockMapper.Setup(m => m.Map<BookingDto>(booking)).Returns(bookingDto);

        // Act
        var result = await _bookingService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByUserEmailAsync_WithNoBookings_ShouldReturnEmptyList()
    {
        // Arrange
        var email = "user@example.com";
        var bookings = new List<Booking>();
        var bookingDtos = new List<BookingDto>();

        _mockBookingRepository.Setup(r => r.GetByUserEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bookings);
        _mockMapper.Setup(m => m.Map<IEnumerable<BookingDto>>(bookings))
            .Returns(bookingDtos);

        // Act
        var result = await _bookingService.GetByUserEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
