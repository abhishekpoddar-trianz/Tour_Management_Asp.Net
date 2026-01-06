using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Bookings;

public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [BindProperty]
    public Booking Booking { get; set; } = new Booking();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var booking = await _bookingService.GetByIdAsync(id.Value);

            if (booking == null)
            {
                _logger.LogWarning("Booking with ID {BookingId} not found", id.Value);
                return NotFound();
            }

            Booking = booking;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking for deletion: {BookingId}", id.Value);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _bookingService.DeleteAsync(Booking.Id);

            _logger.LogInformation("Booking deleted successfully: {BookingId}", Booking.Id);
            TempData["SuccessMessage"] = "Booking deleted successfully!";

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking: {BookingId}", Booking.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the booking.");
            return Page();
        }
    }
}
