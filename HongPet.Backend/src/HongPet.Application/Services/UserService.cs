using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;

namespace HongPet.Application.Services;
public class UserService : GenericService<User>, IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(
        IUnitOfWork unitOfWork,
        IClaimService claimService) : base(unitOfWork, claimService)
    {
        _userRepository = _unitOfWork.UserRepository;
        _repository = _userRepository; // để dùng cho những hàm được override từ Generic sang UserRepository
    }

    public async Task CreateNewAccount(string email, string password)
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

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        var user = (await _repository
            .GetAsync(x => x.Email == email))
            .SingleOrDefault();

        if (user != null && password.Verify(user.Password!))
        {
            return user;
        }
        return null;
    }


}
