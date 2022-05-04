using KUSYS.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS.DAL.Concrete.SQLExpress.Mapping
{
    public class StudentMap : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(e => e.StudentId);
            builder.Property(e => e.UserId);
            builder.Property(e => e.FirstName).HasMaxLength(55);
            builder.Property(e => e.LastName).HasMaxLength(55);
            builder.Property(e => e.BirthDate);

            builder.HasKey(e => e.StudentId);
            builder.HasKey(e => e.UserId);

            builder.HasOne(e => e.User)
                .WithOne(e => e.Student);

            builder.HasMany(e => e.StudentCourse)
                .WithOne(e => e.Student);
        }
    }

}
