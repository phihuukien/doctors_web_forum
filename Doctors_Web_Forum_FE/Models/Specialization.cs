using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Models
{
    [Table("Specialization")]
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Cannot be empty")]
        public string SpecializationName { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
