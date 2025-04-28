using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ImageVM
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string ImageUrl { get; set; } = default!;
    }
}
