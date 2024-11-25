using System.ComponentModel.DataAnnotations;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;

namespace HD.Wallet.Account.Service.Dtos.Users
{
    public class UserDto
    {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Sex { get; set; }

        public string IdCardNo { get; set; }

        public string Nationality { get; set; }

        public string PlaceOfOrigin { get; set; }

        public string PlaceOfResidence { get; set; }

        public DateTime DateOfExpiry { get; set; }

        public string FrontIdCardUrl { get; set; }

        public string BackIdCardUrl { get; set; }

        public string IdCardType { get; set; }

        public bool IsEkycVerfied { get; set; } = false;

        public string? FaceVerificationUrl { get; set; }

        public string? Avatar { get; set; }

        public WorkValueObject Work {  get; set; }
    }
}
