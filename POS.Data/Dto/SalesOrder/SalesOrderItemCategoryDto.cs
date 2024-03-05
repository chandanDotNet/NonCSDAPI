using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto.SalesOrder
{
    public class SalesOrderItemCategoryDto
    {
        public int SlNo { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public decimal Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Rate { get; set; }
        public string TotalPurchasePrice { get; set; }
        public decimal TotalSalesPrice { get; set; }
    }
}
