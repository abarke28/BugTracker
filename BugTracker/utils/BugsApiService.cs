using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BugTracker.utils
{
    public class BugsApiService
    {
        public HttpClient Client { get; set; }

        public BugsApiService(HttpClient client)
        {
            client.BaseAddress = new Uri(@"https://localhost:44313/api/bugs/");

            Client = client;
        }
    }
}
