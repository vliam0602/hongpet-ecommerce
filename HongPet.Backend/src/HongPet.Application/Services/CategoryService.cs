using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class CategoryService : GenericService<Category>, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoryService(
        IUnitOfWork unitOfWork, 
        IClaimService claimService,
        IMapper mapper) : base(unitOfWork, claimService)
    {
        _mapper = mapper;
        _categoryRepository = unitOfWork.CategoryRepository;
        _repository = _categoryRepository; // for reuse the generic service methods
    }

    public async Task<IPagedList<CategoryVM>> GetPagedCategoriesAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        var categories =  await _categoryRepository.GetPagedAsync(
            pageIndex, pageSize, keyword);
        return _mapper.Map<PagedList<CategoryVM>>(categories);
    }

    public async Task<CategoryVM> UpdateCategoryAsync(Guid id, 
        CategoryCreateModel categoryModel)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null || category.DeletedDate != null)
        {
            throw new KeyNotFoundException(
                $"Category with id {id} not found or has been deleted.");
        }

        _mapper.Map(categoryModel, category);

        await base.UpdateAsync(category);

        return _mapper.Map<CategoryVM>(category);
    }


}
