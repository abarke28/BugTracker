using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory clientFactory, ILogger<HomeController> logger, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = config;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Log(LogLevel.Information, "Home: Index entered");

            var vm = new HomeIndexVm
            {
                CardCount = _configuration.GetValue<int>("DashboardCardCount")
            };

            var http = _clientFactory.CreateClient("bugs");
            var apiResponse = await http.GetStringAsync(String.Empty);
            vm.Bugs = JsonConvert.DeserializeObject<List<Bug>>(apiResponse);

            http = _clientFactory.CreateClient("projects");
            apiResponse = await http.GetStringAsync(String.Empty);
            vm.Projects = JsonConvert.DeserializeObject<List<Project>>(apiResponse);
            http.Dispose();

            return View(vm);
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