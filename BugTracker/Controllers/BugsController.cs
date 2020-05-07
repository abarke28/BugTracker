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

namespace BugTracker.Controllers
{
    [Authorize]
    public class BugsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public BugsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET: /projects/detail/projectId/id
        [HttpGet("projects/detail/{projectId}/{id}")]   
        public async Task<IActionResult> Detail(int projectId, int id)
        {
            var vm = new BugDetailVm 
            { 
                Bug = new Bug(),
                ProjectId = projectId
            };

            var httpClient = _clientFactory.CreateClient("bugs");
            var apiResponse = await httpClient.GetAsync(id.ToString()).Result.Content.ReadAsStringAsync();
            vm.Bug = JsonConvert.DeserializeObject<Bug>(apiResponse);

            httpClient = _clientFactory.CreateClient("projects");
            apiResponse = await httpClient.GetAsync(projectId.ToString()).Result.Content.ReadAsStringAsync();
            vm.ProjectName = JsonConvert.DeserializeObject<Project>(apiResponse).Title;

            return View(vm);
        }

        // GET: /bugs/new/projectId/projectName
        [HttpGet("bugs/new/{projectId}/{projectName}")]
        public IActionResult New(int projectId, string projectName)
        {
            var vm = new NewBugVm { Bug = new BugDto() };
            vm.Bug.ProjectId = projectId;
            vm.ProjectName = projectName;

            return View("NewBugForm", vm);
        }

        // POST: /bugs
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

            var statuses = Enum.GetValues(typeof(BugStatus)).Cast<BugStatus>();
            var selectList = new List<SelectListItem>();

            foreach (var status in statuses)
            {
                selectList.Add(new SelectListItem(status.ToString(), status.ToString()));
            }

            vm.Statuses = selectList;

            var http = _clientFactory.CreateClient("bugs");
            var apiResponse = await http.GetAsync(id.ToString()).Result.Content.ReadAsStringAsync();
            vm.Bug = JsonConvert.DeserializeObject<Bug>(apiResponse);

            http = _clientFactory.CreateClient("projects");
            apiResponse = await http.GetAsync(vm.Bug.ProjectId.ToString()).Result.Content.ReadAsStringAsync();
            vm.ProjectName = JsonConvert.DeserializeObject<Project>(apiResponse).Title;

            return View("EditBugForm", vm);
        }
    }
}