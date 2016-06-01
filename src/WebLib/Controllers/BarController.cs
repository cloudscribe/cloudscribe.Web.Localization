using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WebLib.Controllers
{
    public class BarController : Controller
    {
        public BarController(
            IStringLocalizer<BarController> localizer)
        {
            _localizer = localizer;
        }

        private readonly IStringLocalizer _localizer;

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["bar"];
            ViewData["Message"] = _localizer["this is the bar controller"];

            return View();
        }

    }
}
