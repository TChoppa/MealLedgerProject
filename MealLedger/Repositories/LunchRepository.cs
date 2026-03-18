using MealLedger.ApplicationDbContext;
using MealLedger.IRepository;
using MealLedger.Models;
using Microsoft.EntityFrameworkCore;

namespace MealLedger.Repositories
{
    public class LunchRepository :ILunchRepository
    {
        private readonly AppDbContext _context;

        public LunchRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all future registered days with preference
        public async Task<List<LunchRegistration>> GetRegisteredDaysAsync(string workdayID)
        {
            return await _context.LunchRegistrations
                .Where(x => x.WorkdayID == workdayID
                         && x.LunchDate.Date >= DateTime.Today)
                .OrderBy(x => x.LunchDate)
                .ToListAsync();
        }

        // Get holidays by location
        public async Task<List<Holiday>> GetHolidaysAsync(string location)
        {
            return await _context.Holidays
                .Where(x => x.Location == location || x.Location == "All")
                .ToListAsync();
        }

        // Insert or Update registration
        public async Task UpsertLunchAsync(string workdayID, DateTime date, string preference)
        {
            var existing = await GetRegistrationAsync(workdayID, date);

            if (existing != null)
            {
                existing.Preference = preference;
                existing.RegisteredOn = DateTime.Now;
                _context.LunchRegistrations.Update(existing);
            }
            else
            {
                await _context.LunchRegistrations.AddAsync(new LunchRegistration
                {
                    WorkdayID = workdayID,
                    LunchDate = date,
                    Preference = preference,
                    RegisteredOn = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
        }

        // Get single registration
        public async Task<LunchRegistration?> GetRegistrationAsync(string workdayID, DateTime date)
        {
            return await _context.LunchRegistrations
                .FirstOrDefaultAsync(x => x.WorkdayID == workdayID
                                       && x.LunchDate.Date == date.Date);
        }
        public async Task RemoveRegistrationAsync(string workdayID, DateTime date)
        {
            var existing = await GetRegistrationAsync(workdayID, date);
            if (existing != null)
            {
                _context.LunchRegistrations.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
