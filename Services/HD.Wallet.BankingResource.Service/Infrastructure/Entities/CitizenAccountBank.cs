using System;
using System.Collections.Generic;

namespace HD.Wallet.BankingResource.Service.Infrastructure.Entities;

public partial class CitizenAccountBank
{
    public string AccountNo { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public string IdCardNo { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string Bin { get; set; } = null!;

    public double Balance { get; set; }

    public DateOnly OpenedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual Bank Bank { get; set; } = null!;
}
