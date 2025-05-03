using AutoMapper;
using HongPet.Domain.DTOs;
using HongPet.Domain.Entities;
using HongPet.Infrastructure.DTOs;
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
        CreateMap<Product, ProductGeneralVM>()
            .ForMember(model => model.TotalVariants,
                opt => opt.MapFrom(x => x.Variants.Count()));

        CreateMap<Product, ProductDetailVM>()
            .ForMember(model => model.CountOfReviews, 
                opt => opt.MapFrom(x => x.Reviews.Count()))
            .ForMember(model => model.AverageStars,
                opt => opt.MapFrom(x => (x.Reviews.Sum(r => r.Rating) / 5)));

        CreateMap<Variant, VariantVM>();        

        CreateMap<ProductModel, Product>();

        CreateMap<Image, ImageVM>();

        // review mappings
        CreateMap<Review, ReviewVM>()
            .ForMember(model => model.ReviewerName,
                opt => opt.MapFrom(x => x.Customer.Username))
            .ForMember(model => model.ReviewerAvatar,
                opt => opt.MapFrom(x => x.Customer.AvatarUrl));

        CreateMap<ReviewCreateModel, Review>();

        CreateMap<ReviewUpdateModel, Review>();
        CreateMap<Review, ReviewGeneralVM>();

        // order mappings
        CreateMap<OrderCreationModel, Order>();

        CreateMap<OrderItemCreationModel, OrderItem>();

        CreateMap<Order, OrderVM>();        

        CreateMap<ProductAttributeValue, AttributeValueVM>()
            .ForMember(model => model.Attribute,
                opt => opt.MapFrom(x => x.Attribute.Name));

        CreateMap<OrderDto, OrderVM>();

        CreateMap<OrderItemDto, OrderItemVM>();

        CreateMap<AttributeValueDto, AttributeValueVM>();        

        // user mappings
        CreateMap<User, UserVM>();
        CreateMap<UserDto, UserVM>();
        CreateMap<UserUpdateModel, User>();

        // category mappings
        CreateMap<Category, CategoryVM>()
            .ForMember(model => model.ParentCategoryName,
                opt => opt.MapFrom(x => x.ParentCategory!= null ? 
                        x.ParentCategory.Name : null));
        CreateMap<CategoryCreateModel, Category>();
    }
}
