using Autofac;
using KUSYS.Business.Abstract;
using KUSYS.Business.Concrete;
using KUSYS.DAL.Abstract;
using KUSYS.DAL.Concrete;
using KUSYS.DAL.Concrete.SQLExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentManager>().As<IStudentService>();
            builder.RegisterType<CourseManager>().As<ICourseService>();
            builder.RegisterType<StudentCourseManager>().As<IStudentCourseService>();
            builder.RegisterType<UserManager>().As<IUserService>();

            builder.RegisterType<EfStudentDal>().As<IStudentDal>();
            builder.RegisterType<EfCourseDal>().As<ICourseDal>();
            builder.RegisterType<EfStudentCourseDal>().As<IStudentCourseDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();
        }
    }
}
