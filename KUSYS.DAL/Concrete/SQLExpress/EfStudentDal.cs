using KUSYS.Core.DAL.SQLExpress;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete.SQLExpress.Context;
using KUSYS.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.DAL.Concrete.SQLExpress
{
    public class EfStudentDal : EfEntityRepositoryBase<Student, KUSYSContext>, IStudentDal
    {
        public EfStudentDal(KUSYSContext context) : base(context)
        {
        }
    }
}
