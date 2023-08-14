using System.Security.Claims;
using Api.Common.Account.Dto.Request;
using AutoMapper;
using Logic.Account.Interfaces;
using Logic.Account.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Area("Account")]
[Route("account")]
public class AccountController : Controller
{
    private readonly IAccountManager _accountManager;
    private readonly IMapper _mapper;

    public AccountController(
        IAccountManager accountManager, 
        IMapper mapper
    )
    {
        _accountManager = accountManager;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Страница регистрации
    /// </summary>
    [Route("register")]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    /// <summary>
    /// Обработка формы регистрации пользователя
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm]RegisterRequest dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            var createModel = _mapper.Map<AccountCreateModel>(dto);
            var user = await _accountManager.CreateAsync(createModel);
            TempData["FIO"] = user.FIO;
            return RedirectToAction("RegisterSuccess");
        }
        catch
        {
            ModelState.AddModelError("", "Не удалось зарегистрировать пользователя");
            return View();
        }
    }
    
    /// <summary>
    /// Страница авторизации пользователя
    /// </summary>
    [Route("login")]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    /// <summary>
    /// Обработка данных с формы авторизации
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm]LoginRequest dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            var authorizeModel = _mapper.Map<AuthUserModel>(dto);
            var claims = await _accountManager.AuthUserAsync(authorizeModel);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claims));
            return Redirect("/cabinet");
        }
        catch
        {
            ModelState.AddModelError("Phone", "Ошибка авторизации");
            return View();
        }
    }
    
    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    [Authorize]
    [Route("logout")]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("login");
    }

    /// <summary>
    /// Успешный слой после регистрации
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult RegisterSuccess()
    {
        ViewData["FIO"] = TempData["FIO"];
        return View();
    }
}