using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class VariantVM
    {
        public Guid Id { get; set; }
        public IEnumerable<AttributeValueVM> AttributeValues { get; set; }
            = new List<AttributeValueVM>();
        public double Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
    }

    public class AttributeValueVM
    {
        public Guid Id { get; set; }        
        public string Attribute { get; set; } = default!; // reference to Attribute.Name
        public string Value { get; set; } = default!;
    }
}
