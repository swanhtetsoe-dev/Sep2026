namespace Sep2026.WalletShop
{
    /// <summary>
    /// Parameter object used to create a new wallet.
    /// Contains only the user-supplied fields; the system
    /// generates <c>Id</c>, <c>ProductCode</c>, and <c>IsDeleted</c>.
    /// </summary>
    public class WalletCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
