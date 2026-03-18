using MealLedger.DTO;

namespace MealLedger.IRepository
{
    public interface IAdminRepository
    {
        Task<List<AdminRegistrationDto>> GetRegistrationsAsync(
           DateTime fromDate, DateTime toDate,
           string location, string preference);

        Task<List<string>> GetLocationsAsync();
    }
}
