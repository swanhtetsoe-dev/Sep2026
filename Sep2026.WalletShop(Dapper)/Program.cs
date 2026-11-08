using Sep2026.WalletShop;

IWalletService walletService = new DapperWalletService();
ISaleService   saleService   = new DapperSaleService(walletService);
new WalletShopApp(walletService, saleService).Run();
