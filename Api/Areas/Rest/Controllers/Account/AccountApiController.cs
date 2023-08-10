using System.Net;
using Api.Areas.Rest.Controllers.Account.Dto.Request;
using Api.Areas.Rest.Controllers.Account.Dto.Response;
using Logic.Account.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Dal.Models;
using Logic.Account.Models;
using Logic.Account.Services.Interfaces;
using Logic.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Api.Areas.Rest.Controllers.Account;

[ApiController]
[Route("api/account")]
public class AccountApiController : ControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly IMapper _mapper;
    private readonly IClaimsService<UserDal> _claimsService;
    private readonly IJwtTokenService _jwtTokenService;

    public AccountApiController(
        IAccountManager accountManager, 
        IMapper mapper,
        IClaimsService<UserDal> claimsService,
        IJwtTokenService jwtTokenService
        )
    {
        _accountManager = accountManager;
        _mapper = mapper;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),400)]
    public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest dto)
    {
        try
        {
            var user = _mapper.Map<AccountCreateModel>(dto);
            await _accountManager.CreateAsync(user);
            return Ok();
        }
        catch
        {
            var error = new ErrorResponse()
            {
                Code = HttpStatusCode.BadRequest,
                Message = "the user is already registered"
            };
            return BadRequest(error);
        }
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginRequest), 200)]
    public async Task<IActionResult> Login([FromBody]LoginRequest dto)
    {
        var user = await _accountManager.GetItemByPhoneAsync(dto.Phone);
        if (user == null || !user.Password.Equals(dto.Password))
        {
            var error = new ErrorResponse()
            {
                Code = HttpStatusCode.BadRequest,
                Message = "Login error. Wrong phone or password"
            };
            return BadRequest(error); 
        }

        var claimList = _claimsService.GetClaims(user);
        return Ok( _jwtTokenService.GetJwtToken(claimList));
    }
    
    [Authorize]
    [HttpGet("logout")]
    public IActionResult Login()
    {
        return BadRequest();
    }

    [Authorize]
    [HttpGet("get-my-info")]
    //[ProducesResponseType(typeof(LoginRequest), 200)]
    public IActionResult DetailInformation()
    {
        return BadRequest();
    }
    
    //[Authorize]
    [HttpGet("all")]
    //[ProducesResponseType(typeof(LoginRequest), 200)]
    public async Task<IActionResult> GetAllAccount()
    {
        var accounts = await _accountManager.GetAllAsync();
        return Ok(accounts);
    }
}