using KUSYS.Core.DAL.SQLExpress;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete.SQLExpress.Context;
using KUSYS.Entities.Concrete;

namespace KUSYS.DAL.Concrete.SQLExpress
{
    public class EfCourseDal : EfEntityRepositoryBase<Course, KUSYSContext>, ICourseDal
    {
        public EfCourseDal(KUSYSContext context) : base(context)
        {
        }
    }
}
