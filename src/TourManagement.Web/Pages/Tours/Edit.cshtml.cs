using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Tours;

public class EditModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        ITourService tourService,
        IWebHostEnvironment environment,
        ILogger<EditModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TourName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Place { get; set; } = string.Empty;

        [Required]
        [Range(1, 365)]
        public int Days { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100)]
        public string Locations { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string TourInfo { get; set; } = string.Empty;

        public string? CurrentPicturePath { get; set; }

        public IFormFile? PictureFile { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var tour = await _tourService.GetByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = tour.Id,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                CurrentPicturePath = tour.PicturePath
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for editing, ID {TourId}", id);
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
            string? picturePath = Input.CurrentPicturePath;

            if (Input.PictureFile != null)
            {
                // Use environment variable for storage path or default to wwwroot
                var storageBasePath = Environment.GetEnvironmentVariable("STORAGE_BASE_PATH") ?? _environment.WebRootPath;
                var uploadsFolder = Path.Combine(storageBasePath, "images", "tours");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Input.PictureFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.PictureFile.CopyToAsync(fileStream);
                }

                picturePath = "/images/tours/" + uniqueFileName;

                if (!string.IsNullOrEmpty(Input.CurrentPicturePath))
                {
                    var oldFilePath = Path.Combine(storageBasePath, Input.CurrentPicturePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            var updateDto = new TourUpdateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                PicturePath = picturePath
            };

            await _tourService.UpdateAsync(Input.Id, updateDto);

            TempData["SuccessMessage"] = "Tour updated successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the tour.");
            return Page();
        }
    }
}
