using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions.Commons;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class ProductService : GenericService<Product>, IProductService
{
    public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _repository = _unitOfWork.ProductRepository;
    }

}
