using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Tours;

public class EditModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditModel> _logger;

    public EditModel(ITourService tourService, IWebHostEnvironment environment, ILogger<EditModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    [BindProperty]
    public Tour Tour { get; set; } = new Tour();

    [BindProperty]
    public IFormFile? PictureFile { get; set; }

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
            _logger.LogError(ex, "Error retrieving tour for edit: {TourId}", id.Value);
            return RedirectToPage("./Index");
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
            if (PictureFile != null && PictureFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "tours");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + PictureFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await PictureFile.CopyToAsync(fileStream);
                }

                if (!string.IsNullOrEmpty(Tour.PicturePath))
                {
                    var oldFilePath = Path.Combine(uploadsFolder, Tour.PicturePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                Tour.PicturePath = uniqueFileName;
            }

            await _tourService.UpdateAsync(Tour.Id, Tour);

            _logger.LogInformation("Tour updated successfully: {TourId}", Tour.Id);
            TempData["SuccessMessage"] = "Tour updated successfully!";

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour: {TourId}", Tour.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the tour.");
            return Page();
        }
    }
}
