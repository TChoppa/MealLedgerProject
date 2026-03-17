using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealLedger.Models
{
    public class LunchRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required]
        [StringLength(20)]
        public string WorkdayID { get; set; }


        [Required]
        [StringLength(20)]
        public string Preference { get; set; }

        [Required]
        public DateTime LunchDate { get; set; }

        public DateTime RegisteredOn { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("WorkdayID")]
        public Employee Employee { get; set; }
    }
}
