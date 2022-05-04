using KUSYS.Core.DAL.SQLExpress;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete.SQLExpress.Context;
using KUSYS.Entities.Concrete;

namespace KUSYS.DAL.Concrete.SQLExpress
{
    public class EfStudentCourseDal : EfEntityRepositoryBase<StudentCourse, KUSYSContext>, IStudentCourseDal
    {
        public EfStudentCourseDal(KUSYSContext context) : base(context)
        {
        }
    }
}
