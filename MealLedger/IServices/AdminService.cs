using MealLedger.DTO;
using MealLedger.IRepository;

namespace MealLedger.IServices
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public async Task<AdminViewModel> GetAdminDataAsync(AdminFilterDto filter)
        {
            var (fromDate, toDate) = GetDateRange(filter);
            var locations = await _repo.GetLocationsAsync();
            var registrations = await _repo.GetRegistrationsAsync(
                                        fromDate, toDate,
                                        filter.Location, filter.Preference);

            return new AdminViewModel
            {
                SelectedLocation = filter.Location,
                SelectedPreference = filter.Preference,
                SelectedDate = fromDate,
                WeekStart = fromDate,
                WeekEnd = toDate,
                Locations = locations,
                Registrations = registrations,
                TotalCount = registrations.Count,
                VegCount = registrations.Count(r => r.Preference == "Veg"),
                NonVegCount = registrations.Count(r => r.Preference == "NonVeg")
            };
        }

        public async Task<List<AdminRegistrationDto>> GetFilteredRegistrationsAsync(
            AdminFilterDto filter)
        {
            var (fromDate, toDate) = GetDateRange(filter);
            return await _repo.GetRegistrationsAsync(
                fromDate, toDate,
                filter.Location, filter.Preference);
        }

        private (DateTime from, DateTime to) GetDateRange(AdminFilterDto filter)
        {
            if (!DateTime.TryParse(filter.FromDate, out var fromDate))
                fromDate = DateTime.Today;

            if (!DateTime.TryParse(filter.ToDate, out var toDate))
                toDate = fromDate;

            // Make sure toDate is not before fromDate
            if (toDate < fromDate)
                toDate = fromDate;

            return (fromDate, toDate);
        }
    }
}
