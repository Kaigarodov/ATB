using Api.Areas.Cabinet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Api.Areas.Cabinet.Controllers;

[Area("Cabinet")]
[Route("cabinet")]
public class CabinetController : Controller
{
    
    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new CabinetViewModel();
        return View(viewModel);
    }
}