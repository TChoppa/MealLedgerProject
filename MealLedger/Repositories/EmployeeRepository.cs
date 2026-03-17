using MealLedger.ApplicationDbContext;
using MealLedger.IRepository;
using MealLedger.Models;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace MealLedger.Repositories
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByWorkdayIDAsync(string workdayID)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.WorkdayID == workdayID && e.IsActive == true);
        }
    }
}
