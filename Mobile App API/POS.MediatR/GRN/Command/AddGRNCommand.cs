﻿using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using POS.Helper;
using POS.Data.Dto.GRN;

namespace POS.MediatR.CommandAndQuery
{
    public class AddGRNCommand : IRequest<ServiceResponse<GRNDto>>
    {
        public Guid PurchaseOrderId { get; set; }
        public string PONumber { get; set; }
        public string GRNNumber { get; set; }
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
        public List<GRNItemDto> GRNItems { get; set; }
    }
}
