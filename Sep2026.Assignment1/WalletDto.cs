namespace Sep2026.Assignment1
{
    public class WalletDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
