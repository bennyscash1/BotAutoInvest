using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.ApiTest.HttpService
{
    public class HttpServiceResult<T>
    {
        public HttpServiceResult(HttpStatusCode httpStatus, T result, string? body = null)
        {
            HttpStatus = httpStatus;

            Result = result;

            BodyString = body ?? "";
        }

        public HttpStatusCode HttpStatus { get; private set; }

        public T Result { get; private set; }
        public string BodyString { get; set; }

    }
}
