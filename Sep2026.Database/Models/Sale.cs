using System;
using System.Collections.Generic;

namespace Sep2026.Database.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public int WalletId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string WalletName { get; set; } = null!;

    public int SoldQty { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime SaleDate { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
