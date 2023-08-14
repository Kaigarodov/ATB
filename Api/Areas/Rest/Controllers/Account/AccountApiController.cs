using System.Net;
using System.Security.Claims;
using Api.Common.Account.Dto.Request;
using Api.Common.Account.Dto.Response;
using Logic.Account.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Logic.Account.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace Api.Areas.Rest.Controllers.Account;

[ApiController]
[Route("api/account")]
public class AccountApiController : ControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly IMapper _mapper;

    public AccountApiController(
        IAccountManager accountManager, 
        IMapper mapper
        )
    {
        _accountManager = accountManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
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
    
    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> Login([FromBody]LoginRequest dto)
    {
        try
        {
            var model = _mapper.Map<AuthUserModel>(dto);
            var claims = await _accountManager.AuthUserAsync(model);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claims));

            return Ok();
        }
        catch
        {
            var error = new ErrorResponse()
            {
                Code = HttpStatusCode.BadRequest,
                Message = "Login error. Wrong phone or password"
            };
            return BadRequest(error); 
        }
    }
    
    /// <summary>
    /// Выход пользователя из авторизованной зоны
    /// </summary>
    [Authorize]
    [HttpGet("logout")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    /// <summary>
    /// Детальная информация о пользователе
    /// </summary>
    [Authorize]
    [HttpGet("get-my-info")]
    [ProducesResponseType(typeof(UserInfoResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DetailInformation()
    {
        try
        {
            var phone = HttpContext.User.FindFirst(ClaimTypes.MobilePhone).Value;
            var user = await _accountManager.GetItemByPhoneAsync(phone);
            var UserInfoResponse = _mapper.Map<UserInfoResponse>(user);
            return Ok(UserInfoResponse);
        }
        catch
        {
            var error = new ErrorResponse();
            return BadRequest(error);
        }
    }
}