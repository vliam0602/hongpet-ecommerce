using AutoMapper;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>));

        CreateMap<Product, ProductVM>().ReverseMap();
        CreateMap<Variant, VariantVM>().ReverseMap();
        CreateMap<ProductAttributeValue, ProductAttributeValueVM>().ReverseMap();
        CreateMap<ProductAttribute, ProductAttributeVM>().ReverseMap();
    }
}
