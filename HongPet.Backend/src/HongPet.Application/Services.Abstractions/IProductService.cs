using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IProductService : IGenericService<Product>
{
    Task<ProductDetailVM> AddProductAsync(ProductModel productModel);
    Task<IPagedList<ProductGeneralVM>> GetPagedProductAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "");
    Task<ProductDetailVM> GetProductDetailAsync(Guid id);
    Task SoftDeleteProductAsync(Guid id);
    Task<ProductDetailVM> UpdateProductAsync(Guid id, ProductModel productModel);
}
