using MealLedger.Models;

namespace MealLedger.IRepository
{
    public interface IEmployeeRepository
    {  
           public Task<Employee> GetByWorkdayIDAsync(string workdayID);
        
    }
}
