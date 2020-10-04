using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApplication.Controllers.Helpers;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var resultJson = await GetData();
            var result = JsonConvert.DeserializeObject<Obb>(resultJson);
            WriteInViewBag(result);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private async Task<string> GetData()
        {
            var address = new Uri("http://localhost:343/IContract");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(address);
            var channelFactory = new ChannelFactory<IContract>(binding, endpoint);
            var channel = channelFactory.CreateChannel();
            return await channel.GetInfo();
        }

        /// <summary>
        ///     Запись в ViewBag
        /// </summary>
        /// <param name="obj"></param>
        private void WriteInViewBag(Obb obj)
        {
            ViewBag.Logs = obj.Logs;
            ViewBag.Write = obj.Writing;
        }
    }
}