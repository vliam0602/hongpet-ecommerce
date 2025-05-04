using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ProductDetailVM : BaseVM
    {
        public string Name { get; set; } = default!;
        public int CountOfReviews { get; set; } = 0; // num of reviews
        public int AverageStars { get; set; } = 0; // avg of stars
        public decimal Price { get; set; }
        public string? Brief { get; set; }       
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsActive { get; set; }
        public virtual IEnumerable<VariantVM> Variants { get; set; } 
            = new List<VariantVM>();
        public virtual IEnumerable<ImageVM> Images { get; set; } 
            = new List<ImageVM>();
        public virtual IEnumerable<ReviewGeneralVM> Reviews { get; set; } 
            = new List<ReviewGeneralVM>();

    }
}
