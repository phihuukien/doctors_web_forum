using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Models
{
    [Table("Question")]
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = ("Cannot Be Empty"))]
        public string Title { get; set; }
        [Column(TypeName = "ntext")]
        public string Detail { get; set; }
        public int TotalQuestion{ get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Account Account { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public Topic Topic { get; set; }


    }
}
