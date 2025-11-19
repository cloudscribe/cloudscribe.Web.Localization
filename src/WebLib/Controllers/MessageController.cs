using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebLib.ViewModels;

namespace WebLib.Controllers
{
    public class MessageController : Controller
    {

        public IActionResult Index()
        {
            ViewData["Title"] = "Message";

            var model = new ContactMessageViewModel();

            return View(model);
        }
    }
}
