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

            return ShowListAsync(request, "Yönetici Onayı Bekleyen Cezalar", "Pending");
        }

        [Authorize(Roles = RoleName.Finance + "," + RoleName.SuperAdmin)]
        [HttpGet("pending-finance")]
        public Task<IActionResult> PendingFinance(FineFilterRequest request)
        {
            request.Status = FineStatus.ManagerApproved;

            return ShowListAsync(
                request,
                "Finans Onayı Bekleyenler",
                "Pending");
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
                return RedirectToAction(nameof(PendingAdmin));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await fineService.ApproveAsync(id, request, userId);

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Ceza onaylandı." : result.Message;

            return RedirectToAction(nameof(PendingAdmin));
        }

        [Authorize(Roles =
            RoleName.Admin + "," +
            RoleName.Finance + "," +
            RoleName.SuperAdmin)]
        [HttpPost("reject/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, FineDecisionRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Geçersiz işlem bilgisi.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await fineService.RejectAsync(id, request, userId);

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess ? "Ceza reddedildi." : result.Message;

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles =
            RoleName.Officer + "," +
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var canViewAll =
                User.IsInRole(RoleName.Admin) ||
                User.IsInRole(RoleName.Finance) ||
                User.IsInRole(RoleName.SuperAdmin);

            if (!canViewAll &&
                (string.IsNullOrWhiteSpace(userId) ||
                 result.Data.CreatedByUserId != userId))
            {
                return Forbid();
            }


            ViewData["CanEdit"] = CanEdit(result.Data);

            return View(result.Data);
        }

        [Authorize(Roles = 
            RoleName.Officer + "," + 
            RoleName.Admin + "," +
            RoleName.SuperAdmin)]
        [HttpPost("update/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateFineRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var existingResult = await fineService.GetByIdAsync(id);

            if (!existingResult.IsSuccess || existingResult.Data is null)
                return NotFound();

            if (!CanEdit(existingResult.Data))
                return Forbid();

            if (!ModelState.IsValid)
                return View(request);

            var result = await fineService.UpdateAsync(id, request, userId);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }

            TempData["SuccessMessage"] = "Ceza bilgileri güncellendi.";

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = RoleName.Officer + "," + RoleName.SuperAdmin)]
        [HttpGet("created-by-me")]
        public async Task<IActionResult> CreatedByMe(FineFilterRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            ViewData["Filter"] = request;

            if (!ModelState.IsValid)
                return View(new List<FineListResponse>());

            var result = await fineService.GetByCreatedByUserId(request, userId);

            if (!result.IsSuccess || result.Data is null)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                return View(new List<FineListResponse>());
            }

            return View(result.Data);
        }

        [Authorize(Roles =
            RoleName.Officer + "," +
            RoleName.Admin + "," +
            RoleName.Finance + "," +
            RoleName.SuperAdmin)]
        [HttpGet("process/{id:int}")]
        public async Task<IActionResult> Process(int id)
        {
            var result = await fineService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
                return NotFound();

            var fine = result.Data;

            var canViewAll = User.IsInRole(RoleName.Admin)
                || User.IsInRole(RoleName.Finance)
                || User.IsInRole(RoleName.SuperAdmin);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isCreator = !string.IsNullOrWhiteSpace(userId)
                && fine.CreatedByUserId == userId;

            if (!canViewAll && !isCreator)
                return Forbid();

            return View(fine);
        }

        [Authorize(Roles = 
            RoleName.SuperAdmin + "," +
            RoleName.Admin + "," +
            RoleName.Officer)]
        [HttpGet("update/{id:int}")]
        public async Task<IActionResult> Update(int id)
        {
            var result = await fineService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
                return NotFound();

            if (!CanEdit(result.Data))
                return Forbid();

            var fine = result.Data;

            return View(new UpdateFineRequest
            {
                Amount = fine.Amount,
                Description = fine.Description,
                FineDate = fine.FineDate
            });
        }

        private async Task<IActionResult> ShowListAsync(
            FineFilterRequest request,
            string title,
            string viewName = "Index")
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

                return View(viewName, new List<FineListResponse>());
            }

            return View(viewName, result.Data); ;
        }

        private bool CanEdit(FineDetailResponse fine)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var editableStatus =
                fine.Status == FineStatus.New ||
                fine.Status == FineStatus.Rejected;

            if (!editableStatus || fine.IsPaid)
                return false;

            return User.IsInRole(RoleName.SuperAdmin) ||
                (User.IsInRole(RoleName.Officer) &&
                 fine.CreatedByUserId == userId);
        }
    }
}
