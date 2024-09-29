using HD.Wallet.Account.Service.Infrastructure.Entities.Accounts;
using HD.Wallet.Account.Service.Infrastructure.Entities.Users;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;

namespace HD.Wallet.Account.Service.Infrastructure
{
    public class HdWalletAccountDbContext : DbContext
    {

        public HdWalletAccountDbContext(DbContextOptions<HdWalletAccountDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(x => x.Id);
                entity.OwnsOne(x => x.Address);
                entity.OwnsOne(x => x.Work);
            });

            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.ToTable("Account");
                entity.HasKey(x => x.Id);
                entity.OwnsOne(x => x.AccountBank);
                entity
                    .HasOne(x => x.User)
                    .WithMany(user => user.Accounts)
                    .HasForeignKey(x => x.UserId);
            });
        }
    }
}
