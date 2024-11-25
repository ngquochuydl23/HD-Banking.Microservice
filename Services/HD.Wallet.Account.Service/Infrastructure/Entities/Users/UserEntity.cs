using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Shared.Seedworks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HD.Wallet.Account.Service.Infrastructure.Entities.SavedDestinations;

namespace HD.Wallet.Account.Service.Infrastructure.Entities.Users
{
    public class UserEntity : Entity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
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

        public string IdCardType {  get; set; }

        public bool IsEkycVerfied { get; set; } = false;

        public string? FaceVerificationUrl {  get; set; }

        public string? Avatar {  get; set; }

        public string HashPassword { get; set; }

        public string? PinPassword { get; set; }

        public UserStatusEnum AccountStatus {  get; set; } = UserStatusEnum.Active;

        public ICollection<AccountEntity> Accounts { get; set; } = new HashSet<AccountEntity>();

        public ICollection<SavedDestinationEntity> SavedDestinations { get; set; } = new HashSet<SavedDestinationEntity>();

        public ICollection<SavedDestinationEntity> ReferencedUsers { get; set; } = new HashSet<SavedDestinationEntity>();

        public AddressValueObject Address { get; set; }

        public WorkValueObject Work { get; set; }

        public string? CurrentLiving { get; set; }

        public string? MarrigeStatus { get; set; }

        public string? StudyLevel { get; set; }

        public UserEntity() 
        { 
            Id = Guid
                .NewGuid()
                .ToString();
        }
    }
}
