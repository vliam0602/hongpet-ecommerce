using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ProductGeneralVM : BaseVM
    {
        public string Name { get; set; } = default!;
        public double Price { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
