﻿namespace HD.Wallet.Account.Service.Dtos
{
    public class RequestOpenAccountDto
    {
        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string IdCardNo { get; set; }

        public string? FaceVerificationUrl { get; set; }

        public string Password { get; set; }

        public AddressDto? Address { get; set; }
    }
}
