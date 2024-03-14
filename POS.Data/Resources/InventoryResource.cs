using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class InventoryResource : ResourceParameters
    {
        public InventoryResource() : base("ProductName")
        {

        }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string SupplierName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCode { get; set; }
        public DateTime? DefaultDate { get; set; } = DateTime.Now;
        public Guid? ProductMainCategoryId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public Guid? SupplierId { get; set; }
        public bool? IsNegativeStock { get; set; }
    }
}
