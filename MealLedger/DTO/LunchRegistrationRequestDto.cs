namespace MealLedger.DTO
{
    public class LunchRegistrationRequestDto
    {
        public List<DayRegistrationDto> Days { get; set; } = new();
    }
    public class DayRegistrationDto
    {
        public string Date { get; set; }        // "yyyy-MM-dd"
        public string Preference { get; set; }  // "Veg" or "NonVeg"
    }
}
