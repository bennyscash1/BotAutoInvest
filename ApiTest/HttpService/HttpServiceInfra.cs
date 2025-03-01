using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ComprehensivePlayrightAuto.ApiTest.HttpService
{

    public class HttpServiceInfra : IDisposable
    {
        #region Data Members
        private readonly HttpClient m_HttpClient;
        public HttpClient HttpClient => m_HttpClient;
        private readonly HttpServiceOptions m_Options;
        #endregion

        #region Constructor
        public HttpServiceInfra(HttpServiceOptions options)
            : this(options, TimeSpan.FromSeconds(25))
        {
        }

        public HttpServiceInfra(HttpServiceOptions options, TimeSpan timeout)
        {
            m_HttpClient = new HttpClient();
            m_HttpClient.Timeout = timeout;

            m_Options = options;
        }
        #endregion

        #region Http Calls
        private HttpRequestMessage CreateRequest(HttpCallOptionsBase options, HttpMethod method)
        {
            string finalUrl = BuildUrl(options);

            var requestMessage = new HttpRequestMessage(method, finalUrl);

            foreach (KeyValuePair<string, string> headerItem in options.Headers)
            {
                requestMessage.Headers.Add(headerItem.Key, headerItem.Value);
            }

            if (!string.IsNullOrEmpty(options.Token))
            {
                requestMessage.Headers.Add("Authorization", "Bearer " + options.Token);

                //requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer ", options.Token);
            }

            return requestMessage;
        }

        private string BuildUrl(HttpCallOptionsBase options)
        {
            if (!options.IsAppended)
            {
                return options.Url;
            }

            Uri uri = new(new Uri(m_Options.BaseUrl), options.Url);

            return uri.ToString();
        }


        private async Task<HttpServiceResult<TResult>> HandleCall<TResult>(HttpRequestMessage request)
        {
            var response = await m_HttpClient.SendAsync(request);
            var data = await response?.Content.ReadAsStringAsync();

            TResult dataObj = default; // Initialize with a default value

            try
            {
                dataObj = JsonConvert.DeserializeObject<TResult>(data);
            }
            catch (JsonException ex)
            {
                // Handle the exception, log it, or take appropriate action
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }

            return new HttpServiceResult<TResult>(response.StatusCode, dataObj, data);
        }


        public async Task<HttpServiceResult<TResult>> CallWithoutBody<TResult>(HttpCallOptionsSimple options)
        {
            HttpMethod FindMethod()
            {
                switch (options.Method)
                {
                    case HttpCallMethod.Put:
                        return HttpMethod.Put;

                    case HttpCallMethod.Post:
                        return HttpMethod.Post;

                    case HttpCallMethod.Delete:
                        return HttpMethod.Delete;

                    case HttpCallMethod.Get:
                        return HttpMethod.Get;

                    default:
                        throw new Exception($"Cannot Find Method: Method {options.Method} is unknown");

                }
            }

            var method = FindMethod();

            using var request = CreateRequest(options, method);

            return await HandleCall<TResult>(request);
        }

        public async Task<HttpServiceResult<TResult>> CallWithBody<TBody, TResult>(
            TBody body,
            HttpCallOptionsBody options,
            HttpCallMethod method = HttpCallMethod.Post)
        {
            // Use the provided method directly
            HttpMethod httpMethod = method == HttpCallMethod.Put
                ? HttpMethod.Put
                : HttpMethod.Post;

            using var request = CreateRequest(options, httpMethod);

            string json = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await HandleCall<TResult>(request);
        }

        public void Dispose()
        {
            m_HttpClient.Dispose();
        }
        #endregion
    }
}
