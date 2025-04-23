using AutoMapper;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>));

        // mapping for Product
        CreateMap<Product, ProductGeneralVM>();

        CreateMap<Product, ProductDetailVM>()
            .ForMember(model => model.CountOfReviews, 
                opt => opt.MapFrom(x => x.Reviews.Count()));

        CreateMap<Variant, VariantVM>();

        CreateMap<ProductAttributeValue, AttributeValueVM>()
            .ForMember(model => model.Attribute, 
                opt => opt.MapFrom(x => x.Attribute.Name));

        //mapping for review
        CreateMap<Review, ReviewVM>()
            .ForMember(model => model.ReviewerName,
                opt => opt.MapFrom(x => x.Customer.Username))
            .ForMember(model => model.ReviewerAvatar,
                opt => opt.MapFrom(x => x.Customer.AvatarUrl));

        // mapping for order
        CreateMap<OrderCreationModel, Order>();

        CreateMap<OrderItemCreationModel, OrderItem>();

        CreateMap<Order, OrderVM>();

        CreateMap<OrderItem, OrderItemVM>()
            .ForMember(model => model.ProductName,
                opt => opt.MapFrom(x => x.Variant.ProductName))
            .ForMember(model => model.AttributeValues,
                opt => opt.MapFrom(x => x.Variant.AttributeValues));
    }
}
