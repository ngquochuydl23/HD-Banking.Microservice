using HD.Wallet.Account.Service.Dtos.IdCards;
using HD.Wallet.Shared;
using HD.Wallet.Shared.BaseDtos;
using HD.Wallet.Shared.SharedDtos.Banks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HD.Wallet.Account.Service.ExternalServices
{
    public class BankExternalService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdCardExternalService> _logger;
        public BankExternalService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<IdCardExternalService> logger) 
        {
            _httpClient = httpClientFactory.CreateClient(); ;
            _configuration = configuration;
            _logger = logger;
            _apiBaseUrl = configuration
                .GetSection("BankingResourceExternalApi")
                .GetRequiredValue("ApiBaseUrl");
        }

        public async Task<BankDto> GetBankByBin(string bin)
        {
            _logger.LogInformation("Sending request to retrieve bank with bin: {bin}", bin);

            HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl + "/Bank/" + bin);

            if (!response.IsSuccessStatusCode)
            {

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error retrieving bank. Status Code: {StatusCode}, Response: {ErrorContent}", response.StatusCode, errorContent);

                throw new Exception($"Failed to retrieve bank. Status Code: {response.StatusCode}, Response: {errorContent}");
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpResultDto<BankDto>>(data).Result;
        }
    }
}
