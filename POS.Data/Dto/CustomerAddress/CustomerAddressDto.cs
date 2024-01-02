using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class CustomerAddressDto
    {
        public Guid Id { get; set; }
        public string HouseNo { get; set; }
        public string StreetDetails { get; set; }
        public string LandMark { get; set; }
        public string Type { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string CustomerName { get; set; }
        public bool IsPrimary { get; set; }
        public string Latitude { get; set; }
        public string Longitutde { get; set; }
        public string? Pincode { get; set; }
    }
}
