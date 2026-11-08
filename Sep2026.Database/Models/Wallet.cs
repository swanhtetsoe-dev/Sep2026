using System;
using System.Collections.Generic;

namespace Sep2026.Database.Models;

public partial class Wallet
{
    public int Id { get; set; }

    public string ProductCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public byte Gender { get; set; }

    public string Color { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
