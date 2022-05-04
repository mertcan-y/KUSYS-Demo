using KUSYS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Entities.Concrete
{
    public class StudentCourse : IEntity
    {
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        [ForeignKey("Course")]
        public string CourseId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
    }
}
