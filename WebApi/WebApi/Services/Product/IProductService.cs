using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.UIModels.Product;

namespace WebApi.Services.Product
{
   public  interface IProductService
    {

        List<string> GetPermissions(string userid);

        List<MenuUIModel> GetUserMenus(string userid);

        List<TreeviewUIModel> GetMenus(string roleid);

         Task<int> UpdatePermissionsAsync(string roleid, PermissionUIModel request);
    }
}
