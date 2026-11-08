namespace Sep2026.WalletShop
{
    public interface ISaleService
    {
        void SellWallet(WalletDto wallet, int qty);
        void ShowSalesReport();
    }
}
