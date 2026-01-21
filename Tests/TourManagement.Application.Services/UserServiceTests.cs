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

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, Email = "user1@test.com" },
            new User { Id = 2, Email = "user2@test.com" }
        };
        _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

        var result = await _userService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<User>)result).Count);
    }

    [Fact]
    public async Task GetAllAsync_ThrowsException_PropagatesException()
    {
        _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(() => _userService.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsUser()
    {
        var user = new User { Id = 1, Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _userService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        _mockUserRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var result = await _userService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ValidEmail_ReturnsUser()
    {
        var user = new User { Id = 1, Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _userService.GetByEmailAsync("test@example.com");

        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_InvalidEmail_ReturnsNull()
    {
        _mockUserRepository.Setup(r => r.GetByEmailAsync("nonexistent@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var result = await _userService.GetByEmailAsync("nonexistent@example.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidUser_CreatesUserSuccessfully()
    {
        var user = new User { Email = "new@example.com", FirstName = "John" };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("new@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _userService.CreateAsync(user, "password123");

        Assert.NotNull(result);
        Assert.Equal("new@example.com", result.Email);
        Assert.True(result.IsActive);
        Assert.Equal("System", result.CreatedBy);
        Assert.NotNull(result.PasswordHash);
    }

    [Fact]
    public async Task CreateAsync_ExistingEmail_ThrowsInvalidOperationException()
    {
        var user = new User { Email = "existing@example.com" };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("existing@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateAsync(user, "password123"));
    }

    [Fact]
    public async Task CreateAsync_HashesPassword()
    {
        var user = new User { Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _userService.CreateAsync(user, "password123");

        Assert.NotNull(result.PasswordHash);
        Assert.NotEqual("password123", result.PasswordHash);
    }

    [Fact]
    public async Task UpdateAsync_ValidUser_UpdatesUserSuccessfully()
    {
        var existingUser = new User { Id = 1, Email = "test@example.com", FirstName = "John" };
        var updatedUserData = new User { FirstName = "Jane", LastName = "Doe", PhoneNumber = "123-456-7890" };
        _mockUserRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);
        _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

        var result = await _userService.UpdateAsync(1, updatedUserData);

        Assert.NotNull(result);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("123-456-7890", result.PhoneNumber);
        Assert.Equal("System", result.ModifiedBy);
    }

    [Fact]
    public async Task UpdateAsync_UserNotFound_ThrowsKeyNotFoundException()
    {
        var updatedUserData = new User { FirstName = "Jane" };
        _mockUserRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateAsync(999, updatedUserData));
    }

    [Fact]
    public async Task DeleteAsync_ValidId_DeletesUserSuccessfully()
    {
        _mockUserRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _userService.DeleteAsync(1);

        _mockUserRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_UserNotFound_ThrowsKeyNotFoundException()
    {
        _mockUserRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteAsync(999));
    }

    [Fact]
    public async Task AuthenticateAsync_ValidCredentials_ReturnsUser()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, Email = "test@example.com", PasswordHash = hashedPassword };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _userService.AuthenticateAsync("test@example.com", "password123");

        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidEmail_ReturnsNull()
    {
        _mockUserRepository.Setup(r => r.GetByEmailAsync("nonexistent@example.com", It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        var result = await _userService.AuthenticateAsync("nonexistent@example.com", "password123");

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_InvalidPassword_ReturnsNull()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var user = new User { Id = 1, Email = "test@example.com", PasswordHash = hashedPassword };
        _mockUserRepository.Setup(r => r.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _userService.AuthenticateAsync("test@example.com", "wrongpassword");

        Assert.Null(result);
    }

    [Fact]
    public async Task SearchAsync_ValidSearchTerm_ReturnsMatchingUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, Email = "john@test.com", FirstName = "John" },
            new User { Id = 2, Email = "johnny@test.com", FirstName = "Johnny" }
        };
        _mockUserRepository.Setup(r => r.SearchAsync("john", It.IsAny<CancellationToken>())).ReturnsAsync(users);

        var result = await _userService.SearchAsync("john");

        Assert.NotNull(result);
        Assert.Equal(2, ((List<User>)result).Count);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmptyList()
    {
        _mockUserRepository.Setup(r => r.SearchAsync("NonExistent", It.IsAny<CancellationToken>())).ReturnsAsync(new List<User>());

        var result = await _userService.SearchAsync("NonExistent");

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void UserService_Constructor_InitializesCorrectly()
    {
        Assert.NotNull(_userService);
    }
}
