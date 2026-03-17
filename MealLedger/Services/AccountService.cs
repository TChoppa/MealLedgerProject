using MealLedger.IRepository;
using MealLedger.IServices;
using MealLedger.Models;

namespace MealLedger.Services
{
    public class AccountService:IAccountService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AccountService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> LoginAsync(string workdayID)
        {
            if (string.IsNullOrWhiteSpace(workdayID))
                return null;

            return await _employeeRepository.GetByWorkdayIDAsync(workdayID.Trim());
        }
    }
}
