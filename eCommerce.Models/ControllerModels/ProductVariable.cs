﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class ProductVariable
    {
        public int Index { get; set; }
        public SizeDetail SizeDetail { get; set; } = new SizeDetail();
        public ColorDetail ColorDetail { get; set; } = new ColorDetail();
        public List<string> ImageUrls { get; set; } = new List<string>();
        public int Price { get; set; }
        public int Discount { get; set; }
        public int Inventory { get; set; }
        public bool ImageShow { get; set; }

    }
}
