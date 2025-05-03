using AutoMapper;
using HongPet.Domain.DTOs;
using HongPet.Domain.Entities;
using HongPet.Infrastructure.DTOs;

namespace HongPet.Domain;
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

        CreateMap<User, UserDto>()
            .ForMember(model => model.TotalOrders,
                opt => opt.MapFrom(x => x.Orders.Count()))
            .ForMember(model => model.TotalSpend,
                opt => opt.MapFrom(x => x.Orders.Sum(o => o.TotalAmount)));
    }
}
