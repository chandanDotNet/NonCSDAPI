using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class CustomerAddress : BaseEntity
    {
        public Guid Id { get; set; }
        public string HouseNo { get; set; }
        public string StreetDetails { get; set; }
        public string LandMark { get; set; }
        public string Type { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public bool IsPrimary { get; set; }
        public string Latitude { get; set; }
        public string Longitutde { get; set; }
        public string? Pincode { get; set; }
    }
}
