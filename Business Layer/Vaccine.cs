using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business_Layer
{
    public class Vaccine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [ForeignKey(nameof(PetId))]
        public virtual Pet Pet { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } 

        [Required]
        public DateTime DateGiven { get; set; }

        public DateTime? NextDoseDate { get; set; }

        public bool IsOverdue()
        {
            if (NextDoseDate == null) return false;
            return DateTime.Today > NextDoseDate.Value.Date;
        }

        public int? DaysUntilNextDose()
        {
            if (NextDoseDate == null) return null;
            return (NextDoseDate.Value.Date - DateTime.Today).Days;
        }
    }
}
