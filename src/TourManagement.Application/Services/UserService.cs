using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users");
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with email {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email {Email}", email);
            throw;
        }
    }

    public async Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user: {Email}", createDto.Email);

            var existingUser = await _userRepository.GetByEmailAsync(createDto.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with email {createDto.Email} already exists");
            }

            var user = _mapper.Map<User>(createDto);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;
            user.CreatedBy = "System";

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);
            return _mapper.Map<UserDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", createDto.Email);
            throw;
        }
    }

    public async Task UpdateAsync(string email, UserUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with email {Email}", email);

            var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found");
            }

            _mapper.Map(updateDto, existingUser);
            existingUser.ModifiedDate = DateTime.UtcNow;
            existingUser.ModifiedBy = "System";

            await _userRepository.UpdateAsync(existingUser, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email {Email}", email);
            throw;
        }
    }

    public async Task DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with email {Email}", email);

            var exists = await _userRepository.ExistsAsync(email, cancellationToken);
            if (!exists)
            {
                throw new KeyNotFoundException($"User with email {email} not found");
            }

            await _userRepository.DeleteAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with email {Email}", email);
            throw;
        }
    }

    public async Task<UserDto?> ValidateUserAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating user with email {Email}", loginDto.Email);
            var user = await _userRepository.ValidateUserAsync(loginDto.Email, loginDto.Password, cancellationToken);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user with email {Email}", loginDto.Email);
            throw;
        }
    }

    public async Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
            var users = await _userRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
