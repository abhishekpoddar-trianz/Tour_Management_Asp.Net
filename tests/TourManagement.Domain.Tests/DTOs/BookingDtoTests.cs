using Xunit;
using TourManagement.Domain.DTOs;

namespace TourManagement.Domain.Tests.DTOs;

public class BookingDtoTests
{
    [Fact]
    public void BookingDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new BookingDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(0, dto.Id);
        Assert.Equal(0, dto.TourId);
        Assert.Equal(string.Empty, dto.TourName);
        Assert.Equal(string.Empty, dto.Place);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Null(dto.ModifiedDate);
    }

    [Fact]
    public void BookingDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new BookingDto();
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        dto.Id = 1;
        dto.TourId = 100;
        dto.TourName = "European Tour";
        dto.Place = "Paris";
        dto.Email = "test@example.com";
        dto.FirstName = "John";
        dto.CreatedDate = createdDate;
        dto.ModifiedDate = modifiedDate;
        dto.IsActive = true;

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal(100, dto.TourId);
        Assert.Equal("European Tour", dto.TourName);
        Assert.Equal("Paris", dto.Place);
        Assert.Equal("test@example.com", dto.Email);
        Assert.Equal("John", dto.FirstName);
        Assert.Equal(createdDate, dto.CreatedDate);
        Assert.Equal(modifiedDate, dto.ModifiedDate);
        Assert.True(dto.IsActive);
    }
}

public class BookingCreateDtoTests
{
    [Fact]
    public void BookingCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new BookingCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(0, dto.TourId);
        Assert.Equal(string.Empty, dto.TourName);
        Assert.Equal(string.Empty, dto.Place);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
    }

    [Fact]
    public void BookingCreateDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new BookingCreateDto();

        // Act
        dto.TourId = 100;
        dto.TourName = "European Tour";
        dto.Place = "Paris";
        dto.Email = "test@example.com";
        dto.FirstName = "John";

        // Assert
        Assert.Equal(100, dto.TourId);
        Assert.Equal("European Tour", dto.TourName);
        Assert.Equal("Paris", dto.Place);
        Assert.Equal("test@example.com", dto.Email);
        Assert.Equal("John", dto.FirstName);
    }

    [Fact]
    public void BookingCreateDto_WithEmptyEmail_ShouldAllowEmpty()
    {
        // Arrange & Act
        var dto = new BookingCreateDto { Email = "" };

        // Assert
        Assert.Equal("", dto.Email);
    }

    [Fact]
    public void BookingCreateDto_WithZeroTourId_ShouldAllowZero()
    {
        // Arrange & Act
        var dto = new BookingCreateDto { TourId = 0 };

        // Assert
        Assert.Equal(0, dto.TourId);
    }
}

public class BookingUpdateDtoTests
{
    [Fact]
    public void BookingUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new BookingUpdateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(0, dto.TourId);
        Assert.Equal(string.Empty, dto.TourName);
        Assert.Equal(string.Empty, dto.Place);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
    }

    [Fact]
    public void BookingUpdateDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new BookingUpdateDto();

        // Act
        dto.TourId = 200;
        dto.TourName = "Asia Tour";
        dto.Place = "Tokyo";
        dto.Email = "update@example.com";
        dto.FirstName = "Jane";

        // Assert
        Assert.Equal(200, dto.TourId);
        Assert.Equal("Asia Tour", dto.TourName);
        Assert.Equal("Tokyo", dto.Place);
        Assert.Equal("update@example.com", dto.Email);
        Assert.Equal("Jane", dto.FirstName);
    }

    [Fact]
    public void BookingUpdateDto_WithNegativeTourId_ShouldAllowNegative()
    {
        // Arrange & Act
        var dto = new BookingUpdateDto { TourId = -1 };

        // Assert
        Assert.Equal(-1, dto.TourId);
    }
}
