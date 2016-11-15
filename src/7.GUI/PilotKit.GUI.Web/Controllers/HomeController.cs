using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PilotKit.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return PartialView();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return PartialView();
        }

        public IActionResult Error()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public IActionResult SideMenu()
        {
            return PartialView();
        }
    }
}
