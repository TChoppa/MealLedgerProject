using MealLedger.DTO;

namespace MealLedger.IServices
{
    public interface ILunchService
    {
        Task<LunchScheduleViewModel> GetScheduleAsync(string workdayID, string location, int month, int year);
        Task<(bool success, string message)> SaveRegistrationsAsync(string workdayID, string location, LunchRegistrationRequestDto request);
        Task<(bool success, string message)> RemoveRegistrationAsync(string workdayID, DateTime date);
    }
}
