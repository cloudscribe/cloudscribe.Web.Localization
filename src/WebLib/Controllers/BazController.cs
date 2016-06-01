using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace WebLib.Controllers
{
    public class BazController : Controller
    {
        public BazController(
            IStringLocalizerFactory localizerFactory)
        {
            _localizer = localizerFactory.Create("Controllers.BazController", "WebLib");
        }

        private readonly IStringLocalizer _localizer;

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["Baz"];
            ViewData["Message"] = _localizer["Greetings from Baz"];

            return View();
        }
    }
}
