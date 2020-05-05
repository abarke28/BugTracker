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
        private readonly IHttpClientFactory _clientFactory;

        public ProjectsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET /projects
        [HttpGet("projects")]
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("projects");
            var apiResponse = await client.GetAsync("").Result.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<List<Project>>(apiResponse);

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var client = _clientFactory.CreateClient("projects");
            var apiResponse = await client.GetAsync(id.ToString()).Result.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<Project>(apiResponse);

            return View(vm);
        }
    }
}