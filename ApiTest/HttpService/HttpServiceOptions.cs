using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.ApiTest.HttpService
{
    public class HttpServiceOptions
    {
        public string BaseUrl { get; set; }
    }

    public enum HttpMethodBody
    {
        Post,
        Put
    }

    public enum HttpCallMethod
    {
        Get,
        Delete,
        Post,
        Put
    }

    public class HttpCallOptionsBase
    {
        public HttpCallOptionsBase(string url, string token = null)
        {
            Url = url;

            Headers = new();

            Token = token;
        }

        public string Url { get; private set; }

        public bool IsAppended { get; set; } = true;

        public string Token { get; private set; }

        public Dictionary<string, string> Headers { get; set; }
    }

    public class HttpCallOptionsBody : HttpCallOptionsBase
    {
        public HttpCallOptionsBody(string url, string token = null)
            : base(url, token)
        {

        }

        public HttpMethodBody Method { get; set; } = HttpMethodBody.Post;

        public HttpCallOptionsBody WithHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }
    }

    public class HttpCallOptionsSimple : HttpCallOptionsBase
    {
        public HttpCallOptionsSimple(string url, string token = null)
            : base(url, token)
        {

        }

        public HttpCallMethod Method { get; set; }
        public HttpCallOptionsSimple WithHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }
    }
}
