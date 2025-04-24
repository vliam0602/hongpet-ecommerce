using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class ReviewCreateModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; } = 1;
        public string Title { get; set; } = default!;
        public string Comment { get; set; } = default!;
    }

    public class ReviewUpdateModel
    {
        public int Rating { get; set; } = 1;
        public string Title { get; set; } = default!;
        public string Comment { get; set; } = default!;
    }
}
