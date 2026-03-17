using MealLedger.Models;

namespace MealLedger.IServices
{
    public interface IAccountService
    {
        Task<Employee> LoginAsync(string workdayID);
    }
}
