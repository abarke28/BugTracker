using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BugTracker.Controllers
{
    public class BugsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public BugsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

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
    }
}