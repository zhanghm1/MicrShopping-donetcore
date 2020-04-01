using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MicrShopping.WebMVC.Models;
using Newtonsoft.Json.Linq;

namespace MicrShopping.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string orderServerUrl = Configuration["OrderServerUrl"];
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(orderServerUrl+"/order/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
