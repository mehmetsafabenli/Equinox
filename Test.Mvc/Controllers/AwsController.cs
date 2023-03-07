using Microsoft.AspNetCore.Mvc;

namespace Test.Mvc.Controllers;

public class AwsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}