using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions.Commons;

namespace HongPet.Application.Services;
public class UserTokenService : GenericService<UserToken>, IUserTokenService
{
    public UserTokenService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _repository = _unitOfWork.UserTokenRepository;
    }    
}
