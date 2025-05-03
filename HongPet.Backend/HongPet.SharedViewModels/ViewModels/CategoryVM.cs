using System;

namespace HongPet.SharedViewModels.ViewModels
{
    public class CategoryVM : BaseVM
    {
        public string Name { get; set; } = default!;
        public Guid? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
    }
}
