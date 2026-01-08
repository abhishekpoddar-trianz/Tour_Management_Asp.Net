using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using TourManagement.Web.Pages.Tours;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Tests.Pages.Tours;

public class EditModelTests
{
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<ILogger<EditModel>> _mockLogger;
    private readonly EditModel _editModel;

    public EditModelTests()
    {
        _mockTourService = new Mock<ITourService>();
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockLogger = new Mock<ILogger<EditModel>>();
        _editModel = new EditModel(_mockTourService.Object, _mockEnvironment.Object, _mockLogger.Object);
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
            Place = "Paris",
            Days = 7,
            Price = 1500.00m,
            Locations = "France",
            TourInfo = "Test info",
            PicturePath = "/images/test.jpg"
        };
        _mockTourService.Setup(s => s.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tour);

        // Act
        var result = await _editModel.OnGetAsync(tourId);

        // Assert
        Assert.IsType<PageResult>(result);
        Assert.NotNull(_editModel.Input);
        Assert.Equal(tourId, _editModel.Input.Id);
        Assert.Equal("Test Tour", _editModel.Input.TourName);
        Assert.Equal("Paris", _editModel.Input.Place);
        Assert.Equal(7, _editModel.Input.Days);
        Assert.Equal(1500.00m, _editModel.Input.Price);
    }

    [Fact]
    public async Task OnGetAsync_WithNonExistingTour_ShouldReturnNotFound()
    {
        // Arrange
        var tourId = 999;
        _mockTourService.Setup(s => s.GetByIdAsync(tourId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TourDto?)null);

        // Act
        var result = await _editModel.OnGetAsync(tourId);

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
        var result = await _editModel.OnGetAsync(tourId);

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        var redirectResult = result as RedirectToPageResult;
        Assert.Equal("./Index", redirectResult!.PageName);
    }

    [Fact]
    public void InputModel_Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var input = new EditModel.InputModel();

        // Assert
        Assert.Equal(0, input.Id);
        Assert.Equal(string.Empty, input.TourName);
        Assert.Equal(string.Empty, input.Place);
        Assert.Equal(0, input.Days);
        Assert.Equal(0m, input.Price);
        Assert.Equal(string.Empty, input.Locations);
        Assert.Equal(string.Empty, input.TourInfo);
        Assert.Null(input.CurrentPicturePath);
        Assert.Null(input.PictureFile);
    }

    [Fact]
    public void InputModel_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var input = new EditModel.InputModel();

        // Act
        input.Id = 1;
        input.TourName = "Updated Tour";
        input.Place = "Updated Place";
        input.Days = 14;
        input.Price = 3000.00m;
        input.Locations = "New Locations";
        input.TourInfo = "Updated info";
        input.CurrentPicturePath = "/images/old.jpg";

        // Assert
        Assert.Equal(1, input.Id);
        Assert.Equal("Updated Tour", input.TourName);
        Assert.Equal("Updated Place", input.Place);
        Assert.Equal(14, input.Days);
        Assert.Equal(3000.00m, input.Price);
        Assert.Equal("New Locations", input.Locations);
        Assert.Equal("Updated info", input.TourInfo);
        Assert.Equal("/images/old.jpg", input.CurrentPicturePath);
    }

    [Fact]
    public void EditModel_Constructor_ShouldInitializeInput()
    {
        // Assert
        Assert.NotNull(_editModel.Input);
    }
}
