using MealLedger.IRepository;
using MealLedger.IServices;
using MealLedger.Models;
using MealLedger.Repositories;
using MealLedger.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace MealLedger.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in redirect accordingly
            //if (HttpContext.Session.GetString("WorkdayID") != null)
            //{
            //    var role = HttpContext.Session.GetString("Role");
            //    return role == "Admin" ? RedirectToAction("Index", "Admin")
            //                           : RedirectToAction("Index", "Dashboard");
            //}
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string workdayID)
        {
            var employee = await _accountService.LoginAsync(workdayID);

            if (employee == null)
                return Json(new { success = false, message = "Invalid Workday ID or account is inactive." });

            // Store in session
            HttpContext.Session.SetString("WorkdayID", employee.WorkdayID);
            HttpContext.Session.SetString("FullName", employee.FullName);
            HttpContext.Session.SetString("Role", employee.Role);
            HttpContext.Session.SetString("Location", employee.Location);

            var redirectUrl = employee.Role == "Admin" ? "/Admin/Index" : "/Dashboard/Index";

            return Json(new { success = true, redirectUrl });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    
        public IActionResult Index()
        {
            return View();
        }
    }
}
