using KUSYS.Core.DAL;
using KUSYS.Core.DAL.SQLExpress;
using KUSYS.Core.Entities;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete.SQLExpress;
using KUSYS.DAL.Concrete.SQLExpress.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.DAL.Concrete
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class, IEntity, new()
    {
        private EfStudentDal _efStudentDal;
        private EfCourseDal _efCourseDal;
        private EfStudentCourseDal _efStudentCourseDal;
        private EfUserDal _efUserDal;
        private EfEntityRepositoryBase<T, KUSYSContext> _genericDal;

        private KUSYSContext _context;

        public UnitOfWork()
        {
            _context = new KUSYSContext();
        }

        public IStudentDal Student => _efStudentDal ?? new EfStudentDal(_context);
        public ICourseDal Course => _efCourseDal ?? new EfCourseDal(_context);
        public IStudentCourseDal StudentCourse => _efStudentCourseDal ?? new EfStudentCourseDal(_context);
        public IUserDal User => _efUserDal ?? new EfUserDal(_context);
        public IEntityRepository<T> Generic => _genericDal ?? new EfEntityRepositoryBase<T, KUSYSContext>(_context);

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
