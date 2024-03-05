using System;

namespace POS.Data.Resources
{
    public class SalesOrderResource : ResourceParameter
    {
        public SalesOrderResource() : base("SOCreatedDate")
        {
        }

        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime? SOCreatedDate { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsSalesOrderRequest { get; set; } = false;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? ProductId { get; set; }
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.All;
        public string ProductName { get; set; }
        public bool? IsAppOrderRequest { get; set; }
        public bool? IsAdvanceOrderRequest { get; set; }
        public string CounterName { get; set; }
        public string PaymentType { get; set; }
        public string OrderDeliveryStatus { get; set; }
        public string MobileNo { get; set; }
        public string? ProductCategoryName { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public Guid? ProductMainCategoryId { get; set; }
        public string? BrandName { get; set; }

        public int? Month { get; set; }
        public int? Year { get; set; }
        public bool IsSalesOrderNotReturn { get; set; } = false;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.NA;
        public string CategoryType { get; set; }
    }
}

