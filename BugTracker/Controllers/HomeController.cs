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
using Microsoft.AspNetCore.Authorization;
using BugTracker.utils;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ProjectsApiService _projectsApi;
        private readonly BugsApiService _bugsApi;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, ProjectsApiService projectsApiService, BugsApiService bugsApiService)
        {
            _logger = logger;
            _configuration = config;
            _projectsApi = projectsApiService;
            _bugsApi = bugsApiService;

            _logger.LogInformation(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        public async Task<IActionResult> Index()
        {
            _logger.Log(LogLevel.Information, "Home: Index entered");

            var vm = new HomeIndexVm
            {
                CardCount = _configuration.GetValue<int>("DashboardCardCount"),
                Stacks = new List<DashboardStack>(),
                Projects = await _projectsApi.GetProjectsAsync()
            };

            var coloumns = new Dictionary<BugStatus, string>
            {
                { BugStatus.Open, "Open" },
                { BugStatus.Assigned, "In Progress" },
                { BugStatus.Resolved, "Resolved" },
                { BugStatus.Closed, "Closed" }
            };

            var BugsList = await _bugsApi.GetBugsAsync();

            foreach (var pair in coloumns)
            {
                var stack = new DashboardStack
                {
                    AssociatedStatus = pair.Key,
                    Title = pair.Value
                };

                var bugs = BugsList.Where(b => b.Status.HasFlag(pair.Key)).OrderByDescending(b => b.DateSubmitted);
                stack.Count = bugs.Count();
                stack.Bugs = bugs.Take(vm.CardCount).ToList();

                vm.Stacks.Add(stack);
            }

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