using HD.Wallet.Shared.BaseDtos;
using HD.Wallet.Shared.Exceptions;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared.Attributes
{
    public class PinRequiredAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public PinRequiredAttribute(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient() ;
            _configuration = configuration;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
   

            var providedPin = context.HttpContext.Request.Headers["X-EncryptedPin"].ToString();
            var bearerToken = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(providedPin))
            {
                var errorResponse = new
                {
                    Error = "Unauthorized: Missing or invalid PIN.",
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            } else
            {
                var apiBaseUrl = _configuration.GetValue<string>("AccountExternalApi:ApiBaseUrl");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiBaseUrl + "/User/ValidatePinNumber");
                request.Headers.Add("Authorization", bearerToken);
                request.Headers.Add("X-EncryptedPin", providedPin);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                var errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpErrorDto>(errorJson);

                // 500 InternalServerError
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    context.Result = new JsonResult(new
                    {
                        Error = $"Failed to validate user. Status Code: {response.StatusCode}, Response: {errorJson}",
                        StatusCode = StatusCodes.Status400BadRequest
                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }

                // 400 InternalServerError
                if (error != null)
                {
                    context.Result = new JsonResult(new
                    {
                        Error = error.Error.ToString(),
                        StatusCode = StatusCodes.Status400BadRequest
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

          
            }
        }
    }
}
