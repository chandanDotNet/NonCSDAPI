﻿using System;

namespace POS.Data.Dto
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public string ProductCategoryUrl { get; set; }
        public Guid? ProductMainCategoryId { get; set; }
        public string ProductMainCategoryName { get; set; }
    }
}
