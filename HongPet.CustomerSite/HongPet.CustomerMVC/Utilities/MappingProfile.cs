using AutoMapper;
using HongPet.CustomerMVC.Models;
using HongPet.SharedViewModels.Models;

namespace HongPet.CustomerMVC.Utilities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, RegisterModel>().ReverseMap();
    }
}
