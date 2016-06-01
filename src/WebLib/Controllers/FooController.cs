using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WebLib.Controllers
{
    public class FooController : Controller
    {
        public FooController(
            IStringLocalizerFactory localizerFactory
            )
        {
            _localizer = localizerFactory.Create("MyResources.Controllers.FooController", "WebLib"); 
        }

        private readonly IStringLocalizer _localizer;

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["Foo"];
            ViewData["Message"] = _localizer["Greetings ya'll"];

            return View();
        }
    }
}
