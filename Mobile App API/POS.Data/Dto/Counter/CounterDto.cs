﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class CounterDto
    {
        public Guid Id { get; set; }
        public string CounterName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitutde { get; set; }
        public string? CounterNo { get; set; }
    }
}
