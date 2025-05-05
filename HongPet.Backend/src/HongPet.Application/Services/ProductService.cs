using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class ProductService : GenericService<Product>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IGenericRepository<ProductAttribute> _attributeRepository;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IClaimService claimService) : base(unitOfWork, claimService)
    {
        _productRepository = unitOfWork.ProductRepository;
        _repository = _productRepository; // for reuse the GenericService
        _mapper = mapper;
        _attributeRepository = unitOfWork.Repository<ProductAttribute>();
    }

    public async Task<IPagedList<ProductGeneralVM>> GetPagedProductAsync
        (int pageIndex = 1, int pageSize = 10, string? keyword = "",
        List<string>? category = null)
    {
        var pagedProducts = await _productRepository.GetPagedProductsAsync(
            pageIndex, pageSize, keyword, category);

        return _mapper.Map<PagedList<ProductGeneralVM>>(pagedProducts);
    }

    public async Task<ProductDetailVM> GetProductDetailAsync(Guid id)
    {
        var product = await _productRepository.GetProductDetailAsync(id);

        if (product == null || product.DeletedDate != null)
            throw new KeyNotFoundException("Sản phẩm không tồn tại hoặc đã bị xóa.");

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

    public async Task<Guid> AddProductAsync(ProductModel productModel)
    {
        var product = _mapper.Map<Product>(productModel);

        // Process categories
        product.Categories = await ProcessCategoriesAsync(productModel.Categories);

        // Process variants
        product.Variants = await ProcessVariantsAsync(productModel.Variants);

        // Process images
        ProcessImages(product.Images, product.Id);

        // Calculate price
        product.Price = CalculatePrice(product.Variants);

        await base.AddAsync(product);

        return product.Id;
    }


    public async Task<ProductDetailVM> UpdateProductAsync(Guid id, ProductModel productModel)
    {
        var product = await _productRepository.GetProductDetailAsync(id);
        if (product == null || product.DeletedDate != null)
        {
            throw new KeyNotFoundException(
                $"Product with id {id} not found or has been deleted.");
        }

        // Update basic information
        _mapper.Map(productModel, product);

        // Process categories
        product.Categories = await ProcessCategoriesAsync(productModel.Categories);

        // Process variants attribute
        product.Variants = await ProcessVariantsAsync(productModel.Variants);

        // Process images
        ProcessImages(product.Images, product.Id);

        // Calculate price
        product.Price = CalculatePrice(product.Variants);

        await base.UpdateAsync(product);

        return _mapper.Map<ProductDetailVM>(product);
    }

    public async Task<List<AttributeVM>> GetAllAttributes()
    {
        var attributes = await _attributeRepository.GetAllAsync();
        return _mapper.Map<List<AttributeVM>>(attributes);
    }

    #region private methods
    private async Task<List<Category>> ProcessCategoriesAsync(IEnumerable<CategoryModel> categoryModels)
    {
        var categoriesList = new List<Category>();
        foreach (var categoryModel in categoryModels)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryModel.Id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {categoryModel.Id} not found.");
            }
            categoriesList.Add(category);
        }
        return categoriesList;
    }

    private async Task<List<Variant>> ProcessVariantsAsync(IEnumerable<VariantModel> variantModels)
    {
        var variantsList = new List<Variant>();
        foreach (var variantModel in variantModels)
        {
            var attributeValuesList = new List<ProductAttributeValue>();
            foreach (var attributeValueModel in variantModel.Attributes)
            {
                var attributeValue = await _productRepository.GetAttributeValuePairAsync(
                    attributeValueModel.Name, attributeValueModel.Value);

                if (attributeValue == null)
                {
                    var attribute = await _productRepository.GetAttributeByNameAsync(attributeValueModel.Name) ?? new ProductAttribute
                    {
                        Name = attributeValueModel.Name
                    };
                    attributeValue = new ProductAttributeValue
                    {
                        AttributeId = attribute.Id,
                        Attribute = attribute,
                        Value = attributeValueModel.Value
                    };
                }
                attributeValuesList.Add(attributeValue);
            }
            variantsList.Add(new Variant
            {
                Price = variantModel.Price,
                Stock = variantModel.Stock,
                IsActive = variantModel.IsActive,
                AttributeValues = attributeValuesList
            });
        }
        return variantsList;
    }

    private void ProcessImages(IEnumerable<Image> imageModels, Guid productId)
    {
        foreach (var image in imageModels)
        {
            image.ProductId = productId;
        }
    }

    private decimal CalculatePrice(IEnumerable<Variant> variants)
    {
        return variants.Any() ? variants.Min(v => v.Price) : 0;
    }

    #endregion

}
