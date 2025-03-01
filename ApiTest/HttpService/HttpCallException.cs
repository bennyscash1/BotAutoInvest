using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.ApiTest.HttpService
{
    public class HttpCallException : Exception
    {
        public HttpCallException(HttpStatusCode httpCode, string description)
        {
            HttpCode = httpCode;

            Description = description;
        }

        public HttpStatusCode HttpCode { get; private set; }

        public string Description { get; set; }
    }
}
