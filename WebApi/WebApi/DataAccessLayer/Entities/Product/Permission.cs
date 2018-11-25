using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DataAccessLayer.Entities.Product
{
    [Table("tblProductPermissions")]
    public class Permission
    {

        [Key]
        public int Id { get; set; }

        public int MenuId { get; set; }

        public string RoleId { get; set; }
        public bool IsActive { get; set; }

        public DateTime InsertedDate { get; set; }

        public string InsertedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey("MenuId")]
        public virtual Menu Menus { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Roles { get; set; }
    }
}
