using Newtonsoft.Json;

namespace HD.Wallet.Account.Service.Dtos.IdCards
{
    public class ExternalResponseIdCardDto
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("id_card")]
        public IdCardDto IdCard { get; set; }

        [JsonProperty("back-url")]
        public string BackUrl { get; set; }

        [JsonProperty("front-url")]
        public string FrontUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
