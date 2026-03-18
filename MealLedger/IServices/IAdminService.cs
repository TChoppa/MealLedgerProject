using MealLedger.DTO;

namespace MealLedger.IServices
{
    public interface IAdminService
    {
        Task<AdminViewModel> GetAdminDataAsync(AdminFilterDto filter);
        Task<List<AdminRegistrationDto>> GetFilteredRegistrationsAsync(AdminFilterDto filter);
    }
}
