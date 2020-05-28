using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.utils
{
    public static class Extensions
    {
        /// <summary>
        /// Extension: Send a POST request to the specified Uri as an asynchronous operation.
        /// Content is serialized as Json with UTF8 encoding.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient http, string requestUri, object content)
        {
            var objectJson = JsonConvert.SerializeObject(content);
            var contentStr = new StringContent(objectJson, Encoding.UTF8, "application/json");

            return http.PostAsync(requestUri, contentStr);
        }

        /// <summary>
        /// Extension: Send a PUT request to teh specified Uri as an asynchronous operation.
        /// Content is serialized as Json with UTF8 encoding.
        /// </summary>
        /// <param name="http"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PutAsync(this HttpClient http, string requestUri, object content)
        {
            var objectJson = JsonConvert.SerializeObject(content);
            var contentStr = new StringContent(objectJson, Encoding.UTF8, "application/json");

            return http.PutAsync(requestUri, contentStr);
        }

        /// <summary>
        /// Extension: Enumerates an Enum to a IEnumerable
        /// </summary>
        public static IEnumerable<T> Enumerate<T>(this T sourceEnum) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Method not valid for {0}, only valid for Enums",sourceEnum));

            return Enum.GetValues(typeof(T)).Cast<T>();
        } 
    }
}