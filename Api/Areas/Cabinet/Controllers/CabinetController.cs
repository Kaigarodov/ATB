using System.Security.Claims;
using Api.Areas.Cabinet.Models;
using AutoMapper;
using Dal.Models;
using Logic.Account.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Cabinet.Controllers;

[Area("Cabinet")]
[Route("cabinet")]
public class CabinetController : Controller
{
    private readonly IAccountManager _accountManager;
    private readonly IMapper _mapper;
    public CabinetController(
        IAccountManager accountManager,
        IMapper mapper
        )
    {
        _accountManager = accountManager;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var phone = HttpContext.User.FindFirst(ClaimTypes.MobilePhone).Value;
        var user = await _accountManager.GetItemByPhoneAsync(phone);
        if (user is not UserDal)
        {
            return Redirect("/account/logout");
        }

        var viewModel = _mapper.Map<CabinetViewModel>(user);
        return View(viewModel);
    }
}