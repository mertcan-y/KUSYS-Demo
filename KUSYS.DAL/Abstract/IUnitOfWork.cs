using KUSYS.Core.DAL;
using KUSYS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.DAL.Abstract
{
    public interface IUnitOfWork<T> where T : class, IEntity, new()
    {
        IStudentDal Student { get; }
        ICourseDal Course { get; }
        IStudentCourseDal StudentCourse { get; }
        IUserDal User { get; }
        IEntityRepository<T> Generic { get; }

        Task<int> SaveAsync();
    }
}
