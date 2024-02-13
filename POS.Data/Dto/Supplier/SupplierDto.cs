using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class SupplierDto
    {
        public int SNo { get; set; }
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsVarified { get; set; }
        public bool IsUnsubscribe { get; set; }
        public string SupplierProfile { get; set; }
        public string BusinessType { get; set; }
        public string ImageUrl { get; set; }
        public string Country { get; set; }
        public string SupplierNo { get; set; }
        public string SupplierType { get; set; }
        public string? PANNo { get; set; }
        public string? UdyogAadhar { get; set; }
        public string? MSME { get; set; }
        public string? GSTNo { get; set; }
        public string? Pin { get; set; }
        public Guid SupplierAddressId { get; set; }
        public SupplierAddressDto SupplierAddress { get; set; }
        public Guid? BillingAddressId { get; set; }
        public SupplierAddressDto BillingAddress { get; set; }
        public Guid? ShippingAddressId { get; set; }
        public SupplierAddressDto ShippingAddress { get; set; }
        public int? ProductCount { get; set; }
    }
}
