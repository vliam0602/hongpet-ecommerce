using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.CustomerMVC.Services.Abstraction;

public interface IProductApiService
{
    Task<PagedList<ProductGeneralVM>> GetProductsAsync(
        QueryListCriteria criteria);
    Task<ProductDetailVM?> GetProductByIdAsync(Guid id);
    Task<PagedList<ReviewVM>> GetProductReviewsAsync(
        Guid productId, int pageIndex, int pageSize);
}
