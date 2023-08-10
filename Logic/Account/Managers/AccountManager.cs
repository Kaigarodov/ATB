using AutoMapper;
using Dal.Models;
using Dal.Repositories.Interfaces;
using Logic.Account.Interfaces;
using Logic.Account.Models;
using Logic.Account.Services.Interfaces;
using Logic.Application.Services.Interfaces;

namespace Logic.Account;

public class AccountManager : IAccountManager
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IClaimsService<UserDal> _claimsService;
    private readonly IJwtTokenService _jwtTokenService;

    
    public AccountManager(
        IUserRepository userRepository, 
        IMapper mapper, 
        IClaimsService<UserDal> claimsService,
        IJwtTokenService jwtTokenService
        )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
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

    public async Task<string> AuthorizeUser(AuthorizationModel model)
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
        var token = await CreateUserToken(user);
        
        return token;
    }

    public async Task<string> CreateUserToken(UserDal user)
    {
        var claims = _claimsService.GetClaims(user);
        var token = _jwtTokenService.GetJwtToken(claims);

        return token;
    }

    //TODO: доделать
    private async Task<UserDal> UpdateLastLogin(UserDal user)
    {
        DateTime utcTime = DateTime.UtcNow;
        await _userRepository.UpdateAsync(new Dictionary<string, object>()
        {
            {nameof(UserDal.LastLogin), utcTime}
        }, new Dictionary<string, object>()
        {
            {nameof(UserDal.Id), user.Id}
        });
        user.LastLogin = utcTime;
        return user;
    }
    
    public async Task<UserDal> GetItemByPhoneAsync(string phone)
    {
        var users = await _userRepository.GetByField(new Dictionary<string, object>()
        {
            {nameof(UserDal.Phone), phone}
        });
        return users.FirstOrDefault();
    }
    
    public async Task<List<UserDal>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }
    
}