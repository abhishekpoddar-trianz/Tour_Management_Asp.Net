using Xunit;
using TourManagement.Domain.DTOs;

namespace TourManagement.Domain.Tests.DTOs;

public class TourDtoTests
{
    [Fact]
    public void TourDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new TourDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.TourName);
        Assert.Equal(string.Empty, dto.Place);
        Assert.Equal(0, dto.Days);
        Assert.Equal(0m, dto.Price);
        Assert.Equal(string.Empty, dto.Locations);
        Assert.Equal(string.Empty, dto.TourInfo);
        Assert.Null(dto.PicturePath);
        Assert.Null(dto.ModifiedDate);
    }

    [Fact]
    public void TourDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new TourDto();
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        dto.Id = 1;
        dto.TourName = "European Adventure";
        dto.Place = "Europe";
        dto.Days = 10;
        dto.Price = 2500.50m;
        dto.Locations = "Paris, London, Rome";
        dto.TourInfo = "Amazing tour";
        dto.PicturePath = "/images/tour.jpg";
        dto.CreatedDate = createdDate;
        dto.ModifiedDate = modifiedDate;
        dto.IsActive = true;

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("European Adventure", dto.TourName);
        Assert.Equal("Europe", dto.Place);
        Assert.Equal(10, dto.Days);
        Assert.Equal(2500.50m, dto.Price);
        Assert.Equal("Paris, London, Rome", dto.Locations);
        Assert.Equal("Amazing tour", dto.TourInfo);
        Assert.Equal("/images/tour.jpg", dto.PicturePath);
        Assert.Equal(createdDate, dto.CreatedDate);
        Assert.Equal(modifiedDate, dto.ModifiedDate);
        Assert.True(dto.IsActive);
    }
}

public class TourCreateDtoTests
{
    [Fact]
    public void TourCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new TourCreateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.TourName);
        Assert.Equal(string.Empty, dto.Place);
        Assert.Equal(0, dto.Days);
        Assert.Equal(0m, dto.Price);
        Assert.Equal(string.Empty, dto.Locations);
        Assert.Equal(string.Empty, dto.TourInfo);
        Assert.Null(dto.PicturePath);
    }

    [Fact]
    public void TourCreateDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new TourCreateDto();

        // Act
        dto.TourName = "Asia Tour";
        dto.Place = "Asia";
        dto.Days = 14;
        dto.Price = 3500.00m;
        dto.Locations = "Tokyo, Bangkok, Singapore";
        dto.TourInfo = "Comprehensive Asia tour";
        dto.PicturePath = "/images/asia.jpg";

        // Assert
        Assert.Equal("Asia Tour", dto.TourName);
        Assert.Equal("Asia", dto.Place);
        Assert.Equal(14, dto.Days);
        Assert.Equal(3500.00m, dto.Price);
        Assert.Equal("Tokyo, Bangkok, Singapore", dto.Locations);
        Assert.Equal("Comprehensive Asia tour", dto.TourInfo);
        Assert.Equal("/images/asia.jpg", dto.PicturePath);
    }

    [Fact]
    public void TourCreateDto_WithNegativeDays_ShouldAllowNegative()
    {
        // Arrange & Act
        var dto = new TourCreateDto { Days = -1 };

        // Assert
        Assert.Equal(-1, dto.Days);
    }

    [Fact]
    public void TourCreateDto_WithZeroPrice_ShouldAllowZero()
    {
        // Arrange & Act
        var dto = new TourCreateDto { Price = 0m };

        // Assert
        Assert.Equal(0m, dto.Price);
    }

    [Fact]
    public void TourCreateDto_WithNullPicturePath_ShouldAllowNull()
    {
        // Arrange & Act
        var dto = new TourCreateDto { PicturePath = null };

        // Assert
        Assert.Null(dto.PicturePath);
    }
}

public class TourUpdateDtoTests
{
    [Fact]
    public void TourUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new TourUpdateDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.TourName);
        Assert.Equal(string.Empty, dto.Place);
        Assert.Equal(0, dto.Days);
        Assert.Equal(0m, dto.Price);
        Assert.Equal(string.Empty, dto.Locations);
        Assert.Equal(string.Empty, dto.TourInfo);
        Assert.Null(dto.PicturePath);
    }

    [Fact]
    public void TourUpdateDto_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var dto = new TourUpdateDto();

        // Act
        dto.TourName = "Updated Tour";
        dto.Place = "Updated Place";
        dto.Days = 7;
        dto.Price = 1500.00m;
        dto.Locations = "City1, City2";
        dto.TourInfo = "Updated info";
        dto.PicturePath = "/images/updated.jpg";

        // Assert
        Assert.Equal("Updated Tour", dto.TourName);
        Assert.Equal("Updated Place", dto.Place);
        Assert.Equal(7, dto.Days);
        Assert.Equal(1500.00m, dto.Price);
        Assert.Equal("City1, City2", dto.Locations);
        Assert.Equal("Updated info", dto.TourInfo);
        Assert.Equal("/images/updated.jpg", dto.PicturePath);
    }

    [Fact]
    public void TourUpdateDto_WithLargePrice_ShouldAllowLargeValues()
    {
        // Arrange & Act
        var dto = new TourUpdateDto { Price = 999999.99m };

        // Assert
        Assert.Equal(999999.99m, dto.Price);
    }

    [Fact]
    public void TourUpdateDto_WithEmptyStrings_ShouldAllowEmpty()
    {
        // Arrange & Act
        var dto = new TourUpdateDto
        {
            TourName = "",
            Place = "",
            Locations = "",
            TourInfo = ""
        };

        // Assert
        Assert.Equal("", dto.TourName);
        Assert.Equal("", dto.Place);
        Assert.Equal("", dto.Locations);
        Assert.Equal("", dto.TourInfo);
    }
}
