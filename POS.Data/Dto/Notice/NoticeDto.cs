﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class NoticeDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string AlertMessage { get; set; }
        public bool StoreOpenClose { get; set; }
    }
}
