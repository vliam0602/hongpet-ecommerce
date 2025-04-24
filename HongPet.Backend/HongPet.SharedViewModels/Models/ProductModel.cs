using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class ProductModel
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Brief { get; set; }
        public double Price { get; set; } // product price is min price of variants
        public string? ThumbnailUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
