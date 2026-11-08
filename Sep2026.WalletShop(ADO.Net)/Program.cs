using Sep2026.WalletShop;

IWalletService walletService = new WalletServiceProxy(new AdoNetWalletService());
ISaleService   saleService   = new SaleServiceProxy(new AdoNetSaleService(walletService));
new WalletShopApp(walletService, saleService).Run();
