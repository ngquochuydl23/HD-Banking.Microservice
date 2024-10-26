using HD.Wallet.Transaction.Service.Infrastructure.Transactions;
using Microsoft.EntityFrameworkCore;

namespace HD.Wallet.Transaction.Service.Infrastructure
{
    public class TransactionDbContext : DbContext
    {

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionEntity>(entity =>
            {
                entity.ToTable("Transaction");
                entity.HasKey(x => x.Id);

                entity.OwnsOne(x => x.SourceAccount);
                entity.OwnsOne(x => x.DestAccount);
                entity
                   .Property(e => e.TransactionType)
                   .HasConversion(
                        v => v.ToString(),
                        v => (TransactionTypeEnum)Enum.Parse(typeof(TransactionTypeEnum), v));

                entity
                   .Property(e => e.TransactionStatus)
                   .HasConversion(
                        v => v.ToString(),
                        v => (TransactionStatusEnum)Enum.Parse(typeof(TransactionStatusEnum), v));
            });
        }
    }
}
