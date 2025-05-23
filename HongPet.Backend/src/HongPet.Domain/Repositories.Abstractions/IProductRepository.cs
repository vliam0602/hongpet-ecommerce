﻿using HongPet.Domain.Entities;
using HongPet.Domain.Entities.Commons;
using HongPet.SharedViewModels.Generals;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IProductRepository : IGenericRepository<Product>
{
    Task<ProductAttribute?> GetAttributeByNameAsync(string attributeName);
    Task<ProductAttributeValue?> GetAttributeValuePairAsync(string attributeName, string attributeValue);
    Task<Product?> GetProductDetailAsync(Guid id);
    Task<IPagedList<Product>> GetProductsByCategoryAsync(string categoryName, 
        int pageIndex = 1, int pageSize = 10, string? keyword = "");
    public Task<IPagedList<Product>> GetPagedProductsAsync
        (int pageIndex = 1, int pageSize = 10, string? keyword = "",
        List<string>? category = null);
}
