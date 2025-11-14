using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business_Layer
{
    public class VetVisit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [ForeignKey(nameof(PetId))]
        public virtual Pet Pet { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(200)]
        public string Reason { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [MaxLength(100)]
        public string VetName { get; set; }

        public bool IsFollowUpNeeded()
        {
            if (DateTime.Today > Date.AddDays(14))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Reason))
                return false;

            string lower = Reason.ToLower();
            return lower.Contains("операция") || lower.Contains("surgery");
        }
    }
}
