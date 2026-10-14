using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.DTOs.Fine;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Controllers
{
    [Route("fine")]
    [Authorize(Roles =
        RoleName.Officer + "," +
        RoleName.Admin + "," +
        RoleName.Finance + "," +
        RoleName.SuperAdmin)]
    public class FineController(IFineService fineService) : Controller
    {
        [Authorize(Roles =
            RoleName.Admin + "," +
            RoleName.Finance + "," +
            RoleName.SuperAdmin)]
        [HttpGet("index")]
        public Task<IActionResult> Index(FineFilterRequest request)
        {
            return ShowListAsync(request, "Cezalar");
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.SuperAdmin)]
        [HttpGet("pending-admin")]
        public Task<IActionResult> PendingAdmin(FineFilterRequest request)
        {
            request.Status = FineStatus.New;

            return ShowListAsync(request, "Yönetici Onayı Bekleyen Cezalar");
        }

        [Authorize(Roles = RoleName.Finance + "," + RoleName.SuperAdmin)]
        [HttpGet("pending-finance")]
        public Task<IActionResult> PendingFinance(FineFilterRequest request)
        {
            request.Status = FineStatus.ManagerApproved;

            return ShowListAsync(
                request,
                "Finans Onayı Bekleyenler");
        }

        [Authorize(Roles = RoleName.Officer + "," + RoleName.SuperAdmin)]
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CreateFineRequest
            {
                FineDate = DateTime.Today
            });
        }


        private async Task<IActionResult> ShowListAsync(
            FineFilterRequest request,
            string title)
        {
            ViewData["Title"] = title;

            if (!ModelState.IsValid)
            {
                return View("Index", new List<FineListResponse>());
            }

            var result = await fineService.GetAllAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                return View("Index", new List<FineListResponse>());
            }

            return View("Index", result.Data);
        }
    }
}
