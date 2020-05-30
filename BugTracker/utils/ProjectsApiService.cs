using BugTracker.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var apiResponse = await Client.GetStringAsync(String.Empty).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Project>>(apiResponse);
        }

        public async Task<Project> GetProjectAsync(int id)
        {
            var apiResponse = await Client.GetStringAsync(id.ToString()).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Project>(apiResponse);
        }
    }
}