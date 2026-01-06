using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

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
            var user = await _userService.AuthenticateAsync(Email, Password);

            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", Email);
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FirstName);

            _logger.LogInformation("User logged in successfully: {Email}", Email);
            TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";

            return RedirectToPage("/Tours/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Email);
            ModelState.AddModelError(string.Empty, "An error occurred during login.");
            return Page();
        }
    }
}
