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
    public class BugsController : Controller
    {
        [HttpGet("projects/detail/{projectId}/{id}")]   
        public async Task<IActionResult> Detail(int id)
        {
            var vm = new Bug();

            using (var client = new HttpClient())
            {
                var apiResponse = await client.GetAsync(Api.BugsController.Endpoint + @"/" + id).Result.Content.ReadAsStringAsync();
                vm = JsonConvert.DeserializeObject<Bug>(apiResponse);
            }

            return View(vm);
        }
    }
}