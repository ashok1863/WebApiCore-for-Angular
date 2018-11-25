using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DataAccessLayer.Entities
{
    [Table("AspNetUsers")]
    public class AspnetUser:IdentityUser
    {
        public AspnetUser()
        {
            this.UserProfiles = new HashSet<UserProfile>();
        }

        [ForeignKey("UserId")]
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }


    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}