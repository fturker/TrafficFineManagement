using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrafficFineManagement.Web.Models;
using TrafficFineManagement.Web.Models.DTOs.User;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return Challenge();

            var response = new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Name = user.Name,
                Surname = user.Surname,
                AccountType = user.AccountType,
                IdentityNumber = user.IdentityNumber,
                CompanyName = user.CompanyName,
                TaxNumber = user.TaxNumber,
                Address = user.Address,
                BirthDate = user.BirthDate,
                CreatedAt = user.CreatedAt,
                LastUpdatedAt = user.LastUpdatedAt
            };

            return View(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
