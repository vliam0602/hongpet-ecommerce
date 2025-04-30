using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class OrderCreationModel
    {
        public string CustomerName { get; set; } = default!;        
        public string CustomerPhone { get; set; } = default!;
        public string CustomerEmail { get; set; } = default!;
        public string ShippingAddress { get; set; } = default!;                
        public virtual IEnumerable<OrderItemCreationModel> OrderItems { get; set; } = new List<OrderItemCreationModel>();
        //public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        //public string Status { get; set; } = default!;
    }

    public class OrderItemCreationModel
    {
        public Guid VariantId { get; set; }        
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
