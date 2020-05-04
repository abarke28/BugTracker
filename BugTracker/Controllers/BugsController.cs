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
        [HttpGet("projects/detail/{projectId}/{id}")]   
        public async Task<IActionResult> Detail(int projectId, int id)
        {
            var vm = new BugDetailVm 
            { 
                Bug = new Bug(),
                ProjectId = projectId
            };

            using (var client = new HttpClient())
            {
                var apiResponse = await client.GetAsync(Api.BugsController.Endpoint + @"/" + id).Result.Content.ReadAsStringAsync();
                vm.Bug = JsonConvert.DeserializeObject<Bug>(apiResponse);

                apiResponse = await client.GetAsync(Api.ProjectsController.Endpoint + @"/" + projectId).Result.Content.ReadAsStringAsync();
                vm.ProjectName = JsonConvert.DeserializeObject<Project>(apiResponse).Title;
            }

            return View(vm);
        }
    }
}