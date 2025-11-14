using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business_Layer
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }

        public Owner()
        {
            Pets = new HashSet<Pet>();
        }
    }
}
