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
        private readonly ILogger<BugsController> _logger;
        private readonly BugsApiService _bugsApi;
        private readonly ProjectsApiService _projectsApi;

        public BugsController(ILogger<BugsController> logger, BugsApiService bugsApiService, ProjectsApiService projectsApiService)
        {
            _logger = logger;
            _bugsApi = bugsApiService;
            _projectsApi = projectsApiService;
        }

        [HttpGet("bugs")]
        public async Task<IActionResult> Index()
        {
            // Summary
            //
            // Fetch all bugs and projects from Endpoints, render view. Projects are used for display purposes

            var vm = new BugIndexVm
            {
                Bugs = await _bugsApi.GetBugsAsync(),
                Projects = await _projectsApi.GetProjectsAsync()
            };

            return View("Index", vm);
        }

        [HttpGet("projects/detail/{projectId}/{id}")]   
        public async Task<IActionResult> Detail(int projectId, int id)
        {
            // Summary
            //
            // Fetch bug, and project details from Endpoints and render associated view

            var vm = new BugDetailVm 
            { 
                Bug = await _bugsApi.GetBugAsync(id).ConfigureAwait(false),
                ProjectId = projectId,
                UserName = User.FindFirst(ClaimTypes.Name).Value.Split("@")?[0] ?? "guest"
            };

            var project = await _projectsApi.GetProjectAsync(projectId).ConfigureAwait(false);
            vm.ProjectName = project.Title;

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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(NewBugVm vm)
        {
            // Summary
            //
            // Post submitted Bug to the internal API, redirect to either index or
            // back to form based on success

            if (!ModelState.IsValid) return View("New", vm);

            var result = await _bugsApi.PostBugAsync(vm.Bug);
            var bugStr = await result.Content.ReadAsStringAsync();
            var bugId = JsonConvert.DeserializeObject<Bug>(bugStr).Id;

            return result.IsSuccessStatusCode ?
                RedirectToAction("Detail", new { projectId = vm.Bug.ProjectId, id = bugId }) :
                RedirectToAction("New", new { projectId = vm.Bug.ProjectId, projectName = vm.ProjectName });
        }

        [HttpGet("bugs/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            // Summary
            //
            // Fetch associated Bug & Project Name, render view with VM

            var vm = new EditBugVm
            {
                Bug = await _bugsApi.GetBugAsync(id).ConfigureAwait(false)
            };

            vm.Assigned = ((BugStatus.Assigned & vm.Bug.Status) == BugStatus.Assigned);
            vm.Reopened = ((BugStatus.Reopened & vm.Bug.Status) == BugStatus.Reopened);

            var project = await _projectsApi.GetProjectAsync(vm.Bug.ProjectId).ConfigureAwait(false);
            vm.ProjectName = project.Title;

            return View("EditBugForm", vm);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EditBugVm vm)
        {
            // Summary
            //
            // Validate model, then call API to update bug, then render detail view

            if (!ModelState.IsValid) return View("EditBugForm", vm);

            var bug = vm.Bug;

            if (vm.Assigned) bug.Status |= BugStatus.Assigned;
            if (vm.Reopened) bug.Status |= BugStatus.Reopened;

            var apiResponse = await _bugsApi.PutBugAsync(bug.Id, bug).ConfigureAwait(false);

            if (!apiResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation("Failed to update bug with id {0}", bug.Id);
            }

            // Fetch associated comments from API
            bug = await  _bugsApi.GetBugAsync(bug.Id).ConfigureAwait(false);

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