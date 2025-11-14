using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business_Layer
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [ForeignKey(nameof(PetId))]
        public virtual Pet Pet { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Dosage { get; set; } 

        public int DurationDays { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate
        {
            get { return StartDate.AddDays(DurationDays); }
            set { }
        }

        public bool IsActive()
        {
            var today = DateTime.Today;
            return today >= StartDate.Date && today <= EndDate.Date;
        }

        public int RemainingDays()
        {
            var today = DateTime.Today;
            if (today > EndDate.Date) return 0;
            return (EndDate.Date - today).Days;
        }
    }
}
