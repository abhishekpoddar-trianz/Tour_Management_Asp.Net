using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TourManagement.Infrastructure.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;

namespace TourManagement.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _dbContextOptions;
    private readonly Mock<ILogger<UserRepository>> _mockLogger;

    public UserRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<UserRepository>>();
    }

    private TourManagementDbContext CreateContext()
    {
        return new TourManagementDbContext(_dbContextOptions);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Email = "user1@test.com", FirstName = "User1", IsActive = true },
            new User { Email = "user2@test.com", FirstName = "User2", IsActive = true },
            new User { Email = "user3@test.com", FirstName = "User3", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, u => Assert.True(u.IsActive));
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            IsActive = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("Test", result.FirstName);
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistingEmail_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_WithInactiveUser_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "inactive@example.com",
            FirstName = "Inactive",
            IsActive = false
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByEmailAsync("inactive@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new UserRepository(context, _mockLogger.Object);
        var user = new User
        {
            Email = "newuser@example.com",
            FirstName = "New",
            LastName = "User",
            Password = "Password123",
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newuser@example.com", result.Email);

        var savedUser = await context.Users.FindAsync("newuser@example.com");
        Assert.NotNull(savedUser);
        Assert.Equal("New", savedUser.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "update@example.com",
            FirstName = "Old",
            LastName = "Name",
            IsActive = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var repository = new UserRepository(context, _mockLogger.Object);
        user.FirstName = "Updated";

        // Act
        await repository.UpdateAsync(user);

        // Assert
        var updatedUser = await context.Users.FindAsync("update@example.com");
        Assert.NotNull(updatedUser);
        Assert.Equal("Updated", updatedUser.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveToFalse()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "delete@example.com",
            FirstName = "Delete",
            IsActive = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        await repository.DeleteAsync("delete@example.com");

        // Assert
        var deletedUser = await context.Users.FindAsync("delete@example.com");
        Assert.NotNull(deletedUser);
        Assert.False(deletedUser.IsActive);
        Assert.NotNull(deletedUser.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingEmail_ShouldNotThrowException()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act & Assert
        await repository.DeleteAsync("nonexistent@example.com");
    }

    [Fact]
    public async Task ExistsAsync_WithExistingActiveUser_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "exists@example.com",
            FirstName = "Exists",
            IsActive = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync("exists@example.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingUser_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync("nonexistent@example.com");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveUser_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "inactive@example.com",
            FirstName = "Inactive",
            IsActive = false
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync("inactive@example.com");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateUserAsync_WithValidCredentials_ShouldReturnUser()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "validate@example.com",
            Password = "Password123",
            FirstName = "Validate",
            IsActive = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ValidateUserAsync("validate@example.com", "Password123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("validate@example.com", result.Email);
    }

    [Fact]
    public async Task ValidateUserAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "validate@example.com",
            Password = "Password123",
            FirstName = "Validate",
            IsActive = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ValidateUserAsync("validate@example.com", "WrongPassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ValidateUserAsync_WithInactiveUser_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateContext();
        var user = new User
        {
            Email = "inactive@example.com",
            Password = "Password123",
            FirstName = "Inactive",
            IsActive = false
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ValidateUserAsync("inactive@example.com", "Password123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingUsers()
    {
        // Arrange
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Email = "john@example.com", FirstName = "John", LastName = "Doe", IsActive = true },
            new User { Email = "jane@example.com", FirstName = "Jane", LastName = "Smith", IsActive = true },
            new User { Email = "johnny@example.com", FirstName = "Johnny", LastName = "Test", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("john");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        using var context = CreateContext();
        context.Users.Add(new User { Email = "test@example.com", FirstName = "Test", IsActive = true });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("nonexistent");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldSearchInEmailFirstNameAndLastName()
    {
        // Arrange
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Email = "smith@example.com", FirstName = "John", LastName = "Doe", IsActive = true },
            new User { Email = "jane@example.com", FirstName = "Smith", LastName = "Test", IsActive = true },
            new User { Email = "test@example.com", FirstName = "Test", LastName = "Smith", IsActive = true }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("smith");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task SearchAsync_ShouldOnlyReturnActiveUsers()
    {
        // Arrange
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Email = "john1@example.com", FirstName = "John", IsActive = true },
            new User { Email = "john2@example.com", FirstName = "John", IsActive = false }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.SearchAsync("john");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.True(result.First().IsActive);
    }
}
