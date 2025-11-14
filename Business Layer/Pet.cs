using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace Business_Layer
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Species { get; set; } 

        [MaxLength(50)]
        public string Breed { get; set; }

        public DateTime? BirthDate { get; set; }

        public double? Weight { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual Owner Owner { get; set; }

        public virtual ICollection<VetVisit> VetVisits { get; set; }
        public virtual ICollection<Vaccine> Vaccines { get; set; }
        public virtual ICollection<Medication> Medications { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<HealthRecord> HealthRecords { get; set; }

        public Pet()
        {
            VetVisits = new HashSet<VetVisit>();
            Vaccines = new HashSet<Vaccine>();
            Medications = new HashSet<Medication>();
            Appointments = new HashSet<Appointment>();
            HealthRecords = new HashSet<HealthRecord>();
        }

        public int? GetAgeInYears()
        {
            if (BirthDate == null) return null;
            var today = DateTime.Today;
            int age = today.Year - BirthDate.Value.Year;
            if (BirthDate.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
