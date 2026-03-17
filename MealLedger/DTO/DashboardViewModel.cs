using MealLedger.Models;

namespace MealLedger.DTO
{
    public class DashboardViewModel
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Location { get; set; }
        public DateTime Today { get; set; }

        // Employee specific
        public bool IsTodayRegistered { get; set; }
        public List<LunchRegistration> ThisWeekRegistrations { get; set; }

        // Admin specific
        public int TotalRegisteredToday { get; set; }
        public int VegCountToday { get; set; }
        public int NonVegCountToday { get; set; }
    }
}
