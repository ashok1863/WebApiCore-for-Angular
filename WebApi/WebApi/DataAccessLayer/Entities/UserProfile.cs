using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DataAccessLayer.Entities
{
    [Table("AspNetUserProfile")]
    public class UserProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public DateTime InsertedDate { get; set; }

        public string InsertedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey("UserId")]
        public virtual AspnetUser AspNetUsers { get; set; }

        //[ForeignKey("UserId")]
        //public virtual UserRole UserRole { get; set; }
    }
}
