using System;
using System.Collections.Generic;

namespace HD.Wallet.BankingResource.Service.Infrastructure.Entities;

public partial class Bank
{
    public string ShortName { get; set; } = null!;

    public string? AndroidAppId { get; set; }

    public string? LogoApp { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Bin { get; set; } = null!;

    public int Top { get; set; }

    public double? _ { get; set; }

    public virtual ICollection<CitizenAccountBank> CitizenAccountBanks { get; set; } = new List<CitizenAccountBank>();
}
