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
using Newtonsoft.Json;

namespace BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ProjectsApiService _projectsApi;

        public ProjectsController(ProjectsApiService projectsApiService)
        {
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
    }
}