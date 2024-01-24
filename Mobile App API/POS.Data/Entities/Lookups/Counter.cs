using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Counter : BaseEntity
    {
        public Guid Id { get; set; }
        public string CounterName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitutde { get; set; }
        public string? CounterNo { get; set; }
    }
}
