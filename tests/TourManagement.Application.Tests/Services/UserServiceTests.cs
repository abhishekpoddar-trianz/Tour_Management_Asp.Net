using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Services;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.Application.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Email = "user1@example.com", FirstName = "User1" },
            new User { Email = "user2@example.com", FirstName = "User2" }
        };
        var userDtos = new List<UserDto>
        {
            new UserDto { Email = "user1@example.com", FirstName = "User1" },
            new UserDto { Email = "user2@example.com", FirstName = "User2" }
        };

        _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users))
            .Returns(userDtos);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockUserRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User { Email = email, FirstName = "Test" };
        var userDto = new UserDto { Email = email, FirstName = "Test" };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user))
            .Returns(userDto);

        // Act
        var result = await _userService.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
        _mockUserRepository.Verify(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistingEmail_ShouldReturnNull()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _mockUserRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetByEmailAsync(email);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidUser_ShouldCreateUser()
    {
        // Arrange
        var createDto = new UserCreateDto
        {
            Email = "new@example.com",
            FirstName = "New",
            LastName = "User",
            Password = "Password123"
        };
        var user = new User { Email = createDto.Email, FirstName = createDto.FirstName };
        var userDto = new UserDto { Email = createDto.Email, FirstName = createDto.FirstName };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(createDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _mockMapper.Setup(m => m.Map<User>(createDto))
            .Returns(user);
        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user))
            .Returns(userDto);

        // Act
        var result = await _userService.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Email, result.Email);
        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createDto = new UserCreateDto { Email = "existing@example.com" };
        var existingUser = new User { Email = createDto.Email };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(createDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_WithExistingUser_ShouldUpdateUser()
    {
        // Arrange
        var email = "test@example.com";
        var updateDto = new UserUpdateDto { FirstName = "Updated", LastName = "Name" };
        var existingUser = new User { Email = email, FirstName = "Old" };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        _mockMapper.Setup(m => m.Map(updateDto, existingUser))
            .Returns(existingUser);

        // Act
        await _userService.UpdateAsync(email, updateDto);

        // Assert
        _mockUserRepository.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingUser_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var updateDto = new UserUpdateDto { FirstName = "Updated" };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateAsync(email, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_WithExistingUser_ShouldDeleteUser()
    {
        // Arrange
        var email = "test@example.com";
        _mockUserRepository.Setup(r => r.ExistsAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _userService.DeleteAsync(email);

        // Assert
        _mockUserRepository.Verify(r => r.DeleteAsync(email, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingUser_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _mockUserRepository.Setup(r => r.ExistsAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteAsync(email));
    }

    [Fact]
    public async Task ValidateUserAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        var loginDto = new UserLoginDto { Email = "test@example.com", Password = "Password123" };
        var user = new User { Email = loginDto.Email, Password = loginDto.Password };
        var userDto = new UserDto { Email = loginDto.Email };

        _mockUserRepository.Setup(r => r.ValidateUserAsync(loginDto.Email, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user))
            .Returns(userDto);

        // Act
        var result = await _userService.ValidateUserAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(loginDto.Email, result.Email);
    }

    [Fact]
    public async Task ValidateUserAsync_WithInvalidCredentials_ShouldReturnNull()
    {
        // Arrange
        var loginDto = new UserLoginDto { Email = "test@example.com", Password = "WrongPassword" };
        _mockUserRepository.Setup(r => r.ValidateUserAsync(loginDto.Email, loginDto.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.ValidateUserAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingUsers()
    {
        // Arrange
        var searchTerm = "john";
        var users = new List<User> { new User { Email = "john@example.com", FirstName = "John" } };
        var userDtos = new List<UserDto> { new UserDto { Email = "john@example.com", FirstName = "John" } };

        _mockUserRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users))
            .Returns(userDtos);

        // Act
        var result = await _userService.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ShouldCallRepository()
    {
        // Arrange
        var searchTerm = "";
        var users = new List<User>();
        var userDtos = new List<UserDto>();

        _mockUserRepository.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users))
            .Returns(userDtos);

        // Act
        var result = await _userService.SearchAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockUserRepository.Verify(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()), Times.Once);
    }
}
