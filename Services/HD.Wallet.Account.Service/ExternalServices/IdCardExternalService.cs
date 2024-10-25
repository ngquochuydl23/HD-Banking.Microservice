using HD.Wallet.Account.Service.Dtos.IdCards;
using HD.Wallet.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HD.Wallet.Account.Service.ExternalServices
{
    public class IdCardExternalService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdCardExternalService> _logger;
        public IdCardExternalService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<IdCardExternalService> logger) 
        {
            _httpClient = httpClientFactory.CreateClient("IdCardExternalApi"); ;
            _configuration = configuration;
            _logger = logger;
            _apiBaseUrl = configuration
                .GetSection("IdCardExternalApi")
                .GetRequiredValue("ApiBaseUrl");
        }

        public async Task<ExternalResponseIdCardDto> GetIdCardById(string idCardNo)
        {
            _logger.LogInformation("Sending request to retrieve ID card with number: {IdCardNo}", idCardNo);

            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "/id-card/" + idCardNo);

            if (!response.IsSuccessStatusCode)
            {

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error retrieving ID card. Status Code: {StatusCode}, Response: {ErrorContent}", response.StatusCode, errorContent);

                throw new Exception($"Failed to retrieve IdCard. Status Code: {response.StatusCode}, Response: {errorContent}");
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExternalResponseIdCardDto>(data);
        }
    }
}
