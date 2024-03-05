﻿using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class AddSalesOrderCommand : IRequest<ServiceResponse<SalesOrderDto>>
    {
        public string OrderNumber { get; set; }
        public string Note { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsSalesOrderRequest { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public SalesOrderStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public Guid DeliveryAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public List<SalesOrderItemDto> SalesOrderItems { get; set; }
        public bool? IsAppOrderRequest { get; set; }
        public bool? IsAdvanceOrderRequest { get; set; }
        public Guid ProductMainCategoryId { get; set; }
        public Guid? CounterId { get; set; }
        public string PaymentType { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        
    }
}
