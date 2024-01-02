﻿using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;

namespace POS.MediatR.PurchaseOrder.Commands
{
    public class UpdatePurchaseOrderReturnCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Note { get; set; }
        public string TermAndCondition { get; set; }
        public bool IsPurchaseOrderRequest { get; set; }
        public DateTime POCreatedDate { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public List<PurchaseOrderItemDto> PurchaseOrderItems { get; set; }
        public string? PurchaseOrderPaymentStatus { get; set; }
        public string? PurchaseOrderReturnType { get; set; }
    }
}

