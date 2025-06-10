using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Elomoas.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(ILogger<LanguageController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            _logger.LogInformation("Setting language to: {Culture}", culture);

            if (string.IsNullOrEmpty(culture))
            {
                _logger.LogWarning("Culture parameter is null or empty");
                return BadRequest("Culture parameter is required");
            }

            try
            {
                var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
                _logger.LogInformation("Created cookie value: {CookieValue}", cookieValue);

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    cookieValue,
                    new CookieOptions { 
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        SameSite = SameSiteMode.Lax
                    }
                );

                _logger.LogInformation("Language cookie set successfully");

                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "~/";
                }

                _logger.LogInformation("Redirecting to: {ReturnUrl}", returnUrl);

                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting language to {Culture}", culture);
                throw;
            }
        }
    }
} 