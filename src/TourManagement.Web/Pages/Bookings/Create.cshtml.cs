using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Bookings;

public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ITourService _tourService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IBookingService bookingService, ITourService tourService, ILogger<CreateModel> logger)
    {
        _bookingService = bookingService;
        _tourService = tourService;
        _logger = logger;
    }

    [BindProperty]
    public int TourId { get; set; }

    [BindProperty]
    public string TourName { get; set; } = string.Empty;

    [BindProperty]
    public string Place { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string FirstName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int? tourId)
    {
        if (tourId == null)
        {
            return RedirectToPage("/Tours/Index");
        }

        try
        {
            var tour = await _tourService.GetByIdAsync(tourId.Value);
            if (tour != null)
            {
                TourId = tour.Id;
                TourName = tour.TourName;
                Place = tour.Place;
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking page for tour: {TourId}", tourId.Value);
            return RedirectToPage("/Tours/Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var booking = new Booking
            {
                TourId = TourId,
                TourName = TourName,
                Place = Place,
                Email = Email,
                FirstName = FirstName
            };

            await _bookingService.CreateAsync(booking);

            _logger.LogInformation("Booking created successfully for email: {Email}", Email);
            TempData["SuccessMessage"] = "Booking created successfully!";

            return RedirectToPage("./MyBookings", new { email = Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking.");
            return Page();
        }
    }
}
