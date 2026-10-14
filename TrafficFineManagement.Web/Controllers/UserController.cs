using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.User;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Controllers
{
    [Route("user")]
    public class UserController(IUserService userService) : Controller
    {
        [Authorize(Roles = 
            RoleName.SuperAdmin + "," + 
            RoleName.Admin + "," + 
            RoleName.Finance + "," + 
            RoleName.Officer)]
        [HttpGet("index")]
        public async Task<IActionResult> Index(UserFilterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userService.GetAllAsync(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                return View(new List<UserResponse>());
            }

            return View(result.Data);
        }

        [Authorize(Roles =
            RoleName.SuperAdmin + "," +
            RoleName.Admin + "," +
            RoleName.Officer)]
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CreateUserRequest());
        }

        [Authorize(Roles =
            RoleName.SuperAdmin + "," +
            RoleName.Admin + "," +
            RoleName.Officer)]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await userService.CreateAsync(request);

            if(!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles =
            RoleName.SuperAdmin + "," +
            RoleName.Admin)]
        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(string id)
        {
            var result = await userService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var user = result.Data!;

            var request = new UpdateUserRequest
            {
                Name = user.Name,
                Surname = user.Surname,
                CompanyName = user.CompanyName,
                Address = user.Address,
                BirthDate = user.BirthDate
            };

            return View(request);
        }

        [Authorize(Roles = 
            RoleName.Admin + "," + 
            RoleName.SuperAdmin)]
        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(
            string id,
            UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await userService.UpdateAsync(id, request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
