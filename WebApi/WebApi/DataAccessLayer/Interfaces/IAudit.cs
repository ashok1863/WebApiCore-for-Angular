using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DataAccessLayer.Interfaces
{
    interface IAudit
    {
         bool IsActive { get; set; }

         DateTime InsertedDate { get; set; }

         string InsertedBy { get; set; }

         DateTime? UpdatedDate { get; set; }

         string UpdatedBy { get; set; }
    }
}
