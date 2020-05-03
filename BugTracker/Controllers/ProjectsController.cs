using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        // GET /projects
        public async Task<IActionResult> Index()
        {
            var vm = new List<Project>();

            using (var client = new HttpClient())
            {
                string apiResponse = await client.GetAsync(Api.ProjectsController.Endpoint).Result.Content.ReadAsStringAsync();
                vm = JsonConvert.DeserializeObject<List<Project>>(apiResponse);
            };

            return View(vm);
        }

        public IActionResult Detail(Project project)
        {
            return View(project);
        }
    }
}