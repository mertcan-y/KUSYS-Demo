using KUSYS.Business.Abstract;
using KUSYS.DAL.Abstract;
using KUSYS.Entities.Concrete;

namespace KUSYS.Business.Concrete
{
    public class UserManager : BaseManager<User>, IUserService
    {
        public UserManager() : base()
        {
        }
    }
}
