using TourManagement.Domain.DTOs;

namespace TourManagement.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(string email, UserUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(string email, CancellationToken cancellationToken = default);
    Task<UserDto?> ValidateUserAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
