using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business_Layer
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [ForeignKey(nameof(PetId))]
        public virtual Pet Pet { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        [MaxLength(200)]
        public string Reason { get; set; }

        public bool IsUpcoming()
        {
            return DateTime > System.DateTime.Now;
        }
    }
}
