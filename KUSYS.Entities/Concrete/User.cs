using KUSYS.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace KUSYS.Entities.Concrete
{
    public class User : IEntity
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Role { get; set; }


        public virtual Student Student { get; set; }
    }

}
