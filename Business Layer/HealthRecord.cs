using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business_Layer
{
    public class HealthRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [ForeignKey(nameof(PetId))]
        public virtual Pet Pet { get; set; }

        [MaxLength(50)]
        public string RecordType { get; set; } 

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string FileUrl { get; set; } 
    }
}
