using System.ComponentModel.DataAnnotations;

namespace MealLedger.Models
{
    public class Employee
    {
        [Key]
        [StringLength(20)]
        public string WorkdayID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [StringLength(10)]
        public string Role { get; set; }  // "Admin" or "Employee"

        public bool IsActive { get; set; } = true;

        // Navigation Property
        public ICollection<LunchRegistration> LunchRegistrations { get; set; }
    }
}
