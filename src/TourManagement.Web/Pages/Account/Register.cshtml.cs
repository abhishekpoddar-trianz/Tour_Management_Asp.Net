using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string FirstName { get; set; } = string.Empty;

    [BindProperty]
    public string? LastName { get; set; }

    [BindProperty]
    public string? PhoneNumber { get; set; }

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
            var user = new User
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber
            };

            await _userService.CreateAsync(user, Password);

            _logger.LogInformation("User registered successfully: {Email}", Email);
            TempData["SuccessMessage"] = "Registration successful! Please login.";

            return RedirectToPage("./Login");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed for email: {Email}", Email);
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration.");
            return Page();
        }
    }
}
