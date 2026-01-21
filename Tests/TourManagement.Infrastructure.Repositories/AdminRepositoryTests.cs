using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourManagement.Infrastructure.Repositories;
using TourManagement.Infrastructure.Data;
using TourManagement.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.Infrastructure.Repositories.Tests;

public class AdminRepositoryTests
{
    private readonly DbContextOptions<TourManagementDbContext> _options;
    private readonly Mock<ILogger<AdminRepository>> _mockLogger;

    public AdminRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<TourManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _mockLogger = new Mock<ILogger<AdminRepository>>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsActiveAdminsOnly()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.AddRange(
            new Admin { Id = 1, Username = "admin1", Email = "admin1@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" },
            new Admin { Id = 2, Username = "admin2", Email = "admin2@test.com", IsActive = false, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" }
        );
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = await repository.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("admin1", result.First().Username);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsAdmin()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "admin", Email = "admin@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("admin", result.Username);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new AdminRepository(context, _mockLogger.Object);

        var result = await repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_ValidUsername_ReturnsAdmin()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "admin", Email = "admin@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = await repository.GetByUsernameAsync("admin");

        Assert.NotNull(result);
        Assert.Equal("admin", result.Username);
    }

    [Fact]
    public async Task GetByUsernameAsync_InvalidUsername_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new AdminRepository(context, _mockLogger.Object);

        var result = await repository.GetByUsernameAsync("nonexistent");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_InactiveAdmin_ReturnsNull()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "inactive", Email = "inactive@test.com", IsActive = false, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = await repository.GetByUsernameAsync("inactive");

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ValidAdmin_AddsAdminSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new AdminRepository(context, _mockLogger.Object);
        var admin = new Admin { Username = "newadmin", Email = "new@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" };

        var result = await repository.AddAsync(admin);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("newadmin", result.Username);
    }

    [Fact]
    public async Task UpdateAsync_ValidAdmin_UpdatesAdminSuccessfully()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "oldadmin", Email = "old@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var admin = await context.Admins.FindAsync(1);
        admin!.Email = "updated@test.com";
        var result = await repository.UpdateAsync(admin);

        Assert.NotNull(result);
        Assert.Equal("updated@test.com", result.Email);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_SoftDeletesAdmin()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "delete", Email = "delete@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        await repository.DeleteAsync(1);

        var admin = await context.Admins.FindAsync(1);
        Assert.NotNull(admin);
        Assert.False(admin.IsActive);
        Assert.NotNull(admin.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_InvalidId_DoesNotThrow()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new AdminRepository(context, _mockLogger.Object);

        await repository.DeleteAsync(999);

        Assert.True(true);
    }

    [Fact]
    public async Task ExistsAsync_ValidId_ReturnsTrue()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "exists", Email = "exists@test.com", IsActive = true, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_InvalidId_ReturnsFalse()
    {
        using var context = new TourManagementDbContext(_options);
        var repository = new AdminRepository(context, _mockLogger.Object);

        var result = await repository.ExistsAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_InactiveAdmin_ReturnsFalse()
    {
        using var context = new TourManagementDbContext(_options);
        context.Admins.Add(new Admin { Id = 1, Username = "inactive", Email = "inactive@test.com", IsActive = false, CreatedDate = DateTime.UtcNow, PasswordHash = "hash", CreatedBy = "System" });
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = await repository.ExistsAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAsync_OrdersByCreatedDateDescending()
    {
        using var context = new TourManagementDbContext(_options);
        var oldDate = DateTime.UtcNow.AddDays(-5);
        var newDate = DateTime.UtcNow;
        context.Admins.AddRange(
            new Admin { Id = 1, Username = "old", Email = "old@test.com", IsActive = true, CreatedDate = oldDate, PasswordHash = "hash", CreatedBy = "System" },
            new Admin { Id = 2, Username = "new", Email = "new@test.com", IsActive = true, CreatedDate = newDate, PasswordHash = "hash", CreatedBy = "System" }
        );
        await context.SaveChangesAsync();

        var repository = new AdminRepository(context, _mockLogger.Object);
        var result = (await repository.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("new", result[0].Username);
        Assert.Equal("old", result[1].Username);
    }
}
