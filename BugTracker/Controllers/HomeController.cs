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
                { BugStatus.Open, "New" },
                { BugStatus.Assigned, BugStatus.Assigned.ToString() },
                { BugStatus.Closed, BugStatus.Closed.ToString() },
                { BugStatus.Resolved, BugStatus.Resolved.ToString() }
            };

            var BugsList = await _bugsApi.GetBugsAsync();

            foreach (var pair in coloumns)
            {
                vm.Stacks.Add(new DashboardStack
                {
                    AssociatedStatus = pair.Key,
                    Title = pair.Value,
                    Bugs = BugsList.Where(b => b.Status.HasFlag(pair.Key)).OrderByDescending(b => b.DateSubmitted).Take(vm.CardCount).ToList()
                });
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