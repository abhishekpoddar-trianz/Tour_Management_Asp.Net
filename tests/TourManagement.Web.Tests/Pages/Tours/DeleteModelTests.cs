using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Web.Pages.Tours;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Tests.Pages.Tours;

public class DeleteModelTests
{
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<ILogger<DeleteModel>> _mockLogger;
    private readonly DeleteModel _deleteModel;

    public DeleteModelTests()
    {
        _mockTourService = new Mock<ITourService>();
        _mockLogger = new Mock<ILogger<DeleteModel>>();
        _deleteModel = new DeleteModel(_mockTourService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task OnGetAsync_WithExistingTour_ShouldLoadTour()
    {
        // Arrange
        var tourId = 1;
        var tour = new TourDto
        {
            Id = tourId,
            TourName = "Test Tour",
            Place = "Paris"
        };
        _mockTourService.Setup(s => s.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tour);

        // Act
        var result = await _deleteModel.OnGetAsync(tourId);

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_deleteModel.Tour);
        Assert.Equal(tourId, _deleteModel.Tour.Id);
        Assert.Equal("Test Tour", _deleteModel.Tour.TourName);
    }

    [Fact]
    public async Task OnGetAsync_WithNonExistingTour_ShouldReturnNotFound()
    {
        // Arrange
        var tourId = 999;
        _mockTourService.Setup(s => s.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TourDto?)null);

        // Act
        var result = await _deleteModel.OnGetAsync(tourId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task OnGetAsync_WhenServiceThrowsException_ShouldRedirectToIndex()
    {
        // Arrange
        var tourId = 1;
        _mockTourService.Setup(s => s.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _deleteModel.OnGetAsync(tourId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = result as RedirectToPageResult;
        Assert.Equal("./Index", redirectResult!.PageName);
    }

    [Fact]
    public async Task OnPostAsync_WithExistingTour_ShouldDeleteAndRedirect()
    {
        // Arrange
        var tourId = 1;
        _mockTourService.Setup(s => s.DeleteAsync(tourId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _deleteModel.OnPostAsync(tourId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = result as RedirectToPageResult;
        Assert.Equal("./Index", redirectResult!.PageName);
        _mockTourService.Verify(s => s.DeleteAsync(tourId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task OnPostAsync_WhenServiceThrowsException_ShouldRedirectToIndex()
    {
        // Arrange
        var tourId = 1;
        _mockTourService.Setup(s => s.DeleteAsync(tourId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act
        var result = await _deleteModel.OnPostAsync(tourId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = result as RedirectToPageResult;
        Assert.Equal("./Index", redirectResult!.PageName);
    }

    [Fact]
    public void DeleteModel_Constructor_ShouldInitializeTourAsNull()
    {
        // Assert
        Assert.Null(_deleteModel.Tour);
    }

    [Fact]
    public async Task OnPostAsync_WithZeroId_ShouldCallService()
    {
        // Arrange
        var tourId = 0;
        _mockTourService.Setup(s => s.DeleteAsync(tourId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _deleteModel.OnPostAsync(tourId);

        // Assert
        _mockTourService.Verify(s => s.DeleteAsync(tourId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
