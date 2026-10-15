using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Web.Models.DTOs.Auth;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        [AllowAnonymous]
        [HttpGet("/")]
        [HttpGet("/login")]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await authService.LoginAsync(request);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index","Home");
            }

            ModelState.AddModelError(string.Empty, result.Message);

            return View();
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();

            return RedirectToAction("login", "auth");
        }

        [AllowAnonymous]
        [HttpGet("/access-denied")]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;

            return View();
        }
    }
}
