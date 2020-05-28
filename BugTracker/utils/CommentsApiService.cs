using BugTracker.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BugTracker.utils
{
    public class CommentsApiService
    {
        public HttpClient Client { get; set; }

        public CommentsApiService(HttpClient client, IConfiguration config)
        {
            client.BaseAddress = new Uri(config.GetSection("Endpoints").GetValue<string>("comments"));

            Client = client;
        }
    }
}
