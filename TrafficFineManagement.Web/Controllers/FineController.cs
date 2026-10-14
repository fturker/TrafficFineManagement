using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        [Authorize(Roles = 
            RoleName.Officer + "," + 
            RoleName.Admin + "," +
            RoleName.SuperAdmin)]
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFineRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await fineService.CreateAsync(request, userId);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }

            TempData["SuccessMessage"] = "Ceza oluşturuldu.";

            return RedirectToAction(nameof(Create));
        }

        [Authorize(Roles =
            RoleName.Admin + "," +
            RoleName.Finance + "," +
            RoleName.SuperAdmin)]
        [HttpPost("approve/{id:int}")]
        public async Task<IActionResult> Approve(int id, FineDecisionRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Geçersiz işlem bilgisi.";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await fineService.ApproveAsync(id, request, userId);

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Ceza onaylandı." : result.Message;

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles =
            RoleName.Admin + "," +
            RoleName.Finance + "," +
            RoleName.SuperAdmin)]
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await fineService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            ViewData["CanEdit"] = CanEdit(result.Data);

            return View(result.Data);
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

        private bool CanEdit(FineDetailResponse fine)
        {
            return User.IsInRole(RoleName.SuperAdmin)
                && fine.Status == FineStatus.New
                && !fine.IsPaid;
        }
    }
}
