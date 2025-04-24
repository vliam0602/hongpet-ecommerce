using AutoMapper;
using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class ProductService : GenericService<Product>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IClaimService claimService) : base(unitOfWork, claimService)
    {
        _productRepository = unitOfWork.ProductRepository;
        _repository = _productRepository; // for reuse the GenericService
        _mapper = mapper;
    }

    public async Task<IPagedList<ProductGeneralVM>> GetPagedProductAsync
        (int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        var pagedProducts = await base.GetPagedAsync(pageIndex, pageSize, keyword);
        return _mapper.Map<PagedList<ProductGeneralVM>>(pagedProducts);
    }

    public async Task<ProductDetailVM> GetProductDetailAsync(Guid id)
    {
        var product = await _productRepository.GetProductDetailAsync(id);

        if (product == null || product.DeletedDate != null)
            throw new KeyNotFoundException("Sản phẩm không tồn tại hoặc đã bị xóa.");

        return _mapper.Map<ProductDetailVM>(product);
    }

    public async Task<ProductDetailVM> AddProductAsync(ProductModel productModel)
    {
        var product = _mapper.Map<Product>(productModel);

        await base.AddAsync(product);
        return _mapper.Map<ProductDetailVM>(product);
    }

    public async Task<ProductDetailVM> UpdateProductAsync(Guid id, ProductModel productModel)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null || product.DeletedDate != null)
        {
            throw new KeyNotFoundException(
                $"Product with id {id} not found or has been deleted.");
        }

        _mapper.Map(productModel, product);
        await base.UpdateAsync(product);
        return _mapper.Map<ProductDetailVM>(product);
    }

    public async Task SoftDeleteProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null || product.DeletedDate != null)
        {
            throw new KeyNotFoundException(
                $"Product with id {id} not found or has been deleted.");
        }

        await base.SoftDeleteAsync(id);
    }
}
