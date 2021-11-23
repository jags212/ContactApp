using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactApp.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
