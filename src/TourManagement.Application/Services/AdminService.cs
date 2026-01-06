using Microsoft.Extensions.Logging;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Application.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly ILogger<AdminService> _logger;

    public AdminService(IAdminRepository adminRepository, ILogger<AdminService> logger)
    {
        _adminRepository = adminRepository;
        _logger = logger;
    }

    public async Task<Admin?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating admin with username: {Username}", username);

            var admin = await _adminRepository.GetByUsernameAsync(username, cancellationToken);
            if (admin == null)
            {
                _logger.LogWarning("Admin with username {Username} not found", username);
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
            {
                _logger.LogWarning("Invalid password for admin: {Username}", username);
                return null;
            }

            _logger.LogInformation("Admin authenticated successfully: {Username}", username);
            return admin;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating admin: {Username}", username);
            throw;
        }
    }

    public async Task<Admin?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving admin with username: {Username}", username);
            return await _adminRepository.GetByUsernameAsync(username, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin with username: {Username}", username);
            throw;
        }
    }
}
