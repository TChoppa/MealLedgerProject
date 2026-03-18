using MealLedger.DTO;
using MealLedger.IRepository;
using MealLedger.IServices;

namespace MealLedger.Services
{
    public class LunchService : ILunchService
    {
        private readonly ILunchRepository _repo;

        public LunchService(ILunchRepository repo)
        {
            _repo = repo;
        }

        public async Task<LunchScheduleViewModel> GetScheduleAsync(
            string workdayID, string location, int month, int year)
        {
            var registrations = await _repo.GetRegisteredDaysAsync(workdayID);
            var holidays = await _repo.GetHolidaysAsync(location);

            return new LunchScheduleViewModel
            {
                CurrentMonth = month,
                CurrentYear = year,

                // Map each registration to date + preference
                RegisteredDays = registrations.Select(r => new RegisteredDayDto
                {
                    Date = r.LunchDate.Date.ToString("yyyy-MM-dd"),
                    Preference = r.Preference
                }).ToList(),

                // Map holidays
                Holidays = holidays.Select(h => new HolidayDto
                {
                    Date = h.HolidayDate.Date.ToString("yyyy-MM-dd"),
                    Type = h.Type,
                    Name = h.HolidayName
                }).ToList()
            };
        }

        public async Task<(bool success, string message)> SaveRegistrationsAsync(
            string workdayID, string location, LunchRegistrationRequestDto request)
        {
            if (request.Days == null || !request.Days.Any())
                return (false, "No days selected.");

            var holidays = await _repo.GetHolidaysAsync(location);

            var companyHolidays = holidays
                .Where(h => h.Type == "Company")
                .Select(h => h.HolidayDate.Date)
                .ToList();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var cutoff = DateTime.Now.Hour >= 16; // 4 PM

            foreach (var day in request.Days)
            {
                // Parse date
                if (!DateTime.TryParse(day.Date, out var date))
                    return (false, $"Invalid date format: {day.Date}");

                date = date.Date;

                // Past or today → block
                if (date <= today)
                    return (false, $"{date:dd MMM} is in the past or today. Cannot register.");

                // Tomorrow after 4 PM → block
                if (date == tomorrow && cutoff)
                    return (false, "Registration for tomorrow is closed after 4:00 PM.");

                // Company holiday → block
                if (companyHolidays.Contains(date))
                    return (false, $"{date:dd MMM} is a company holiday. Cannot register.");

                // Weekend → block
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    return (false, $"{date:dd MMM} is a weekend. Cannot register.");

                // Validate preference
                if (day.Preference != "Veg" && day.Preference != "NonVeg")
                    return (false, $"Invalid preference for {date:dd MMM}. Choose Veg or NonVeg.");

                // Save or update
                await _repo.UpsertLunchAsync(workdayID, date, day.Preference);
            }

            var count = request.Days.Count;
            return (true, $"Successfully registered {count} day{(count > 1 ? "s" : "")}!");
        }

        public async Task<(bool success, string message)> RemoveRegistrationAsync(
    string workdayID, DateTime date)
        {
            var existing = await _repo.GetRegistrationAsync(workdayID, date);

            if (existing == null)
                return (false, "No registration found for this date.");

            await _repo.RemoveRegistrationAsync(workdayID, date);

            return (true, $"Registration for {date:dd MMM} removed successfully.");
        }
    }
}
