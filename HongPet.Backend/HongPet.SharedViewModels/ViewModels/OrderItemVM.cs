using System;
using System.Collections.Generic;

namespace HongPet.SharedViewModels.ViewModels
{
    public class OrderItemVM
    {
        public Guid ProductId { get; set; }
        public Guid VariantId { get; set; }
        public string? ProductName { get; set; }
        public string? ThumbnailImageUrl { get; set; }
        public IEnumerable<AttributeValueVM> AttributeValues { get; set; }
            = new List<AttributeValueVM>();
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
