using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ProductGeneralVM : BaseVM
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int TotalVariants { get; set; } = 0;
        public bool IsActive { get; set; }
        public virtual IEnumerable<CategoryVM> Categories { get; set; } 
            = new List<CategoryVM>();        
    }
}
