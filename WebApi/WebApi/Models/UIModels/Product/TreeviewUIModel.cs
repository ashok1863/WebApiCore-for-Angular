using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.UIModels.Product
{
    public class TreeviewUIModel
    {

        public string text { get; set; }

        public int value { get; set; }

        public bool Checked {get;set;}

        public List<TreeviewUIModel> children { get; set; }
    }

    

}
