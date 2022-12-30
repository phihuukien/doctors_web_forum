using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Models
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public int Reply { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public bool Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }



        public Question Question { get; set; }

        public Account Account { get; set; }
    }
}
