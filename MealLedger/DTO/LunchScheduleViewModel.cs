namespace MealLedger.DTO
{
    public class LunchScheduleViewModel
    {
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        // Stores date + preference for already registered days
        public List<RegisteredDayDto> RegisteredDays { get; set; } = new();
        public List<HolidayDto> Holidays { get; set; } = new();
    }
    public class RegisteredDayDto
    {
        public string Date { get; set; }        // "yyyy-MM-dd"
        public string Preference { get; set; }  // "Veg" or "NonVeg"
    }

    public class HolidayDto
    {
        public string Date { get; set; }        // "yyyy-MM-dd"
        public string Type { get; set; }        // "Company" or "Optional"
        public string Name { get; set; }
    }
}
