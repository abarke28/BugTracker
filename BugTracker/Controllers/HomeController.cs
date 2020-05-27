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
                CardCount = _configuration.GetValue<int>("DashboardCardCount"),
                Stacks = new List<DashboardStack>()
            };

            var http = _clientFactory.CreateClient("bugs");
            var apiResponse = await http.GetStringAsync(String.Empty);
            var BugsList = JsonConvert.DeserializeObject<List<Bug>>(apiResponse);

            vm.Stacks.Add(new DashboardStack
            {
                AssociatedStatus = BugStatus.Open,
                Title = "NEW",
                Bugs = BugsList.Where(b => b.Status.HasFlag(BugStatus.Open)).OrderByDescending(b => b.DateSubmitted).Take(vm.CardCount).ToList()
            });

            vm.Stacks.Add(new DashboardStack
            {
                AssociatedStatus = BugStatus.Assigned,
                Title = "ASSIGNED",
                Bugs = BugsList.Where(b => b.Status.HasFlag(BugStatus.Assigned)).OrderByDescending(b => b.DateSubmitted).Take(vm.CardCount).ToList()
            });

            vm.Stacks.Add(new DashboardStack
            {
                AssociatedStatus = BugStatus.Closed,
                Title = "CLOSED",
                Bugs = BugsList.Where(b => b.Status.HasFlag(BugStatus.Closed)).OrderByDescending(b => b.DateSubmitted).Take(vm.CardCount).ToList()
            });

            vm.Stacks.Add(new DashboardStack
            {
                AssociatedStatus = BugStatus.Resolved,
                Title = "RESOLVED",
                Bugs = BugsList.Where(b => b.Status.HasFlag(BugStatus.Resolved)).OrderByDescending(b => b.DateSubmitted).Take(vm.CardCount).ToList()
            });

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