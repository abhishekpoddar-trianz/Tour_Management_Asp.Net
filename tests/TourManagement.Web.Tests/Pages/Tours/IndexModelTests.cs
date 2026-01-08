using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TourManagement.Web.Pages.Tours;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Tests.Pages.Tours;

public class IndexModelTests
{
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _indexModel;

    public IndexModelTests()
    {
        _mockTourService = new Mock<ITourService>();
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _indexModel = new IndexModel(_mockTourService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task OnGetAsync_ShouldLoadTours()
    {
        // Arrange
        var tours = new List<TourDto>
        {
            new TourDto { Id = 1, TourName = "Tour1" },
            new TourDto { Id = 2, TourName = "Tour2" }
        };
        _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tours);

        // Act
        await _indexModel.OnGetAsync();

        // Assert
        Assert.NotNull(_indexModel.Tours);
        Assert.Equal(2, _indexModel.Tours.Count());
        _mockTourService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnGetAsync_WhenServiceThrowsException_ShouldSetEmptyTours()
    {
        // Arrange
        _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        await _indexModel.OnGetAsync();

        // Assert
        Assert.NotNull(_indexModel.Tours);
        Assert.Empty(_indexModel.Tours);
    }

    [Fact]
    public async Task OnGetAsync_WithNoTours_ShouldReturnEmptyList()
    {
        // Arrange
        var tours = new List<TourDto>();
        _mockTourService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tours);

        // Act
        await _indexModel.OnGetAsync();

        // Assert
        Assert.NotNull(_indexModel.Tours);
        Assert.Empty(_indexModel.Tours);
    }

    [Fact]
    public void IndexModel_Constructor_ShouldInitializeProperties()
    {
        // Assert
        Assert.NotNull(_indexModel.Tours);
        Assert.Empty(_indexModel.Tours);
    }
}
