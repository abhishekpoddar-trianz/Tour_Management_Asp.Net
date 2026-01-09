using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Tours;

public class CreateModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        ITourService tourService,
        IWebHostEnvironment environment,
        ILogger<CreateModel> logger)
    {
        _tourService = tourService;
        _environment = environment;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
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

        public IFormFile? PictureFile { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            string? picturePath = null;

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
            }

            var createDto = new TourCreateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Days = Input.Days,
                Price = Input.Price,
                Locations = Input.Locations,
                TourInfo = Input.TourInfo,
                PicturePath = picturePath
            };

            await _tourService.CreateAsync(createDto);

            TempData["SuccessMessage"] = "Tour created successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the tour.");
            return Page();
        }
    }
}
