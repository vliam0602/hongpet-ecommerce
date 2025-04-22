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

        CreateMap<Product, ProductGeneralVM>();

        CreateMap<Product, ProductDetailVM>()
            .ForMember(model => model.CountOfReviews, 
                opt => opt.MapFrom(x => x.Reviews.Count()));

        CreateMap<Variant, VariantVM>();

        CreateMap<ProductAttributeValue, AttributeValueVM>()
            .ForMember(model => model.AttributeName, 
                opt => opt.MapFrom(x => x.Attribute.Name));
    }
}
