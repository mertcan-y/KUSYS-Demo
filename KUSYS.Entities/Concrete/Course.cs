using KUSYS.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace KUSYS.Entities.Concrete
{
    public class Course : IEntity
    {
        public Course()
        {
            StudentCourse = new HashSet<StudentCourse>();
        }

        [Key]
        public string CourseId { get; set; }
        [Required]
        public string CourseName { get; set; }

        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }

}
