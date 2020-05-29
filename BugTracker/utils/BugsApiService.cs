using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using BugTracker.Models;
using Newtonsoft.Json;

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

        public async Task<IList<Bug>> GetBugsAsync()
        {
            var apiResponse = await Client.GetStringAsync(String.Empty).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Bug>>(apiResponse);
        }
    }
}