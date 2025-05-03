using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface ICategoryService : IGenericService<Category>
{
    Task<IPagedList<CategoryVM>> GetPagedCategoriesAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "");
    Task<CategoryVM> UpdateCategoryAsync(Guid id, CategoryCreateModel categoryModel);
}
