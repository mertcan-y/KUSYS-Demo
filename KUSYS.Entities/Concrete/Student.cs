using KUSYS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Entities.Concrete
{
    public class Student : IEntity
    {
        public Student()
        {
            StudentCourse = new HashSet<StudentCourse>();
        }

        [Key]
        public string StudentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<StudentCourse> StudentCourse { get; set; }

    }

}
