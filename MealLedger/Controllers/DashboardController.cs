using MealLedger.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealLedger.Controllers
{
    public class DashboardController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // Session check
            var workdayID = HttpContext.Session.GetString("WorkdayID");
            if (string.IsNullOrEmpty(workdayID))
                return RedirectToAction("Login", "Account");

            var fullName = HttpContext.Session.GetString("FullName");
            var role = HttpContext.Session.GetString("Role");
            var location = HttpContext.Session.GetString("Location");

            var vm = await _dashboardService.GetDashboardDataAsync(workdayID, role, fullName, location);

            ViewData["Title"] = "Dashboard";
            ViewData["Subtitle"] = $"Welcome back, {fullName}!";
            ViewData["ActivePage"] = "Dashboard";

            return View(vm);
        }

    }
}
