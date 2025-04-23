using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class OrderVM : BaseVM
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string CustomerPhone { get; set; } = default!;
        public string ShippingAddress { get; set; } = default!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = default!;
        public string PaymentMethod { get; set; } = default!;        
        public virtual ICollection<OrderItemVM> OrderItems { get; set; } = default!;
    }

    public class OrderItemVM
    {
        public Guid VariantId { get; set; }
        public string ProductName { get; set; } = default!;
        public IEnumerable<AttributeValueVM> AttributeValues { get; set; }
            = new List<AttributeValueVM>();
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

