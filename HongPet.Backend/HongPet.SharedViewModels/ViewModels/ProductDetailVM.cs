using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ProductDetailVM : BaseVM
    {
        public string Name { get; set; } = default!;
        public int CountOfReviews { get; set; } = 0;
        public double Price { get; set; }
        public string? Brief { get; set; }       
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public virtual IEnumerable<VariantVM> Variants { get; set; } = new List<VariantVM>();
        public virtual IEnumerable<ImageVM> Images { get; set; } = new List<ImageVM>();
    }
}
