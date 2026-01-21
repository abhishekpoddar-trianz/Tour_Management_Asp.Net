using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TourManagement.Application.Services.Tests;

public class AdminServiceTests
{
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    private readonly Mock<ILogger<AdminService>> _mockLogger;
    private readonly AdminService _adminService;

    public AdminServiceTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _mockLogger = new Mock<ILogger<AdminService>>();
        _adminService = new AdminService(_mockAdminRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ValidCredentials_ReturnsAdmin()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        var admin = new Admin { Id = 1, Username = "admin", PasswordHash = hashedPassword, Email = "admin@example.com" };
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(admin);

        var result = await _adminService.AuthenticateAsync("admin", "admin123");

        Assert.NotNull(result);
        Assert.Equal("admin", result.Username);
        Assert.Equal("admin@example.com", result.Email);
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidUsername_ReturnsNull()
    {
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>())).ReturnsAsync((Admin?)null);

        var result = await _adminService.AuthenticateAsync("nonexistent", "password");

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidPassword_ReturnsNull()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        var admin = new Admin { Id = 1, Username = "admin", PasswordHash = hashedPassword };
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(admin);

        var result = await _adminService.AuthenticateAsync("admin", "wrongpassword");

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_ThrowsException_PropagatesException()
    {
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _adminService.AuthenticateAsync("admin", "password"));
    }

    [Fact]
    public async Task GetByUsernameAsync_ValidUsername_ReturnsAdmin()
    {
        var admin = new Admin { Id = 1, Username = "admin", Email = "admin@example.com" };
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(admin);

        var result = await _adminService.GetByUsernameAsync("admin");

        Assert.NotNull(result);
        Assert.Equal("admin", result.Username);
        Assert.Equal("admin@example.com", result.Email);
    }

    [Fact]
    public async Task GetByUsernameAsync_InvalidUsername_ReturnsNull()
    {
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("nonexistent", It.IsAny<CancellationToken>())).ReturnsAsync((Admin?)null);

        var result = await _adminService.GetByUsernameAsync("nonexistent");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_ThrowsException_PropagatesException()
    {
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _adminService.GetByUsernameAsync("admin"));
    }

    [Fact]
    public void AdminService_Constructor_InitializesCorrectly()
    {
        Assert.NotNull(_adminService);
    }

    [Fact]
    public async Task AuthenticateAsync_EmptyUsername_ReturnsNull()
    {
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync(string.Empty, It.IsAny<CancellationToken>())).ReturnsAsync((Admin?)null);

        var result = await _adminService.AuthenticateAsync(string.Empty, "password");

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_EmptyPassword_ReturnsNull()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        var admin = new Admin { Id = 1, Username = "admin", PasswordHash = hashedPassword };
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(admin);

        var result = await _adminService.AuthenticateAsync("admin", string.Empty);

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_CaseSensitiveUsername_BehavesCorrectly()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
        var admin = new Admin { Id = 1, Username = "admin", PasswordHash = hashedPassword };
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(admin);
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("Admin", It.IsAny<CancellationToken>())).ReturnsAsync((Admin?)null);

        var result = await _adminService.AuthenticateAsync("Admin", "admin123");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_EmptyUsername_ReturnsNull()
    {
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync(string.Empty, It.IsAny<CancellationToken>())).ReturnsAsync((Admin?)null);

        var result = await _adminService.GetByUsernameAsync(string.Empty);

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_SpecialCharactersInUsername_HandlesCorrectly()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");
        var admin = new Admin { Id = 1, Username = "admin_123!@", PasswordHash = hashedPassword };
        _mockAdminRepository.Setup(r => r.GetByUsernameAsync("admin_123!@", It.IsAny<CancellationToken>())).ReturnsAsync(admin);

        var result = await _adminService.AuthenticateAsync("admin_123!@", "password");

        Assert.NotNull(result);
        Assert.Equal("admin_123!@", result.Username);
    }
}
