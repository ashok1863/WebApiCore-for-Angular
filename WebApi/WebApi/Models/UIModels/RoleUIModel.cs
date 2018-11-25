using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.UIModels
{
    public class RoleUIModel
    {
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

    }
}
