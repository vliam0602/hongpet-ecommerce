using AutoMapper;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>));

        // product mappings
        CreateMap<Product, ProductGeneralVM>();

        CreateMap<Product, ProductDetailVM>()
            .ForMember(model => model.CountOfReviews, 
                opt => opt.MapFrom(x => x.Reviews.Count()));

        CreateMap<Variant, VariantVM>();

        CreateMap<ProductAttributeValue, AttributeValueVM>()
            .ForMember(model => model.Attribute, 
                opt => opt.MapFrom(x => x.Attribute.Name));

        CreateMap<ProductModel, Product>();

        // review mappings
        CreateMap<Review, ReviewVM>()
            .ForMember(model => model.ReviewerName,
                opt => opt.MapFrom(x => x.Customer.Username))
            .ForMember(model => model.ReviewerAvatar,
                opt => opt.MapFrom(x => x.Customer.AvatarUrl));

        CreateMap<ReviewCreateModel, Review>();

        CreateMap<ReviewUpdateModel, Review>();

        // order mappings
        CreateMap<OrderCreationModel, Order>();

        CreateMap<OrderItemCreationModel, OrderItem>();

        CreateMap<Order, OrderVM>();

        CreateMap<OrderItem, OrderItemVM>()
            .ForMember(model => model.ProductId,
                opt => opt.MapFrom(x => x.Variant.ProductId))
            .ForMember(model => model.ProductName,
                opt => opt.MapFrom(x => x.Variant.ProductName))
            .ForMember(model => model.AttributeValues,
                opt => opt.MapFrom(x => x.Variant.AttributeValues));

        // user mappings
        CreateMap<User, UserVM>();
        CreateMap<UserUpdateModel, User>();
    }
}
