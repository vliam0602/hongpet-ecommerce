using AutoMapper;
using HongPet.Domain.Entities;
using HongPet.Infrastructure.DTOs;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Infrastructure;
public class MappingDto : Profile
{
    public MappingDto()
    {
        CreateMap<Order, OrderDto>();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(model => model.ProductId,
                opt => opt.MapFrom(x => x.Variant.ProductId))
            .ForMember(model => model.ProductName,
                opt => opt.MapFrom(x => x.Variant.Product.Name))
            .ForMember(model => model.ThumbnailImageUrl,
                opt => opt.MapFrom(x => x.Variant.Product.ThumbnailUrl))
            .ForMember(model => model.AttributeValues,
                opt => opt.MapFrom(x => x.Variant.AttributeValues));

        CreateMap<ProductAttributeValue, AttributeValueDto>()
            .ForMember(model => model.Attribute,
                opt => opt.MapFrom(x => x.Attribute.Name));
    }
}
