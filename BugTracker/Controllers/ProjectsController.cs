﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Models.Dtos;
using BugTracker.Models.ViewModels;
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

        [HttpGet("projects")]
        public async Task<IActionResult> Index()
        {
            var http = _clientFactory.CreateClient("projects");
            var apiResponse = await http.GetAsync(String.Empty).Result.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<List<Project>>(apiResponse);

            return View(vm);
        }

        [HttpGet("projects/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var http = _clientFactory.CreateClient("projects");
            var apiResponse = await http.GetAsync(id.ToString()).Result.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<Project>(apiResponse);

            return View(vm);
        }

        [HttpGet("projects/new")]
        public IActionResult New()
        {
            return View("NewProjectForm");
        }
    }
}