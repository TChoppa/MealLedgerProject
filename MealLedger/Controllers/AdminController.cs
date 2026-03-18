using MealLedger.DTO;
using MealLedger.IServices;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace MealLedger.Controllers
{
    public class AdminController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // POST: Admin/Filter (Ajax)
        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] AdminFilterDto filter)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return Json(new { success = false, message = "Unauthorized." });

            var registrations = await _adminService.GetFilteredRegistrationsAsync(filter);

            var result = registrations.Select((r, i) => new
            {
                sno = i + 1,
                workdayID = r.WorkdayID,
                fullName = r.FullName,
                department = r.Department,
                location = r.Location,
                preference = r.Preference,
                lunchDate = r.LunchDate.ToString("dd MMM yyyy")
            });

            return Json(new
            {
                success = true,
                data = result,
                total = registrations.Count,
                vegCount = registrations.Count(r => r.Preference == "Veg"),
                nonVegCount = registrations.Count(r => r.Preference == "NonVeg")
            });
        }
        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            var workdayID = HttpContext.Session.GetString("WorkdayID");
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(workdayID))
                return RedirectToAction("Login", "Account");

            if (role != "Admin")
                return RedirectToAction("Index", "Dashboard");

            var filter = new AdminFilterDto
            {
                FromDate = DateTime.Today.ToString("yyyy-MM-dd"),
                ToDate = DateTime.Today.ToString("yyyy-MM-dd"),
                Location = "All",
                Preference = "All"
            };

            var vm = await _adminService.GetAdminDataAsync(filter);

            ViewData["Title"] = "Admin Panel";
            ViewData["Subtitle"] = "Manage lunch registrations";
            ViewData["ActivePage"] = "Admin";

            return View(vm);
        }
        public async Task<IActionResult> ExportExcel(
      string fromDate, string toDate,
      string location, string preference)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return RedirectToAction("Login", "Account");

            var filter = new AdminFilterDto
            {
                FromDate = fromDate ?? DateTime.Today.ToString("yyyy-MM-dd"),
                ToDate = toDate ?? DateTime.Today.ToString("yyyy-MM-dd"),
                Location = location ?? "All",
                Preference = preference ?? "All"
            };

            var registrations = await _adminService.GetFilteredRegistrationsAsync(filter);

            ExcelPackage.License.SetNonCommercialPersonal("MealLedger");

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Lunch");

            // ── Title Row ──
            sheet.Cells[1, 1].Value = "MealLedger — Lunch Registrations";
            sheet.Cells[1, 1, 1, 4].Merge = true;
            sheet.Cells[1, 1].Style.Font.Bold = true;
            sheet.Cells[1, 1].Style.Font.Size = 14;
            sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
            sheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor
                 .SetColor(Color.FromArgb(74, 45, 156));
            sheet.Row(1).Height = 28;

            // ── Filter Info Row ──
            var dateRange = fromDate == toDate
                ? $"Date: {fromDate}"
                : $"From: {fromDate}  To: {toDate}";

            sheet.Cells[2, 1].Value = $"{dateRange}  |  Location: {location}";
            sheet.Cells[2, 1, 2, 4].Merge = true;
            sheet.Cells[2, 1].Style.Font.Italic = true;
            sheet.Cells[2, 1].Style.Font.Size = 10;
            sheet.Cells[2, 1].Style.Font.Color.SetColor(Color.FromArgb(91, 33, 182));
            sheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(2).Height = 18;

            // ── Summary Row — merged across all 4 columns, centered ──
            sheet.Cells[3, 1, 3, 4].Merge = true;
            sheet.Cells[3, 1].Value =
                $"Total: {registrations.Count}          " +
                $"Veg: {registrations.Count(r => r.Preference == "Veg")}          " +
                $"Non-Veg: {registrations.Count(r => r.Preference == "NonVeg")}";
            sheet.Cells[3, 1].Style.Font.Bold = true;
            sheet.Cells[3, 1].Style.Font.Size = 11;
            sheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[3, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[3, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[3, 1].Style.Fill.BackgroundColor
                 .SetColor(Color.FromArgb(237, 233, 254));
            sheet.Cells[3, 1].Style.Font.Color.SetColor(Color.FromArgb(74, 45, 156));
            sheet.Row(3).Height = 20;

            // ── Header Row ──
            var headers = new[] { "S.No", "Work ID", "Name", "Signature" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = sheet.Cells[4, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor
                     .SetColor(Color.FromArgb(124, 58, 237));
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.White);
            }
            sheet.Row(4).Height = 22;

            // ── Data Rows ──
            for (int i = 0; i < registrations.Count; i++)
            {
                var r = registrations[i];
                var row = i + 5;
                var isAlt = i % 2 == 1;

                sheet.Cells[row, 1].Value = i + 1;       // S.No
                sheet.Cells[row, 2].Value = r.WorkdayID; // Work ID
                sheet.Cells[row, 3].Value = r.FullName;  // Name
                sheet.Cells[row, 4].Value = "";          // Signature — empty

                sheet.Row(row).Height = 24;

                // Alternate row color
                if (isAlt)
                {
                    sheet.Cells[row, 1, row, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[row, 1, row, 4].Style.Fill.BackgroundColor
                         .SetColor(Color.FromArgb(237, 233, 254));
                }

                // Row border
                sheet.Cells[row, 1, row, 4].Style.Border.BorderAround(
                    ExcelBorderStyle.Thin, Color.FromArgb(209, 196, 233));

                // Center S.No and WorkID
                sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Signature cell — thick purple border
                sheet.Cells[row, 4].Style.Border.BorderAround(
                    ExcelBorderStyle.Medium, Color.FromArgb(124, 58, 237));
            }

            // ── Column Widths ──
            sheet.Column(1).Width = 8;   // S.No
            sheet.Column(2).Width = 15;  // Work ID
            sheet.Column(3).Width = 30;  // Name
            sheet.Column(4).Width = 25;  // Signature

            // ── Print Settings ──
            sheet.PrinterSettings.Orientation = eOrientation.Portrait;
            sheet.PrinterSettings.PaperSize = ePaperSize.A4;
            sheet.PrinterSettings.FitToPage = true;
            sheet.PrinterSettings.FitToWidth = 1;
            sheet.PrinterSettings.FitToHeight = 0;
            sheet.PrinterSettings.TopMargin = (double)0.5m;
            sheet.PrinterSettings.BottomMargin = (double)0.5m;
            sheet.PrinterSettings.LeftMargin = (double)0.5m;
            sheet.PrinterSettings.RightMargin = (double)0.5m;

            var fileName = $"Lunch_{fromDate}_to_{toDate}_{location}.xlsx";
            var content = package.GetAsByteArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

       

    }
}
