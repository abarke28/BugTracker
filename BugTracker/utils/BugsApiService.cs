using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace BugTracker.utils
{
    public class BugsApiService
    {
        public HttpClient Client { get; set; }

        public BugsApiService(HttpClient client, IConfiguration config)
        {
            client.BaseAddress = new Uri(config.GetValue<string>("Endpoints:bugs"));

            Client = client;
        }
    }
}