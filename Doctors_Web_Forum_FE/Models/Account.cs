
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage ="Cannot be empty")]
        public string DisplayName { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Cannot be empty")]
        public string Email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Cannot be empty")]
        public string Password { get; set; }

        [StringLength(1000)]
        public string Avatar { get; set; }

        public DateTime? BirthDay { get; set; }

        public bool Gender { get; set; }

        public int? Status { get; set; }

        public string Role { get; set; }

        public bool Access_rights { get; set; }

        [Column(TypeName = "ntext")]
        public string About { get; set; }

        [StringLength(200)]
        public string Work_Place { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        [Column(TypeName = "ntext")]
        public string Experience { get; set; }

        [Column(TypeName = "ntext")]
        public string Achievement { get; set; }
        [ForeignKey("Qualification")]
        public int? QualificationId { get; set; }
        [ForeignKey("Specialization")]
        public int? SpecializationId { get; set; }

        [Column(TypeName = "ntext")]
        public string LinkFaceBook { get; set; }

        [Column(TypeName = "ntext")]
        public string LinkYoutube { get; set; }

        [Column(TypeName = "ntext")]
        public string LinkTiktok { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Qualification Qualification { get; set; }

        public Specialization Specialization { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments{ get; set; }
    }
}
