﻿using HD.Wallet.Shared;
using HD.Wallet.Shared.BaseDtos;
using HD.Wallet.Shared.Exceptions;
using HD.Wallet.Shared.SharedDtos.Accounts;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;
using IdentityModel.Client;

namespace HD.Wallet.Transaction.Service.ExternalServices
{
    public class AccountExternalService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountExternalService> _logger;

        public AccountExternalService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<AccountExternalService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("AccountExternalService"); ;
            _configuration = configuration;
            _logger = logger;
            _apiBaseUrl = configuration
                .GetSection("AccountExternalApi")
                .GetRequiredValue("ApiBaseUrl");
        }

        public async Task<AccountDto> GetAccountById(string accountId)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "/Account/" + accountId);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(400) || response.StatusCode.Equals(404))
                {
                    return null;
                }
                throw new Exception($"Failed to get account. Status Code: {response.StatusCode}, Response: {errorJson}");
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpResultDto<AccountDto>>(data).Result;
        }

        public async Task<AccountDto> GetWalletAccountByNo(string accountNo)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "/Account/Wallet/" + accountNo);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(400) || response.StatusCode.Equals(404))
                {
                    return null;
                }
                throw new Exception($"Failed to get account. Status Code: {response.StatusCode}, Response: {errorJson}");
            }

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpResultDto<AccountDto>>(data).Result;
        }
    }
}
