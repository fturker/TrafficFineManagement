using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.DTOs.Car;
using TrafficFineManagement.Web.Models.DTOs.Fine;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Controllers
{
    [Authorize(Roles = 
        RoleName.Citizen + "," +
        RoleName.Officer + "," +
        RoleName.Admin + "," +
        RoleName.Finance + "," +
        RoleName.SuperAdmin)]
    public class MyController(
        ICarService carService,
        IFineService fineService) : Controller
    {
        [HttpGet("car/my-cars")]
        public async Task<IActionResult> MyCars(CarFilterRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            ViewData["Filter"] = request;

            if (!ModelState.IsValid)
                return View(new List<CarListResponse>());

            var result = await carService.GetByOwnerId(request, userId);

            if (!result.IsSuccess || result.Data is null)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<CarListResponse>());
            }

            ViewData["PageNumber"] = request.PageNumber;
            ViewData["PageSize"] = request.PageSize;
            ViewData["HasNextPage"] = result.Data.Count > request.PageSize;

            return View(result.Data);
        }

        [HttpGet("fine/my-fines")]
        public async Task<IActionResult> MyFines(FineFilterRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            ViewData["Filter"] = request;

            if (!ModelState.IsValid)
                return View(new List<FineListResponse>());

            var result = await fineService.GetByOwnerId(request, userId);

            if (!result.IsSuccess || result.Data is null)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<FineListResponse>());
            }

            ViewData["PageNumber"] = request.PageNumber;
            ViewData["PageSize"] = request.PageSize;
            ViewData["HasNextPage"] = result.Data.Count > request.PageSize;

            return View(result.Data);
        }
    }
}