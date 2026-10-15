using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Controllers
{
    [Authorize(Roles =
            RoleName.Citizen + "," +
            RoleName.Admin + "," +
            RoleName.Finance + "," +
            RoleName.SuperAdmin)]
    [Route("payment")]
    public class PaymentController(IPaymentService paymentService, IFineService fineService) : Controller
    {
        [HttpGet("pay/{fineId:int}")]
        public async Task<IActionResult> Pay(int fineId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await fineService.GetByIdAsync(fineId);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction("MyFines", "My");
            }

            return View(result.Data);
        }

        [HttpPost("pay/{fineId:int}")]
        public async Task<IActionResult> PayConfirmed(int fineId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var result = await paymentService.PayAsync(fineId, userId);

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] =
                result.IsSuccess
                    ? "Ödeme başaralı bir şekilde gerçekleştirildi"
                    : result.Message;

            return RedirectToAction("MyFines", "My");
        }
    }
}
