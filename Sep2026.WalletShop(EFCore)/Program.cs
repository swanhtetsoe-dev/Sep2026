using Sep2026.Database.Models;
using Sep2026.WalletShop;

AppDbContext db = new AppDbContext();
IWalletService walletService = new WalletService(db);
ISaleService   saleService   = new SaleService(walletService, db);
new WalletShopApp(walletService, saleService).Run();
