using Newtonsoft.Json;

namespace HD.Wallet.Account.Service.Dtos.IdCards
{
    public class IdCardDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("place_of_origin")]
        public string PlaceOfOrigin { get; set; }

        [JsonProperty("place_of_residence")]
        public string PlaceOfResidence { get; set; }

        [JsonProperty("date_of_expiry")]
        public string DateOfExpiry { get; set; }
    }
}
