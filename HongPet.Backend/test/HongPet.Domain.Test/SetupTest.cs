﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions.Commons;
using HongPet.Infrastructure.Database;
using HongPet.WebApi;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HongPet.Domain.Test;
public class SetupTest
{
    protected readonly Mock<AppConfiguration> _appConfigMock;
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
    protected readonly IFixture _fixture;
    protected readonly DbContextOptions<AppDbContext> _dbContextOptions;
    protected readonly IMapper _mapper;

    public SetupTest()
    {
        _appConfigMock = new Mock<AppConfiguration>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Set up AutoFixture to mock data of the object
        _fixture = new Fixture().Customize(new AutoMoqCustomization());        
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                          .ToList()
                          .ForEach(b => _fixture.Behaviors.Remove(b));  // remove the exception for the recursion relation object
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior()); // ignore the recursion relation object

        // Set up InMemory database
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "HongPetDbTest")
            .Options;
        // set up auto mapper
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        _mapper = mappingConfig.CreateMapper();
    }
    protected List<User> UsersMockData(int count)
    {
        return _fixture.Build<User>()
                       .Without(u => u.Orders)   
                       .Without(u => u.Reviews)   
                       .CreateMany(count)                       
                       .ToList();
    }
}
