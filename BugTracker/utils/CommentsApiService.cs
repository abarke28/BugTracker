﻿using BugTracker.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            client.BaseAddress = new Uri(config.GetValue<string>("Endpoints:comments"));

            Client = client;
        }

        public async Task<IList<Comment>> GetCommentsAsync()
        {
            var apiResponse = await Client.GetStringAsync(String.Empty).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Comment>>(apiResponse);
        }
    }
}
