using System.ComponentModel.DataAnnotations;

namespace MealLedger.Models
{
    public class Holiday
    {
        [Key]
        public int HolidayID { get; set; }

        [Required]
        public DateTime HolidayDate { get; set; }

        [Required]
        [StringLength(100)]
        public string HolidayName { get; set; }

        [Required]
        [StringLength(10)]
        public string Type { get; set; }  // "Company" or "Optional"

        [Required]
        [StringLength(100)]
        public string Location { get; set; }
    }
}
