using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.DTOs.Car;
using TrafficFineManagement.Web.Services.Implementations;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Controllers
{
    [Authorize(Roles =
        RoleName.Admin + "," +
        RoleName.SuperAdmin)]
    [Route("car")]
    public class CarController(ICarService carService) : Controller
    {
        [HttpGet("index")]
        public async Task<IActionResult> Index(CarFilterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(new List<CarListResponse>());
            }
            var result = await carService.GetAllAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(new List<CarListResponse>());
            }

            return View(result.Data);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CreateCarRequest());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCarRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await carService.CreateAsync(request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await carService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpGet("update/{id:int}")]
        public async Task<IActionResult> Update(int id)
        {
            var result = await carService.GetByIdAsync(id);

            if (!result.IsSuccess || result.Data is null)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var car = result.Data;

            return View(new UpdateCarRequest
            {
                Plate = car.Plate,
                CarType = car.CarType,
                Brand = car.Brand,
                Model = car.Model,
                OwnerId = car.OwnerId ?? string.Empty
            });
        }

        [HttpPost("update/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            int id,
            UpdateCarRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await carService.UpdateAsync(id, request);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(request);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await carService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

