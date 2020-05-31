using BugTracker.Models;
using BugTracker.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.utils
{
    public class ProjectsApiService
    {
        public HttpClient Client { get; set; }

        public ProjectsApiService(HttpClient client, IConfiguration config)
        {
            client.BaseAddress = new Uri(config.GetValue<string>("Endpoints:projects"));

            Client = client;
        }

        public async Task<IList<Project>> GetProjectsAsync()
        {
            // Summary
            //
            // Get all projects from Endpoint

            var apiResponse = await Client.GetStringAsync(String.Empty).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Project>>(apiResponse);
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            // Summary
            //
            // Get project by id from Endpoint

            var apiResponse = await Client.GetStringAsync(id.ToString()).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Project>(apiResponse);
        }

        public async Task<HttpResponseMessage> PostProjectAsync(object project)
        {
            // Summary
            //
            // Post project to Endpoint, return HttpResponse
            
            var json = JsonConvert.SerializeObject(project);
            var postContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await Client.PostAsync(String.Empty, postContent).ConfigureAwait(false);
        }
    }
}