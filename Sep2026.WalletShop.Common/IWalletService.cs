namespace Sep2026.WalletShop
{
    public interface IWalletService
    {
        List<WalletDto>  GetAllWallets();
        void             AddWallet(WalletCreateDto wallet);
        WalletDto?       FindWallet(string nameOrCode);
        void             EditWallet(WalletEditDto wallet);
        void             DeleteWallet(int id);
        bool             DeductStock(WalletDto wallet, int qty);
        void             ShowWallets();
    }
}
