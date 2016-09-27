using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PilotKit.Orchestration.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PilotKit.Web.Controllers
{
    public class LayoutController : Controller
    {
        private ILayoutOrchestrator orchestrator;

        public LayoutController(ILayoutOrchestrator orchestrator)
        {
            this.orchestrator = orchestrator;
        }

        // GET: /Layout/Index
        [AllowAnonymous]
        public IActionResult Index()
        {
            Console.WriteLine("UseName:" + User.Identity.Name);
            return View();
        }

        // GET: /Layout/Error
        public IActionResult Error()
        {
            return PartialView();
        }

        // GET: /Layout/GetPilotKitMenu
        public IActionResult GetPilotKitMenu(string category)
        {
            return Json(this.orchestrator.GetPilotKitMenu(category));
        }
    }
}
