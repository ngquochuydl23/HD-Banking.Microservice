using HD.Wallet.Identity.Service.Dtos;
using HD.Wallet.Shared;
using HD.Wallet.Shared.BaseDtos;
using HD.Wallet.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HD.Wallet.Identity.ExternalServices
{
    public class UserExternalService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserExternalService> _logger;
        public UserExternalService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<UserExternalService> logger) 
        {
            _httpClient = httpClientFactory.CreateClient("UserExternalApi"); ;
            _configuration = configuration;
            _logger = logger;
            _apiBaseUrl = configuration
                .GetSection("UserExternalApi")
                .GetRequiredValue("ApiBaseUrl");
        }

        public async Task<UserResponseDto> ValidateUser(string phone, string password)
        {
            string jsonBody = JsonConvert.SerializeObject(new
            {
                PhoneNumber = phone,
                Password = password
            });

    
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_apiBaseUrl + "User/validate", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<HttpErrorDto>(errorJson);

                if (error != null)
                {
                    throw new AppException(error.Error.ToString());
                } else
                {
                    throw new Exception($"Failed to validate user. Status Code: {response.StatusCode}, Response: {errorJson}");
                }
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpResultDto<UserResponseDto>>(data).Result;
        }
    }
}
