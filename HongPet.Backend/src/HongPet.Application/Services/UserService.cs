using AutoMapper;
using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class UserService : GenericService<User>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUnitOfWork unitOfWork,
        IClaimService claimService,
        IMapper mapper) : base(unitOfWork, claimService)
    {
        _userRepository = _unitOfWork.UserRepository;
        _repository = _userRepository; // để dùng cho những hàm được override từ Generic sang UserRepository
        _mapper = mapper;
    }

    public async Task CreateNewAccountAsync(string email, string password)
    {
        if (await _userRepository.IsEmailExistAsync(email))
        {
            throw new ArgumentException("Email đã tồn tại!");
        }
        var user = new User
        {
            Email = email,
            Password = password.Hash()
        };

        await base.AddAsync(user);
    }

    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
    {
        var user = (await _repository
            .GetAsync(x => x.Email == email))
            .SingleOrDefault();
        if (user != null && user.DeletedDate != null)
        {
            throw new UnauthorizedAccessException(
                $"The account with email {email} is inactived.");
        }
        if (user != null && password.Verify(user.Password!))
        {
            return user;
        }
        return null;
    }

    public async Task<bool> ChangePasswordAsync(Guid id, 
        string oldPassword, string newPassword)
    {
        this.CheckAuthorize(id: id, checkOwner: true);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null || user.DeletedDate != null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        if (!oldPassword.Verify(user.Password))
        {
            throw new ArgumentException("Old password is incorrect.");
        }

        user.Password = newPassword.Hash();
        await base.UpdateAsync(user);

        return true;
    }    

    public async Task<UserVM> GetUserDetailAsync(Guid id)
    {
        this.CheckAuthorize(id, checkAdmin: true, checkOwner: true);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        return _mapper.Map<UserVM>(user);
    }

    public async Task<IPagedList<UserVM>> GetUsersListAsync(int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        this.CheckAuthorize(checkAdmin: true);

        var pagedUsers = await _userRepository
            .GetPagedAsync(pageIndex, pageSize, keyword);

        return _mapper.Map<PagedList<UserVM>>(pagedUsers);
    }

    public async Task InactiveUserAsync(Guid id)
    {
        this.CheckAuthorize(checkAdmin: true);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null || user.DeletedDate != null)
        {
            throw new KeyNotFoundException($"User with id {id} not found " +
                $"or been inactive already..");
        }

        await base.SoftDeleteAsync(id);
    }

    public async Task<bool> UpdateAvatarAsync(Guid id, string avatarUrl)
    {
        this.CheckAuthorize(id: id, checkOwner: true);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null || user.DeletedDate != null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        user.AvatarUrl = avatarUrl;
        await base.UpdateAsync(user);

        return true;
    }

    public async Task<UserVM> UpdateUserInfoAsync(Guid id, UserUpdateModel userModel)
    {
        this.CheckAuthorize(id: id, checkOwner: true);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null || user.DeletedDate != null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }

        _mapper.Map(userModel, user);
        await base.UpdateAsync(user);

        return _mapper.Map<UserVM>(user);
    }

    private void CheckAuthorize(Guid? id = null,
        bool checkAdmin = false, bool checkOwner = false)
    {
        var currentUserId = _claimService.GetCurrentUserId;

        var isAdmin = _claimService.IsAdmin.GetValueOrDefault();

        var isOwner = currentUserId != null 
                        && id != null 
                        && currentUserId.Value == id.Value;

        var isAuthorized = true;

        if ((checkOwner && !isOwner)
            && (checkAdmin && !isAdmin)
            && (checkOwner && checkAdmin && (!isAdmin || !isOwner)))
        {
            isAuthorized = false;
        }

        if (!isAuthorized)
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to access this feature.");
        }
    }
}
