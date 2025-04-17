using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions.Commons;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class ProductService : GenericService<Product, ProductVM>, IProductService
{
    public ProductService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper)
    {
        _repository = _unitOfWork.ProductRepository;
    }

}
