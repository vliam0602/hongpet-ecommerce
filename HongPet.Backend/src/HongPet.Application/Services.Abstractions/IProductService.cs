using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IProductService : IGenericService<Product>
{
    Task<IPagedList<ProductGeneralVM>> GetPagedProductAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "");
    Task<ProductDetailVM> GetProductDetailAsync(Guid id);
    Task<IPagedList<ReviewVM>> GetProductReviewsAsync(Guid productId, 
        int pageIndex = 1, int pageSize = 10);
}
