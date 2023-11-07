using System.Net.Http.Headers;

namespace Postex.SharedKernel.Common
{
    public static class HttpClientUtilities
    {
        public static HttpClient SetHttpClient(string baseAddress, string token = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            //string token = GetToken().Result;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
            client.DefaultRequestHeaders
                         .Accept
                         .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
