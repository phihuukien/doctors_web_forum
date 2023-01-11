using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Models
{
    [Table("Topic")]
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Cannot be empty")]
        public string TopicName { get; set; }
        public bool Status { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
