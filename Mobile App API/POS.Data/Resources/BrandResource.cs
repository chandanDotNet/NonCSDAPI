using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class BrandResource : ResourceParameters
    {
        public BrandResource() : base("Name")
        {

        }
        public Guid? Id { get; set; }
        //public string Name { get; set; }
        public string SearchQuery { get; set; }
        public Guid? ProductMainCategoryId { get; set; }
    }
}
