namespace MealLedger.DTO
{
    public class AdminViewModel
    {
        public string SelectedLocation { get; set; } = "All";
        public string SelectedPreference { get; set; } = "All";
        public string FilterType { get; set; } = "Daily";        // "Daily" or "Weekly"
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public List<string> Locations { get; set; } = new();
        public List<AdminRegistrationDto> Registrations { get; set; } = new();
        public int TotalCount { get; set; }
        public int VegCount { get; set; }
        public int NonVegCount { get; set; }
    }

    public class AdminRegistrationDto
    {
        public string WorkdayID { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Preference { get; set; }
        public DateTime LunchDate { get; set; }
    }

    public class AdminFilterDto
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Location { get; set; } = "All";
        public string Preference { get; set; } = "All";

    }
}
