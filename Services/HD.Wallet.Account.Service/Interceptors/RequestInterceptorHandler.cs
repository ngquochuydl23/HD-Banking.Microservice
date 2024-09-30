using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Interceptors
{
    public class RequestInterceptorHandler : DelegatingHandler
    {
        private readonly ILogger<RequestInterceptorHandler> _logger;

        public RequestInterceptorHandler(ILogger<RequestInterceptorHandler> logger)
        {
            _logger = logger;
        }

        // Intercept the request
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Add custom logic before the request is sent (e.g., add headers)
            _logger.LogInformation($"Sending request to {request.RequestUri}");

            // Example: Add a custom header to all outgoing requests
            request.Headers.Add("X-Custom-Header", "MyCustomValue");

            // Call the base handler to continue with the request
            var response = await base.SendAsync(request, cancellationToken);

            if (response.Content != null && response.Content.Headers.ContentType.MediaType == "application/json")
            {


                string modifiedResponseBody;
                string responseBody = await response.Content.ReadAsStringAsync();
                JToken resultToken = JObject.Parse(responseBody)["result"];

                if (resultToken is JObject resultObject)
                {
                    modifiedResponseBody = resultObject.ToString();
                    response.Content = new StringContent(modifiedResponseBody, Encoding.UTF8, "application/json");
                }
                else if (resultToken is JArray resultArray)
                {
                    modifiedResponseBody = resultArray.ToString();
                    response.Content = new StringContent(modifiedResponseBody, Encoding.UTF8, "application/json");
                }
            }
            return response;
        }
    }
}
