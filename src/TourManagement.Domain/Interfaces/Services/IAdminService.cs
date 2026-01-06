using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Services;

public interface IAdminService
{
    Task<Admin?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<Admin?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
