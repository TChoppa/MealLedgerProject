using MealLedger.Models;

namespace MealLedger.IRepository
{
    public interface IDashboardRepositorycs
    {
        Task<bool> IsTodayRegisteredAsync(string workdayID, DateTime today);
        Task<List<LunchRegistration>> GetThisWeekRegistrationsAsync(string workdayID, DateTime weekStart, DateTime weekEnd);
        Task<int> GetTotalRegisteredTodayAsync(DateTime today, string location);
        Task<int> GetVegCountTodayAsync(DateTime today, string location);
        Task<int> GetNonVegCountTodayAsync(DateTime today, string location);
    }
}
