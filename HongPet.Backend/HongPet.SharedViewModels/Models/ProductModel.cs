using System;
using System.Collections.Generic;

namespace HongPet.SharedViewModels.Models
{
    public class ProductModel
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Brief { get; set; }        
        public string? ThumbnailUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public List<VariantModel> Variants { get; set; } = new List<VariantModel>();
        public List<ProductImageModel> Images { get; set; } = new List<ProductImageModel>();
    }

    public class CategoryModel
    {
        public Guid Id { get; set; }
    }

    public class VariantModel
    {
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public List<AttributeValuePairModel> Attributes { get; set; } 
            = new List<AttributeValuePairModel>();
    }

    public class AttributeValuePairModel
    {
        public string Name { get; set; } = default!;
        public string Value { get; set; } = default!;
    }

    public class ProductImageModel
    {
        public string Name { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}