using Sep2026.Database.Models;
using Sep2026.WalletShop;

AppDbContext    db            = new AppDbContext();
IWalletService walletService = new WalletServiceProxy(new WalletService(db));
ISaleService   saleService   = new SaleServiceProxy(new SaleService(db), walletService);
new WalletShopApp(walletService, saleService).Run();
