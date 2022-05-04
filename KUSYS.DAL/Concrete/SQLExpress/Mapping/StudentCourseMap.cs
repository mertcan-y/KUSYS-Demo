using KUSYS.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KUSYS.DAL.Concrete.SQLExpress.Mapping
{
    public class StudentCourseMap : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.Property(e => e.CourseId);
            builder.Property(e => e.StudentId);

            builder.HasKey(e => e.StudentId);
            builder.HasKey(e => e.CourseId);

            builder.HasOne(e => e.Student)
                .WithMany(e => e.StudentCourse)
                .HasForeignKey(e=>e.StudentId);

            builder.HasOne(e => e.Course)
                .WithMany(e => e.StudentCourse)
                .HasForeignKey(e => e.CourseId);
        }
    }

}
