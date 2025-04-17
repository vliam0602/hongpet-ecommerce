using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IProductService : IGenericService<Product, ProductVM>
{
}
