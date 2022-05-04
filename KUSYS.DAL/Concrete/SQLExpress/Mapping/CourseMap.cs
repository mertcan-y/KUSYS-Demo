using KUSYS.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KUSYS.DAL.Concrete.SQLExpress.Mapping
{
    public class CourseMap : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(e => e.CourseId);
            builder.Property(e => e.CourseName);
            
            builder.HasKey(e => e.CourseId);

            builder.HasMany(e=>e.StudentCourse)
                .WithOne(e=>e.Course);
        }
    }

}
