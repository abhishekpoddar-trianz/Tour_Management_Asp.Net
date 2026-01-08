using TourManagement.Domain.DTOs;

namespace TourManagement.Domain.Interfaces.Services;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetByUserEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<BookingDto> CreateAsync(BookingCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, BookingUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
