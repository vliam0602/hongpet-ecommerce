using AutoMapper;
using AutoMapper.QueryableExtensions;
using HongPet.Domain.DTOs;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.SharedViewModels.Generals;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    private IMapper _mapper;
    public UserRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<IPagedList<UserDto>> GetCustomersPagedAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 1;
        
        var query = _dbSet
            .Where(x => x.Role != Domain.Enums.RoleEnum.Admin)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query
                .Where(x => x.Fullname.Contains(keyword))
                .AsQueryable();
        }

        var totalCount = await query.CountAsync();

        var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
        
        if (pageIndex > totalPage) pageIndex = totalPage;

        var items = await query  
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<UserDto>(items, totalCount, pageIndex, pageSize);
    }
}
