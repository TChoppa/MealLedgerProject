using MealLedger.DTO;

namespace MealLedger.IServices
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync(string workdayID, string role, string fullName, string location);
    }
}
