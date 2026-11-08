namespace Sep2026.WalletShop
{
    public interface ISaleService
    {
        void          SellWallet(WalletDto wallet, int qty);
        List<SaleDto> GetSalesReport();
        void          ShowSalesReport();
    }
}
