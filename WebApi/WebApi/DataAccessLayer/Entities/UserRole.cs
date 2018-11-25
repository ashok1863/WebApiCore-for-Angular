using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DataAccessLayer.Entities
{
    [Table("AspNetUserRoles")]
    public class UserRole
    {
        public UserRole()
        {
       
        }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual AspnetUser AspNetUsers { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Roles { get; set; }

    
    }
}
