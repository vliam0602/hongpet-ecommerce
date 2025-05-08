using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using HongPet.Application.Commons;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.WebApi;
using Moq;

namespace HongPet.Domain.Test;
public class SetupTest
{
    protected readonly Mock<AppConfiguration> _appConfigMock;
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
    protected readonly Mock<IClaimService> _claimServiceMock;
    protected readonly IFixture _fixture;
    protected readonly IMapper _mapper;

    public SetupTest()
    {
        _appConfigMock = new Mock<AppConfiguration>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _claimServiceMock = new Mock<IClaimService>();

        // Set up AutoFixture to mock data of the object
        _fixture = new Fixture().Customize(new AutoMoqCustomization());        
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                          .ToList()
                          .ForEach(b => _fixture.Behaviors.Remove(b));  // remove the exception for the recursion relation object
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior()); // ignore the recursion relation object

        // set up auto mapper
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
            mc.AddProfile(new MappingDto());
        });
        _mapper = mappingConfig.CreateMapper();
    }
    protected List<User> MockUsers(int count)
    {
        return _fixture.Build<User>()
                       .Without(u => u.Orders)   
                       .Without(u => u.Reviews)   
                       .CreateMany(count)                       
                       .ToList();
    }    

    protected List<Category> MockCategoriesNotDeleted(int count)
    {
        return _fixture.Build<Category>()
                                 .Without(c => c.SubCategories)
                                 .Without(c => c.Products)
                                 .Without(c => c.DeletedDate)
                                 .Without(c => c.DeletedBy)
                                 .CreateMany(count)
                                 .ToList();
    }
    

}
