using Microsoft.Extensions.Configuration;
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
            client.BaseAddress = new Uri(config.GetSection("Endpoints").GetValue<string>("projects"));

            Client = client;
        }
    }
}
