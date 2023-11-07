using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Postex.Payment.Infrastructure.PaymentMethods.Jibit
{
    public class RestTools
    {
        public async Task<ResponseWarpper<T>> SendRequest<T>(
            object model,
            string baseUrl,
            string endpointUrl,
            Method method,
            IDictionary<string, string> HeaderItem)
        {
            RestClient _client = new RestClient(baseUrl);
            var request = new RestRequest(endpointUrl);
            if (model != null)
            {
                string jsonToSend = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                request.AddParameter("application/json", jsonToSend, ParameterType.RequestBody);
                request.Method = method;
            }
            if (HeaderItem != null)
            {
                foreach (var item in HeaderItem)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            request.RequestFormat = DataFormat.Json;
            try
            {
                var reposnce = _client.Execute(request);
                if (reposnce.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrEmpty(reposnce.Content))
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(reposnce.Content);
                        return new ResponseWarpper<T>()
                        {
                            success = true,
                            message = "",
                            data = result
                        };
                    }
                }
                return new ResponseWarpper<T>()
                {
                    success = false,
                    message = reposnce.StatusCode.ToString(),
                    data = default(T)
                };
            }
            catch (Exception ex)
            {
                return new ResponseWarpper<T>()
                {
                    success = false,
                    message = ex.Message,
                    data = default(T)
                };
            }

        }
        public async Task<ResponseWarpper> SendRequest(
            object model,
            string baseUrl,
            string endpointUrl,
            Method method,
            IDictionary<string, string> HeaderItem)
        {
            RestClient _client = new RestClient(baseUrl);
            var request = new RestRequest(endpointUrl);
            if (model != null)
            {
                string jsonToSend = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                request.AddParameter("application/json", jsonToSend, ParameterType.RequestBody);
                request.Method = method;
            }
            if (HeaderItem != null)
            {
                foreach (var item in HeaderItem)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            request.RequestFormat = DataFormat.Json;
            try
            {
                var response = _client.Execute(request);
                return new ResponseWarpper()
                {
                    success = response.StatusCode == HttpStatusCode.OK,
                    message = "",
                    data = response?.Content
                };
            }
            catch (Exception ex)
            {
                return new ResponseWarpper()
                {
                    success = false,
                    message = ex.Message,
                    data = ""
                };
            }
        }
        public class ResponseWarpper<T>
        {
            public bool success { get; set; }
            public string message { get; set; }
            public T data { get; set; }
        }
        public class ResponseWarpper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }
    }
}
