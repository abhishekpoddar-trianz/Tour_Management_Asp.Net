using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourManagement.Infrastructure.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TourManagement.Infrastructure.Repositories.Tests;

public class UserRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _options;
    private readonly Mock<ILogger<UserRepository>> _mockLogger;

    public UserRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<UserRepository>>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveUsersOnly()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.AddRange(
            new User { Id = 1, Email = "active@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, FirstName = "Active", PasswordHash = "hash", CreatedBy = "System" },
            new User { Id = 2, Email = "inactive@test.com", IsActive = false, CreatedDate = DateTime.UtcNow, FirstName = "Inactive", PasswordHash = "hash", CreatedBy = "System" }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("active@test.com", result.First().Email);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsUser()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "test@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, FirstName = "Test", PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ValidEmail_ReturnsUser()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "test@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, FirstName = "Test", PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.GetByEmailAsync("test@test.com");

        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_InvalidEmail_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        var result = await repository.GetByEmailAsync("nonexistent@test.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_InactiveUser_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "inactive@test.com", IsActive = false, CreatedDate = DateTime.UtcNow, FirstName = "Test", PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.GetByEmailAsync("inactive@test.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ValidUser_AddsUserSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);
        var user = new User { Email = "new@test.com", FirstName = "New", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" };

        var result = await repository.AddAsync(user);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("new@test.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_ValidUser_UpdatesUserSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "old@test.com", FirstName = "Old", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var user = await context.Users.FindAsync(1);
        user!.FirstName = "Updated";
        var result = await repository.UpdateAsync(user);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_SoftDeletesUser()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "delete@test.com", FirstName = "Delete", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        await repository.DeleteAsync(1);

        var user = await context.Users.FindAsync(1);
        Assert.NotNull(user);
        Assert.False(user.IsActive);
        Assert.NotNull(user.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_InvalidId_DoesNotThrow()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        await repository.DeleteAsync(999);

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ValidId_ReturnsTrue()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "exists@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, FirstName = "Exists", PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_InvalidId_ReturnsFalse()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new UserRepository(context, _mockLogger.Object);

        var result = await repository.ExistsAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_MatchingEmail_ReturnsUsers()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.AddRange(
            new User { Id = 1, Email = "john@test.com", FirstName = "John", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" },
            new User { Id = 2, Email = "jane@test.com", FirstName = "Jane", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" }
        );
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("john");

        Assert.Single(result);
        Assert.Equal("john@test.com", result.First().Email);
    }

    [Fact]
    public async Task SearchAsync_MatchingFirstName_ReturnsUsers()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "test@test.com", FirstName = "Johnny", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Johnny");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_MatchingLastName_ReturnsUsers()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "test@test.com", FirstName = "John", LastName = "Doe", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("Doe");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmpty()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "test@test.com", FirstName = "John", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("NonExistent");

        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_ExcludesInactiveUsers()
    {
        using var context = new TourManagementDbContext(_options);
        context.Users.Add(new User { Id = 1, Email = "test@test.com", FirstName = "John", IsActive = false, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context, _mockLogger.Object);
        var result = await repository.SearchAsync("John");

        Assert.Empty(result);
    }
}
