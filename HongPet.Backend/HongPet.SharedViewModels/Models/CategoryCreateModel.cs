using System;

namespace HongPet.SharedViewModels.Models
{
    public class CategoryCreateModel
    {

        public string Name { get; set; } = default!;
        public Guid? ParentCategoryId { get; set; }
    }
}
