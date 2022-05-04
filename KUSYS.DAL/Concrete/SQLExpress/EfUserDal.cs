using KUSYS.Core.DAL.SQLExpress;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete.SQLExpress.Context;
using KUSYS.Entities.Concrete;

namespace KUSYS.DAL.Concrete.SQLExpress
{
    public class EfUserDal : EfEntityRepositoryBase<User, KUSYSContext>, IUserDal
    {
        public EfUserDal(KUSYSContext context) : base(context)
        {
        }
    }
}
