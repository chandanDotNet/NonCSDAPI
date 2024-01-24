using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class SalesOrder : BaseEntity
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Note { get; set; }
        public string SaleReturnNote { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsSalesOrderRequest { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Not_Return;
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public Guid DeliveryAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<SalesOrderItem> SalesOrderItems { get; set; }
        public List<SalesOrderPayment> SalesOrderPayments { get; set; }
        //public List<CustomerAddress> DeliveryAddresses { get; set; }
        public bool? IsAppOrderRequest { get; set; }
        public bool? IsAdvanceOrderRequest { get; set; }
        public Guid? UserId { get; set; }
        public string OrderDeliveryStatus { get; set; }
        public User User { get; set; }
        public string PaymentType { get; set; }
        public Counter Counter { get; set; }
        public Guid? CounterId { get; set; }
        public SalesOrderReturnPaymentStatus? PaymentReturnStatus { get; set; }
        public string? UTRNo { get; set; }
        public DateTime? PaymentReturnDate { get; set; }
        public string? OfflineMode { get; set; }
        public string? CancelReason { get; set; }
        public string? StatusType { get; set; }
        public Guid? ProductMainCategoryId { get; set; }
    }
}
