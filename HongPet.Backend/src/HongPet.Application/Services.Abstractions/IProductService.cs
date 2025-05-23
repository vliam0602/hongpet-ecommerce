﻿using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IProductService : IGenericService<Product>
{
    Task<Guid> AddProductAsync(ProductModel productModel);
    Task<List<AttributeVM>> GetAllAttributes();
    Task<IPagedList<ProductGeneralVM>> GetPagedProductAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "",
        List<string>? category = null);
    Task<ProductDetailVM> GetProductDetailAsync(Guid id);
    Task SoftDeleteProductAsync(Guid id);
    Task<ProductDetailVM> UpdateProductAsync(Guid id, ProductModel productModel);
}
