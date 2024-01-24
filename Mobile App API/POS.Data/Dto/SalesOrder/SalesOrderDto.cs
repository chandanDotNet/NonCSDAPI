using System;
using System.Collections.Generic;


namespace POS.Data.Dto
{
    public class SalesOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Note { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsSalesOrderRequest { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public SalesOrderStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public Guid DeliveryAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<SalesOrderItemDto> SalesOrderItems { get; set; }
        public List<SalesOrderPaymentDto> SalesOrderPayments { get; set; }
       // public List<CustomerAddressDto> DeliveryAddresses { get; set; }
        public CustomerDto Customer { get; set; }
        public bool? IsAppOrderRequest { get; set; }
        public bool? IsAdvanceOrderRequest { get; set; }
        public string OrderDeliveryStatus { get; set; }
        public string AssignDeliveryPerson { get; set; }
        public Guid? AssignDeliveryPersonId { get; set; }
        public string CounterName { get; set; }
        public decimal Quantity { get; set; }
        public string PaymentMethod { get; set; }
        public string BillNo { get; set; }
        public string PaymentType { get; set; }
        public Guid? CounterId { get; set; }
        public SalesOrderReturnPaymentStatus? PaymentReturnStatus { get; set; }
        public string? UTRNo { get; set; }
        public DateTime? PaymentReturnDate { get; set; }
        public string? OfflineMode { get; set; }
        public string? MobileNo { get; set; }
        public string? CancelReason { get; set; }
        public string? StatusType { get; set; }
        public Guid? ProductMainCategoryId { get; set; }
    }
}
