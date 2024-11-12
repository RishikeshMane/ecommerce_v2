﻿using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        public int SizeLinkId { get; set; }
        public string SizeCode { get; set; }
        public string Description { get; set; }
    }
}
