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

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<ILogger<BookingService>> _mockLogger;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockLogger = new Mock<ILogger<BookingService>>();
        _bookingService = new BookingService(_mockBookingRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBookings()
    {
        var bookings = new List<Booking>
        {
            new Booking { Id = 1, TourName = "Tour 1" },
            new Booking { Id = 2, TourName = "Tour 2" }
        };
        _mockBookingRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        var result = await _bookingService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<Booking>)result).Count);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsException_PropagatesException()
    {
        _mockBookingRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _bookingService.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsBooking()
    {
        var booking = new Booking { Id = 1, TourName = "Test Tour", Email = "test@example.com" };
        _mockBookingRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(booking);

        var result = await _bookingService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Tour", result.TourName);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        _mockBookingRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Booking?)null);

        var result = await _bookingService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserEmailAsync_ValidEmail_ReturnsBookings()
    {
        var bookings = new List<Booking>
        {
            new Booking { Id = 1, Email = "test@example.com", TourName = "Tour 1" },
            new Booking { Id = 2, Email = "test@example.com", TourName = "Tour 2" }
        };
        _mockBookingRepository.Setup(r => r.GetByUserEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        var result = await _bookingService.GetByUserEmailAsync("test@example.com");

        Assert.NotNull(result);
        Assert.Equal(2, ((List<Booking>)result).Count);
    }

    [Fact]
    public async Task GetByUserEmailAsync_NoBookings_ReturnsEmptyList()
    {
        _mockBookingRepository.Setup(r => r.GetByUserEmailAsync("noemail@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Booking>());

        var result = await _bookingService.GetByUserEmailAsync("noemail@example.com");

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateAsync_ValidBooking_CreatesBookingSuccessfully()
    {
        var booking = new Booking { TourName = "Beach Tour", Email = "user@example.com", FirstName = "John" };
        _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(booking);

        var result = await _bookingService.CreateAsync(booking);

        Assert.NotNull(result);
        Assert.Equal("Beach Tour", result.TourName);
        Assert.True(result.IsActive);
        Assert.Equal("user@example.com", result.CreatedBy);
        Assert.NotEqual(default(DateTime), result.BookingDate);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
    }

    [Fact]
    public async Task CreateAsync_SetsDefaultValues()
    {
        var booking = new Booking { TourName = "Test Tour", Email = "test@example.com" };
        _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(booking);

        var result = await _bookingService.CreateAsync(booking);

        Assert.True(result.IsActive);
        Assert.Equal("test@example.com", result.CreatedBy);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_PropagatesException()
    {
        var booking = new Booking { TourName = "Test Tour" };
        _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _bookingService.CreateAsync(booking));
    }

    [Fact]
    public async Task UpdateAsync_ValidBooking_UpdatesBookingSuccessfully()
    {
        var existingBooking = new Booking { Id = 1, TourName = "Old Tour", Email = "old@example.com" };
        var updatedBookingData = new Booking { TourName = "Updated Tour", Place = "Paris", Email = "new@example.com", FirstName = "Jane" };
        _mockBookingRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingBooking);
        _mockBookingRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingBooking);

        var result = await _bookingService.UpdateAsync(1, updatedBookingData);

        Assert.NotNull(result);
        Assert.Equal("Updated Tour", result.TourName);
        Assert.Equal("Paris", result.Place);
        Assert.Equal("new@example.com", result.Email);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("new@example.com", result.ModifiedBy);
    }

    [Fact]
    public async Task UpdateAsync_BookingNotFound_ThrowsKeyNotFoundException()
    {
        var updatedBookingData = new Booking { TourName = "Updated Tour" };
        _mockBookingRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Booking?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookingService.UpdateAsync(999, updatedBookingData));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesModifiedDate()
    {
        var existingBooking = new Booking { Id = 1, TourName = "Test Tour" };
        var updatedBookingData = new Booking { TourName = "Updated Tour", Email = "test@example.com" };
        _mockBookingRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingBooking);
        _mockBookingRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingBooking);

        var result = await _bookingService.UpdateAsync(1, updatedBookingData);

        Assert.NotNull(result.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_DeletesBookingSuccessfully()
    {
        _mockBookingRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockBookingRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _bookingService.DeleteAsync(1);

        _mockBookingRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_BookingNotFound_ThrowsKeyNotFoundException()
    {
        _mockBookingRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookingService.DeleteAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_PropagatesException()
    {
        _mockBookingRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _bookingService.DeleteAsync(1));
    }

    [Fact]
    public async Task SearchAsync_ValidSearchTerm_ReturnsMatchingBookings()
    {
        var bookings = new List<Booking>
        {
            new Booking { Id = 1, TourName = "Paris Tour", Email = "user1@test.com" },
            new Booking { Id = 2, TourName = "Paris Adventure", Email = "user2@test.com" }
        };
        _mockBookingRepository.Setup(r => r.SearchAsync("Paris", It.IsAny<CancellationToken>())).ReturnsAsync(bookings);

        var result = await _bookingService.SearchAsync("Paris");

        Assert.NotNull(result);
        Assert.Equal(2, ((List<Booking>)result).Count);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmptyList()
    {
        _mockBookingRepository.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>())).ReturnsAsync(new List<Booking>());

        var result = await _bookingService.SearchAsync("NonExistent");

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ThrowsException_PropagatesException()
    {
        _mockBookingRepository.Setup(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _bookingService.SearchAsync("Test"));
    }

    [Fact]
    public void BookingService_Constructor_InitializesCorrectly()
    {
        Assert.NotNull(_bookingService);
    }
}
