using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class ReviewVM : BaseVM
    {
        public int Rating { get; set; }
        public string Title { get; set; } = default!;
        public string Comment { get; set; } = default!;
        public string ReviewerName { get; set; } = default!;
        public string? ReviewerAvatar { get; set; }
    }

    public class ReviewGeneralVM
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
    }
}
