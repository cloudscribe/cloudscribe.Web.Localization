using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WebLib.Controllers
{
    public class OtherController : Controller
    {
        public OtherController(
            IStringLocalizer<OtherController> localizer
            )
        {
            _localizer = localizer;
        }

        private readonly IStringLocalizer _localizer;

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["Otherwise"];
            ViewData["Message"] = _localizer["this is the other one"];

            return View();
        }
    }
}
