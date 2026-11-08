namespace Oct2026.WebApi.Models
{
    public class WalletCreateRequestModel
    {
        public string Name { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public byte Gender { get; set; }

        public string Color { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

    }
}
