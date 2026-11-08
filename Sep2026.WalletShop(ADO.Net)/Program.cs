using Sep2026.WalletShop;

IWalletService walletService = new AdoNetWalletService();
ISaleService   saleService   = new AdoNetSaleService(walletService);
new WalletShopApp(walletService, saleService).Run();