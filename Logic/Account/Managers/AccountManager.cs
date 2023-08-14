using System.Security.Claims;
using AutoMapper;
using Dal.Models;
using Dal.Repositories.Interfaces;
using Logic.Account.Interfaces;
using Logic.Account.Models;
using Logic.Account.Services.Interfaces;

namespace Logic.Account;

public class AccountManager : IAccountManager
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IClaimsService<UserDal> _claimsService;
    
    public AccountManager(
        IUserRepository userRepository, 
        IMapper mapper, 
        IClaimsService<UserDal> claimsService
        )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _claimsService = claimsService;
    }
    
    public async Task<UserDal> CreateAsync(AccountCreateModel createModel)
    {
        var hasUser = await _userRepository.ExistByAsync(new Dictionary<string, object>()
            {
                {nameof(UserDal.Email), createModel.Email},
                {nameof(UserDal.Phone), createModel.Phone}
            }
        );
        if (hasUser)
        {
            throw new Exception("user is exist");
        }
        var userData = _mapper.Map<UserDal>(createModel);
        return await _userRepository.CreateAsync(userData);
    }
    
    public async Task<ClaimsPrincipal> AuthUserAsync(AuthUserModel model)
    {
        var users = await _userRepository.GetByField(new Dictionary<string, object>()
            {
                { nameof(UserDal.Phone), model.Phone }
            }
        );
        var user = users.FirstOrDefault();
        if (user == null || user.Password != model.Password)
        {
            throw new Exception("there is no user with such a password or phone number");
        }
        user = await UpdateLastLogin(user);
        return _claimsService.GetClaimsPrincipal(user);;
    }

    /// <summary>
    /// Обновление даты последнего входа для пользователя
    /// </summary>
    /// <param name="user">Модель пользователя</param>
    private async Task<UserDal> UpdateLastLogin(UserDal user)
    {
        DateTime utcTime = DateTime.UtcNow;
        var freshUser = await _userRepository.UpdateAsync(new Dictionary<string, object>()
        {
            {nameof(UserDal.LastLogin), utcTime}
        }, new Dictionary<string, object>()
        {
            {nameof(UserDal.Id), user.Id}
        });
        return freshUser;
    }
    
    /// <summary>
    /// Получить пользователя по номеру телефона
    /// </summary>
    /// <param name="phone">Номер телефона</param>
    /// <returns>Модель пользователя</returns>
    public async Task<UserDal> GetItemByPhoneAsync(string phone)
    {
        var users = await _userRepository.GetByField(new Dictionary<string, object>()
        {
            {nameof(UserDal.Phone), phone}
        });
        return users.FirstOrDefault();
    }
}