using Newtonsoft.Json;

namespace HD.Wallet.Identity.Service.Dtos
{
    public class UserResponseDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }


        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("sex")]
        public int Sex { get; set; }

        [JsonProperty("idCardNo")]
        public string IdCardNo { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }


        [JsonProperty("placeOfOrigin")]
        public string PlaceOfOrigin { get; set; }

        [JsonProperty("placeOfResidence")] 
        public string PlaceOfResidence { get; set; }

        [JsonProperty("dateOfExpiry")]
        public DateTime DateOfExpiry { get; set; }

        [JsonProperty("frontIdCardUrl")]
        public string FrontIdCardUrl { get; set; }

        [JsonProperty("backIdCardUrl")]
        public string BackIdCardUrl { get; set; }

        [JsonProperty("idCardType")]
        public string IdCardType { get; set; }

        [JsonProperty("isEkycVerfied")]
        public bool IsEkycVerfied { get; set; } = false;

        [JsonProperty("faceVerificationUrl")]
        public string? FaceVerificationUrl { get; set; }

        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
    }
}
