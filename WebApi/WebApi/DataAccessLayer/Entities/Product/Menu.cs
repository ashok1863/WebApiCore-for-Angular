using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Entities.Product;

namespace WebApi.DataAccessLayer.Entities
{
    [Table("tblProductMenus")]
    public class Menu
    {
        public Menu()
        {
            this.Permissions = new HashSet<Permission>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Routing { get; set; }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string Variant { get; set; }

        public string Text { get; set; }

        public int? ParentId { get; set; }

        public bool IsDisplay { get; set; }

        public bool IsMandatory { get; set; }

        public int? SequenceNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime InsertedDate { get; set; }

        public string InsertedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }



        [ForeignKey("MenuId")]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
