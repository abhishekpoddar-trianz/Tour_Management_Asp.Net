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

public class CreateModelTests
{
    private readonly Mock<ITourService> _mockTourService;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<ILogger<CreateModel>> _mockLogger;
    private readonly CreateModel _createModel;

    public CreateModelTests()
    {
        _mockTourService = new Mock<ITourService>();
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockLogger = new Mock<ILogger<CreateModel>>();
        _createModel = new CreateModel(_mockTourService.Object, _mockEnvironment.Object, _mockLogger.Object);
    }

    [Fact]
    public void OnGet_ShouldExecuteSuccessfully()
    {
        // Act
        _createModel.OnGet();

        // Assert - No exception thrown
        Assert.NotNull(_createModel.Input);
    }

    [Fact]
    public void CreateModel_Constructor_ShouldInitializeInput()
    {
        // Assert
        Assert.NotNull(_createModel.Input);
    }

    [Fact]
    public void InputModel_Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var input = new CreateModel.InputModel();

        // Assert
        Assert.Equal(string.Empty, input.TourName);
        Assert.Equal(string.Empty, input.Place);
        Assert.Equal(0, input.Days);
        Assert.Equal(0m, input.Price);
        Assert.Equal(string.Empty, input.Locations);
        Assert.Equal(string.Empty, input.TourInfo);
        Assert.Null(input.PictureFile);
    }

    [Fact]
    public void InputModel_SetProperties_ShouldSetAllProperties()
    {
        // Arrange
        var input = new CreateModel.InputModel();

        // Act
        input.TourName = "European Tour";
        input.Place = "Europe";
        input.Days = 10;
        input.Price = 2500.00m;
        input.Locations = "Paris, London, Rome";
        input.TourInfo = "Amazing tour";

        // Assert
        Assert.Equal("European Tour", input.TourName);
        Assert.Equal("Europe", input.Place);
        Assert.Equal(10, input.Days);
        Assert.Equal(2500.00m, input.Price);
        Assert.Equal("Paris, London, Rome", input.Locations);
        Assert.Equal("Amazing tour", input.TourInfo);
    }
}
