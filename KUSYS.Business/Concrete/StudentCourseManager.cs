using KUSYS.Business.Abstract;
using KUSYS.DAL.Abstract;
using KUSYS.Entities.Concrete;

namespace KUSYS.Business.Concrete
{
    public class StudentCourseManager : BaseManager<StudentCourse>, IStudentCourseService
    {
        public StudentCourseManager() : base()
        {
        }
    }
}
