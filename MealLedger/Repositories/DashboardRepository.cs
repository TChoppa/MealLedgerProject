using MealLedger.ApplicationDbContext;
using MealLedger.IRepository;
using MealLedger.Models;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace MealLedger.Repositories
{
    public class DashboardRepository: IDashboardRepositorycs
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTodayRegisteredAsync(string workdayID, DateTime today)
        {
            return await _context.LunchRegistrations
                .AnyAsync((x => x.WorkdayID == workdayID
                            && x.LunchDate.Date == today.Date));
        }

        public async Task<List<LunchRegistration>> GetThisWeekRegistrationsAsync(
            string workdayID, DateTime weekStart, DateTime weekEnd)
        {
            return await _context.LunchRegistrations
                .Where(x => x.WorkdayID == workdayID
                         && x.LunchDate.Date >= weekStart.Date
                         && x.LunchDate.Date <= weekEnd.Date)
                .OrderBy(x => x.LunchDate)
                .ToListAsync();
        }

        public async Task<int> GetTotalRegisteredTodayAsync(DateTime today, string location)
        {
            return await _context.LunchRegistrations
                .Include(x => x.Employee)
                .Where(x => x.LunchDate.Date == today.Date
                         && x.Employee.Location == location
                         && x.Employee.IsActive)
                .CountAsync();
        }

        public async Task<int> GetVegCountTodayAsync(DateTime today, string location)
        {
            return await _context.LunchRegistrations
                .Include(x => x.Employee)
                .Where(x => x.LunchDate.Date == today.Date
                         && x.Preference == "Veg"
                         && x.Employee.Location == location
                         && x.Employee.IsActive)
                .CountAsync();
        }

        public async Task<int> GetNonVegCountTodayAsync(DateTime today, string location)
        {
            return await _context.LunchRegistrations
                .Include(x => x.Employee)
                .Where(x => x.LunchDate.Date == today.Date
                         && x.Preference == "NonVeg"
                         && x.Employee.Location == location
                         && x.Employee.IsActive)
                .CountAsync();
        }
    }
}
