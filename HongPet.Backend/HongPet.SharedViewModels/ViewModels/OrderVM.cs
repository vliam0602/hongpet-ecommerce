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
        public virtual IEnumerable<OrderItemVM> OrderItems { get; set; } 
            = new List<OrderItemVM>();
    }    
}

