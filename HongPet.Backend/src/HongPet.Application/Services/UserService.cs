using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions.Commons;

namespace HongPet.Application.Services;
public class UserService : GenericService<User>, IUserService
{
    private readonly IClaimService _claimService;
    public UserService(IUnitOfWork unitOfWork, IClaimService claimService) 
        : base(unitOfWork)
    {
        _claimService = claimService;
    }

    public async Task<User?> CheckLoginAsync(string email, string password)
    {
        var user = (await _repository
            .GetAsync(x => x.Email.Equals(email)))
            .SingleOrDefault();
        if (user != null && password.Verify(user.Password!))
        {
            return user;
        }
        return null;
    }

    public async Task CreateNewUserAsync(User user)
    {
        // check if email duplicated
        var errMsg = await ValidateAccountAsync(user);
        if (!string.IsNullOrEmpty(errMsg))
        {
            throw new ArgumentException(errMsg);
        }
        // account valid -> add new account in db
        if (user.Password != null)
        {
            user.Password = user.Password.Hash(); // hash the password
        }
        await _repository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task EditPasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _repository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy tài khoản.");
        }
        // check authorized
        if (user.Id != _claimService.GetCurrentUserId)
        {
            throw new UnauthorizedAccessException("Bạn không có quyền sử dụng chức năng này.");
        }
        // check old password
        if (oldPassword.Verify(user.Password!))
        {
            // change password
            user.Password = newPassword.Hash();
            _repository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        } else
        {
            throw new ArgumentException("Mật khẩu cũ không đúng.");
        }
    }

    private async Task<string> ValidateAccountAsync(User user)
    {
        // check if email duplicated
        string errMsg = "";        
        var tmpAcc = (await _repository
                        .GetAsync(x => x.Email.Equals(user.Email)))
                    .SingleOrDefault();
        if (tmpAcc != null)
        {
            errMsg = "Email đã tồn tại.";
        }
        return errMsg;
    }
}
