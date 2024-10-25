using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using HD.Wallet.BankingResource.Service.Infrastructure.Entities;

namespace HD.Wallet.BankingResource.Service.Infrastructure;

public partial class BankingResourceDbContext : DbContext
{
    public BankingResourceDbContext()
    {
    }

    public BankingResourceDbContext(DbContextOptions<BankingResourceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<CitizenAccountBank> CitizenAccountBanks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Bin).HasName("PRIMARY");

            entity.Property(e => e.Bin).HasMaxLength(10);
            entity.Property(e => e.AndroidAppId).HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.LogoApp).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.Top).HasColumnType("int(11)");
            entity.Property(e => e._).HasColumnName("#");

            entity
                .HasIndex(a => new { a.Name, a.ShortName , a.Bin})
                .HasMethod("FULLTEXT");

        });

        modelBuilder.Entity<CitizenAccountBank>(entity =>
        {
            entity.HasKey(e => e.AccountNo).HasName("PRIMARY");

            entity.ToTable("CitizenAccountBank");

            entity.HasIndex(e => e.Bin, "FK_CitizenAccountBank_Banks");

            entity.Property(e => e.AccountNo).HasMaxLength(11);
            entity.Property(e => e.Balance).HasPrecision(15, 2);
            entity.Property(e => e.BankName).HasMaxLength(50);
            entity.Property(e => e.Bin).HasMaxLength(10);
            entity.Property(e => e.IdCardNo).HasMaxLength(12);
            entity.Property(e => e.OwnerName).HasMaxLength(100);
            entity.Property(e => e.Status).HasColumnType("enum('Active','Inactive','Closed','Frozen')");

            entity.HasOne(d => d.Bank).WithMany(p => p.CitizenAccountBanks)
                .HasForeignKey(d => d.Bin)
                .HasConstraintName("FK_CitizenAccountBank_Banks");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
