using Api.Areas.Rest.Controllers.Account.Dto.Request;
using AutoMapper;
using Logic.Account.Interfaces;
using Logic.Account.Models;
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
    
    [Route("register")]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
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

    [Route("login")]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm]LoginRequest dto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        try
        {
            var authorizeModel = _mapper.Map<AuthorizationModel>(dto);
            var token = await _accountManager.AuthorizeUser(authorizeModel);
            TempData["USER_TOKEN"] = token;
            return Redirect("/cabinet");
        }
        catch
        {
            ModelState.AddModelError("Phone", "Ошибка авторизации");
            return View();
        }
    }

    [HttpGet]
    public IActionResult RegisterSuccess()
    {
        ViewData["FIO"] = TempData["FIO"];
        return View();
    }
}