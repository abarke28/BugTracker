using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.utils;
using BugTracker.Models;
using BugTracker.Models.Dtos;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace BugTracker.Controllers
{
    [Authorize]
    public class BugsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<BugsController> _logger;

        public BugsController(IHttpClientFactory clientFactory, ILogger<BugsController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [HttpGet("bugs")]
        public async Task<IActionResult> Index()
        {
            var vm = new BugIndexVm();

            var http = _clientFactory.CreateClient("bugs");
            var apiResponse = await http.GetStringAsync(String.Empty);
            vm.Bugs = JsonConvert.DeserializeObject<List<Bug>>(apiResponse);

            http = _clientFactory.CreateClient("projects");
            apiResponse = await http.GetStringAsync(String.Empty);
            vm.Projects = JsonConvert.DeserializeObject<List<Project>>(apiResponse);

            return View("Index", vm);
        }

        [HttpGet("projects/detail/{projectId}/{id}")]   
        public async Task<IActionResult> Detail(int projectId, int id)
        {
            var vm = new BugDetailVm 
            { 
                Bug = new Bug(),
                ProjectId = projectId,
                UserName = User.FindFirst(ClaimTypes.Name).Value.Split("@")?[0] ?? "guest"
            };

            var httpClient = _clientFactory.CreateClient("bugs");
            var apiResponse = await httpClient.GetAsync(id.ToString()).Result.Content.ReadAsStringAsync();
            vm.Bug = JsonConvert.DeserializeObject<Bug>(apiResponse);

            httpClient = _clientFactory.CreateClient("projects");
            apiResponse = await httpClient.GetAsync(projectId.ToString()).Result.Content.ReadAsStringAsync();
            vm.ProjectName = JsonConvert.DeserializeObject<Project>(apiResponse).Title;

            return View(vm);
        }

        [HttpGet("bugs/new/{projectId}/{projectName}")]
        public IActionResult New(int projectId, string projectName)
        {
            var vm = new NewBugVm { Bug = new BugDto() };
            vm.Bug.ProjectId = projectId;
            vm.ProjectName = projectName;

            return View("NewBugForm", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(NewBugVm vm)
        {
            // Summary
            //
            // Post submitted Bug to the internal API, redirect to either index or
            // back to form based on success

            if (!ModelState.IsValid) return View("New", vm);

            var http = _clientFactory.CreateClient("bugs");
            var result = await http.PostAsync(String.Empty, vm.Bug);
            var bugStr = await result.Content.ReadAsStringAsync();
            var bugId = JsonConvert.DeserializeObject<Bug>(bugStr).Id;

            return result.IsSuccessStatusCode ?
                RedirectToAction("Detail", new { projectId = vm.Bug.ProjectId, id = bugId }) :
                RedirectToAction("New", new { projectId = vm.Bug.ProjectId, projectName = vm.ProjectName });
        }

        [HttpGet("bugs/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = new EditBugVm();

            var http = _clientFactory.CreateClient("bugs");
            var apiResponse = await http.GetAsync(id.ToString()).Result.Content.ReadAsStringAsync();
            vm.Bug = JsonConvert.DeserializeObject<Bug>(apiResponse);
            vm.Assigned = ((BugStatus.Assigned & vm.Bug.Status) == BugStatus.Assigned);
            vm.Reopened = ((BugStatus.Reopened & vm.Bug.Status) == BugStatus.Reopened);

            http = _clientFactory.CreateClient("projects");
            apiResponse = await http.GetAsync(vm.Bug.ProjectId.ToString()).Result.Content.ReadAsStringAsync();
            vm.ProjectName = JsonConvert.DeserializeObject<Project>(apiResponse).Title;

            return View("EditBugForm", vm);
        }

        [HttpPost("bugs/save")]
        public async Task<IActionResult> Save(EditBugVm vm)
        {
            var bug = vm.Bug;

            if (vm.Assigned) bug.Status |= BugStatus.Assigned;
            if (vm.Reopened) bug.Status |= BugStatus.Reopened;

            var http = _clientFactory.CreateClient("bugs");
            var apiResponse = await http.PutAsync(bug.Id.ToString(), bug);

            if (!apiResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("Failed to update bug with id {0}", bug.Id);
            }

            // Fetch associated comments from API
            var apiResponseStr = await http.GetAsync(bug.Id.ToString()).Result.Content.ReadAsStringAsync();
            bug = JsonConvert.DeserializeObject<Bug>(apiResponseStr);

            var detailVm = new BugDetailVm
            {
                Bug = bug,
                ProjectId = bug.ProjectId,
                ProjectName = vm.ProjectName,
                UserName = User.FindFirst(ClaimTypes.Name).Value.Split("@")?[0] ?? "guest"
            };

            return View("Detail", detailVm);
        }
    }
}