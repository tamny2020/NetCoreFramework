using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using My.FrameworkCore.Models.Domain;
using My.FrameworkCore.Services.Interface;
using My.FrameworkCore.Web.Models;

namespace My.FrameworkCore.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;

        public HomeController(IUserService serService)
        {
            _userService = serService;
        }
        public IActionResult Index()
        {
            var user = _userService.GetById(1);

            //http://localhost:33054/api/values/get

            //var resultq = _userService.Insert(new User { Name = "王小虎" });

            return View(user);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
