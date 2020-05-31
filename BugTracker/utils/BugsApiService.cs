using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using BugTracker.Models;
using Newtonsoft.Json;
using System.Text;

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
            // Summary
            //
            // Get all bugs from Endpoint

            var apiResponse = await Client.GetStringAsync(String.Empty).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Bug>>(apiResponse);
        }

        public async Task<Bug> GetBugAsync(int id)
        {
            // Summary
            //
            // Get single bug from Endpoint by id

            var apiResponse = await Client.GetStringAsync(id.ToString()).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Bug>(apiResponse);
        }

        public async Task<HttpResponseMessage> PostBugAsync(object bug)
        {
            // Summary
            //
            // Post supplied object to Endpoint, return the response

            var json = JsonConvert.SerializeObject(bug);
            var postContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await Client.PostAsync(String.Empty, postContent);
        }
    }
}