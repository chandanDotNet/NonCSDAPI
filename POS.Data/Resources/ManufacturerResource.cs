using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class ManufacturerResource : ResourceParameters
    {
        public ManufacturerResource() : base("ManufacturerName")
        {

        }
        public Guid? Id { get; set; }
        //public string ManufacturerName { get; set; }
        public string SearchQuery { get; set; }
    }
}