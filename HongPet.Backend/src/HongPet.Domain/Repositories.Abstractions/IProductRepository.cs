using HongPet.Domain.Entities;
using HongPet.Domain.Entities.Commons;
using HongPet.SharedViewModels.Generals;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetProductDetailAsync(Guid id);
    Task<IPagedList<Product>> GetProductsByCategoryAsync(string categoryName, int pageIndex = 1, int pageSize = 10, string? keyword = "");    
}
