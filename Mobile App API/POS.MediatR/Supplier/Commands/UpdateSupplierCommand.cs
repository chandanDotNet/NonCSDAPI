﻿using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateSupplierCommand : IRequest<ServiceResponse<SupplierDto>>
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public bool? IsVarified { get; set; }
        public bool? IsUnsubscribe { get; set; }
        public bool IsImageUpload { get; set; }
        public string SupplierProfile { get; set; }
        public string Email { get; set; }
        public string SupplierNo { get; set; }
        public string SupplierType { get; set; }
        public string? PANNo { get; set; }
        public string? UdyogAadhar { get; set; }
        public string? MSME { get; set; }
        public string? GSTNo { get; set; }
        public string? Pin { get; set; }
        public Guid SupplierAddressId { get; set; }
        public SupplierAddressDto SupplierAddress { get; set; }
        public Guid BillingAddressId { get; set; }
        public SupplierAddressDto BillingAddress { get; set; }
        public Guid ShippingAddressId { get; set; }
        public SupplierAddressDto ShippingAddress { get; set; }
    }
}
