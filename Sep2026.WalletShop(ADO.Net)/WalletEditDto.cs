namespace Sep2026.WalletShop
{
    /// <summary>
    /// Parameter object used to edit an existing wallet.
    /// Carries the target <c>Id</c> together with the
    /// user-supplied fields that should be updated.
    /// </summary>
    public class WalletEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
