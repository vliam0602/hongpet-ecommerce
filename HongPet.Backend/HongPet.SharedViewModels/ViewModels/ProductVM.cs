
using System;
using System.Collections.Generic;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ProductVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<VariantVM> Variants { get; set; } = new List<VariantVM>();
    }

    public class VariantVM
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductAttributeValueVM> AttributeValues { get; set; } = new List<ProductAttributeValueVM>();
    }

    public class ProductAttributeValueVM
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = default!;
        public ProductAttributeVM Attribute { get; set; } = default!;
    }

    public class ProductAttributeVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
