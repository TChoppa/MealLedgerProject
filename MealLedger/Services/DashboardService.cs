using MealLedger.DTO;
using MealLedger.IRepository;
using MealLedger.IServices;

namespace MealLedger.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IDashboardRepositorycs _repo;

        public DashboardService(IDashboardRepositorycs repo)
        {
            _repo = repo;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync(
            string workdayID, string role, string fullName, string location)
        {
            var today = DateTime.Today;

            // Get current week Monday to Friday
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = today.AddDays(-diff);
            var weekEnd = weekStart.AddDays(4); // Friday

            var vm = new DashboardViewModel
            {
                FullName = fullName,
                Role = role,
                Location = location,
                Today = today
            };

            if (role == "Admin")
            {
                vm.TotalRegisteredToday = await _repo.GetTotalRegisteredTodayAsync(today, location);
                vm.VegCountToday = await _repo.GetVegCountTodayAsync(today, location);
                vm.NonVegCountToday = await _repo.GetNonVegCountTodayAsync(today, location);
            }
            else
            {
                vm.IsTodayRegistered = await _repo.IsTodayRegisteredAsync(workdayID, today);
                vm.ThisWeekRegistrations = await _repo.GetThisWeekRegistrationsAsync(workdayID, weekStart, weekEnd);
            }

            return vm;
        }
    }
}
