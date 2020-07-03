using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFirstVimeoWebApp.Models;
using VimeoOpenApi.Api;

namespace MyFirstVimeoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIInformationEssentialsApi informationEssentialsApi;

        public HomeController(ILogger<HomeController> logger, IAPIInformationEssentialsApi informationEssentialsApi)
        {
            _logger = logger;
            this.informationEssentialsApi = informationEssentialsApi;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // XXX 定義と実際の返却値にギャップがあるのでデシリアライズできない。他にもそういうエンドポイントあるかも。
            var res = await informationEssentialsApi.GetEndpointsAsyncWithHttpInfo();
            ViewBag.Endpoints = JsonSerializer.Serialize(res.Data);
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
