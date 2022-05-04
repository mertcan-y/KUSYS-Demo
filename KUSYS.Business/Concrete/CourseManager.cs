using KUSYS.Business.Abstract;
using KUSYS.DAL.Abstract;
using KUSYS.Entities.Concrete;

namespace KUSYS.Business.Concrete
{
    public class CourseManager : BaseManager<Course>, ICourseService
    {
        public CourseManager() : base()
        {
        }
    }
}
