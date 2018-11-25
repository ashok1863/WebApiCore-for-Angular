using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DataAccessLayer.Entities
{
    [Table("AspNetRoles")]
    public class Role
    {

        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
   
    }
}
