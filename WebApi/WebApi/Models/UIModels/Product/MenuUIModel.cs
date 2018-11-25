using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.UIModels.Product
{
    public class MenuUIModel
    {

        public MenuUIModel()
        {
           // this.children = new List<MenuUIModel>();
        }

        public string name { get; set; }

        public string url { get; set; }

        public string icon { get; set; }

        public badgeUIModel badge { get; set; }

        public List<MenuUIModel> children { get; set; }
    }

    public class badgeUIModel
    {
        public string variant { get; set; }

        public string text { get; set; }
    }

    public class PermissionUIModel
    {
        public List<int> SelectedIds { get; set; }
    }
}
