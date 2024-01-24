﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class WishlistDto
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ProductId { get; set; }
        public ProductDto Product { get; set; }

    }
}
