using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Tours;

public class DeleteModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITourService tourService, ILogger<DeleteModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    [BindProperty]
    public Tour Tour { get; set; } = new Tour();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var tour = await _tourService.GetByIdAsync(id.Value);

            if (tour == null)
            {
                _logger.LogWarning("Tour with ID {TourId} not found", id.Value);
                return NotFound();
            }

            Tour = tour;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour for deletion: {TourId}", id.Value);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _tourService.DeleteAsync(Tour.Id);

            _logger.LogInformation("Tour deleted successfully: {TourId}", Tour.Id);
            TempData["SuccessMessage"] = "Tour deleted successfully!";

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour: {TourId}", Tour.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the tour.");
            return Page();
        }
    }
}
