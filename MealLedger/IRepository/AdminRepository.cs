using MealLedger.ApplicationDbContext;
using MealLedger.DTO;
using Microsoft.EntityFrameworkCore;

namespace MealLedger.IRepository
{
    public class AdminRepository:IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminRegistrationDto>> GetRegistrationsAsync(
            DateTime fromDate, DateTime toDate,
            string location, string preference)
        {
            var query = _context.LunchRegistrations
                .Include(x => x.Employee)
                .Where(x => x.Employee.IsActive
                         && x.LunchDate.Date >= fromDate.Date
                         && x.LunchDate.Date <= toDate.Date);

            // Location filter
            if (location != "All")
                query = query.Where(x => x.Employee.Location == location);

            // Preference filter
            if (preference != "All")
                query = query.Where(x => x.Preference == preference);

            return await query
                .OrderBy(x => x.LunchDate)
                .ThenBy(x => x.Employee.FullName)
                .Select(x => new AdminRegistrationDto
                {
                    WorkdayID = x.WorkdayID,
                    FullName = x.Employee.FullName,
                    Department = x.Employee.Department,
                    Location = x.Employee.Location,
                    Preference = x.Preference,
                    LunchDate = x.LunchDate
                })
                .ToListAsync();
        }

        public async Task<List<string>> GetLocationsAsync()
        {
            return await _context.Employees
                .Where(x => x.IsActive)
                .Select(x => x.Location)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }
    }
}
