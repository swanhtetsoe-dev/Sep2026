namespace Oct2026.WebApi.Models
{
    public class SaleResponseModel
    {
        public int SaleId { get; set; }

        public int WalletId { get; set; }

        public string ProductCode { get; set; } = null!;

        public string WalletName { get; set; } = null!;

        public int SoldQty { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime SaleDate { get; set; }
    }
}
