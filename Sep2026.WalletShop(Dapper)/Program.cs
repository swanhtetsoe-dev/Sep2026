using Sep2026.WalletShop;

IWalletService walletService = new WalletServiceProxy(new DapperWalletService());
ISaleService   saleService   = new SaleServiceProxy(new DapperSaleService(), walletService);
new WalletShopApp(walletService, saleService).Run();
