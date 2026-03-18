using MealLedger.Models;

namespace MealLedger.IRepository
{
    public interface ILunchRepository
    {
        Task<List<LunchRegistration>> GetRegisteredDaysAsync(string workdayID);
        Task<List<Holiday>> GetHolidaysAsync(string location);
        Task UpsertLunchAsync(string workdayID, DateTime date, string preference);
        Task<LunchRegistration?> GetRegistrationAsync(string workdayID, DateTime date);
        Task RemoveRegistrationAsync(string workdayID, DateTime date);
    }
}
