namespace Sep2026.WalletShop
{
    public class SaleDto
    {
        public int SaleId { get; set; }
        public int WalletId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string WalletName { get; set; } = string.Empty;
        public int SoldQty { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
