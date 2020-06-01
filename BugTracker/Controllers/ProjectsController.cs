using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.Dtos;
using BugTracker.Models.ViewModels;
using BugTracker.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly ProjectsApiService _projectsApi;

        public ProjectsController(ILogger<ProjectsController> logger, ProjectsApiService projectsApiService)
        {
            _logger = logger;
            _projectsApi = projectsApiService;
        }

        [HttpGet("projects")]
        public async Task<IActionResult> Index()
        {
            // Summary
            //
            // Returns list of all projects, consumes internal API

            var vm = await _projectsApi.GetProjectsAsync();

            return View(vm);
        }

        [HttpGet("projects/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            // Summary
            //
            // Returns Project overview including list of Bugs, consumes internal API

            var vm = await _projectsApi.GetProjectAsync(id);

            return View(vm);
        }

        [HttpGet("projects/new")]
        public IActionResult New()
        {
            // Summary
            //
            // Returns New Project form, distinct from the Edit Project form

            var vm = new NewProjectVm { Project = new Project() };
            return View("NewProjectForm", vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(NewProjectVm vm)
        {
            // Summary
            //
            // Post submitted Project to the internal API, redirect to either index or
            // back to form based on success

            if (!ModelState.IsValid) return View("New", vm);

            var result = await _projectsApi.PostProjectAsync(vm.Project);

            return result.IsSuccessStatusCode ? RedirectToAction("Index") : RedirectToAction("New", vm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Summary
            //
            // Fetch associated project by Id and render view

            var vm = new EditProjectVm
            {
                Project = await _projectsApi.GetProjectAsync(id).ConfigureAwait(false)
            };

            return View("EditProjectForm", vm);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EditProjectVm vm)
        {
            // Summary
            //
            // Validate form submission, call api, then return detail view

            if (!ModelState.IsValid) return View("Edit", vm);

            var project = vm.Project;

            var apiResponse = await _projectsApi.PutProjectAsync(project.Id, project).ConfigureAwait(false);

            if (!apiResponse.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Failed to update Project {project.Id}");
            }

            project = await _projectsApi.GetProjectAsync(project.Id);

            return View("Detail", project);
        }
    }
}