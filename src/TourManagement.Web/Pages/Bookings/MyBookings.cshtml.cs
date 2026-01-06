using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Bookings;

public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<Booking> Bookings { get; set; } = new List<Booking>();

    [BindProperty(SupportsGet = true)]
    public string Email { get; set; } = string.Empty;

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(Email))
        {
            try
            {
                Bookings = await _bookingService.GetByUserEmailAsync(Email);
                _logger.LogInformation("Retrieved {Count} bookings for email: {Email}", Bookings.Count(), Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings for email: {Email}", Email);
                Bookings = new List<Booking>();
            }
        }
    }
}
