using Domain.Entities.Commons;
using HongPet.Domain.Entities;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetProductDetailAsync(Guid id);
    Task<IPagedList<Product>> GetProductsByCategoryAsync(string categoryName, int pageIndex = 1, int pageSize = 10, string? keyword = "");    
}
