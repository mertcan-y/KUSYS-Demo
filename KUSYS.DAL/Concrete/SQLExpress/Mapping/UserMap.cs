using KUSYS.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KUSYS.DAL.Concrete.SQLExpress.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.UserId);
            builder.Property(e => e.UserName).HasMaxLength(55);
            builder.Property(e => e.PasswordHash);
            builder.Property(e => e.PasswordSalt);
            builder.Property(e => e.Role).HasMaxLength(55);
            
            builder.HasKey(e=>e.UserId);

            builder.HasOne(e => e.Student)
                .WithOne(e => e.User);
        }
    }

}
