using KUSYS.Business.Abstract;
using KUSYS.DAL.Abstract;
using KUSYS.Entities.Concrete;

namespace KUSYS.Business.Concrete
{
    public class StudentManager : BaseManager<Student>, IStudentService
    {
        public StudentManager() : base()
        {
        }
    }
}
