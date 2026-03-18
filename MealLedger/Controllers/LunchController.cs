using MealLedger.DTO;
using MealLedger.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealLedger.Controllers
{
    public class LunchController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly ILunchService _lunchService;

        public LunchController(ILunchService lunchService)
        {
            _lunchService = lunchService;
        }

        // GET: Lunch/Schedule
        public async Task<IActionResult> Schedule(int? month, int? year)
        {
            // Session check
            var workdayID = HttpContext.Session.GetString("WorkdayID");
            if (string.IsNullOrEmpty(workdayID))
                return RedirectToAction("Login", "Account");

            var location = HttpContext.Session.GetString("Location");
            var now = DateTime.Now;
            var m = month ?? now.Month;
            var y = year ?? now.Year;

            var vm = await _lunchService.GetScheduleAsync(workdayID, location, m, y);

            ViewData["Title"] = "Lunch Schedule";
            ViewData["Subtitle"] = "Select your lunch days";
            ViewData["ActivePage"] = "Lunch";

            return View(vm);
        }

        // POST: Lunch/Save
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] LunchRegistrationRequestDto request)
        {
            // Session check
            var workdayID = HttpContext.Session.GetString("WorkdayID");
            if (string.IsNullOrEmpty(workdayID))
                return Json(new { success = false, message = "Session expired. Please login again." });

            var location = HttpContext.Session.GetString("Location");

            if (request == null || request.Days == null || !request.Days.Any())
                return Json(new { success = false, message = "No days selected." });

            var (success, message) = await _lunchService
                                        .SaveRegistrationsAsync(workdayID, location, request);

            return Json(new { success, message });
        }

        // POST: Lunch/Remove
        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] RemoveLunchDto request)
        {
            var workdayID = HttpContext.Session.GetString("WorkdayID");
            if (string.IsNullOrEmpty(workdayID))
                return Json(new { success = false, message = "Session expired. Please login again." });

            if (!DateTime.TryParse(request.Date, out var date))
                return Json(new { success = false, message = "Invalid date." });

            date = date.Date;
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var cutoff = DateTime.Now.Hour >= 16;

            // Cannot remove past or today
            if (date <= today)
                return Json(new { success = false, message = "Cannot remove past registrations." });

            // Cannot remove tomorrow after 4 PM
            if (date == tomorrow && cutoff)
                return Json(new { success = false, message = "Cannot remove tomorrow's registration after 4:00 PM." });

            var (success, message) = await _lunchService.RemoveRegistrationAsync(workdayID, date);

            return Json(new { success, message });
        }
    }
}
