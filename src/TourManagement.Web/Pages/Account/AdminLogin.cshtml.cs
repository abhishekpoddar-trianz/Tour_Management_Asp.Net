using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Account;

public class AdminLoginModel : PageModel
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AdminLoginModel> _logger;

    public AdminLoginModel(IAdminService adminService, ILogger<AdminLoginModel> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

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
            var admin = await _adminService.AuthenticateAsync(Username, Password);

            if (admin == null)
            {
                _logger.LogWarning("Failed admin login attempt for username: {Username}", Username);
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            HttpContext.Session.SetString("AdminUsername", admin.Username);
            HttpContext.Session.SetString("IsAdmin", "true");

            _logger.LogInformation("Admin logged in successfully: {Username}", Username);
            TempData["SuccessMessage"] = $"Welcome, Admin {admin.Username}!";

            return RedirectToPage("/Tours/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin login for username: {Username}", Username);
            ModelState.AddModelError(string.Empty, "An error occurred during login.");
            return Page();
        }
    }
}
