using Microsoft.EntityFrameworkCore;
using Sep2026.Database.Models;

namespace Sep2026.WalletShop
{
    public class WalletService : IWalletService
    {
        private readonly AppDbContext _db;

        public WalletService(AppDbContext db)
        {
            _db = db;
        }

        public List<WalletDto> GetAllWallets()
        {
            return _db.Wallets
                .Where(w => !w.IsDeleted)
                .Select(w => ToDto(w))
                .ToList();
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            int    maxId      = _db.Wallets.Any() ? _db.Wallets.Max(w => w.Id) : 0;
            string productCode = $"WAL-{maxId + 1:D4}";

            Wallet entity = new Wallet
            {
                ProductCode   = productCode,
                Name          = wallet.Name,
                Brand         = wallet.Brand,
                Gender        = (byte)wallet.Gender,
                Color         = wallet.Color,
                Price         = wallet.Price,
                StockQuantity = wallet.StockQuantity,
                IsDeleted     = false
            };

            _db.Wallets.Add(entity);
            _db.SaveChanges();
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            Wallet? wallet = _db.Wallets
                .Where(w => !w.IsDeleted)
                .Where(w => w.ProductCode.Contains(nameOrCode) || w.Name.Contains(nameOrCode))
                .FirstOrDefault();

            return wallet == null ? null : ToDto(wallet);
        }

        public void EditWallet(WalletEditDto wallet)
        {
            Wallet? entity = _db.Wallets
                .FirstOrDefault(w => w.Id == wallet.Id && !w.IsDeleted);

            if (entity == null)
                throw new InvalidOperationException("Wallet not found.");

            entity.Name          = wallet.Name;
            entity.Brand         = wallet.Brand;
            entity.Gender        = (byte)wallet.Gender;
            entity.Color         = wallet.Color;
            entity.Price         = wallet.Price;
            entity.StockQuantity = wallet.StockQuantity;

            _db.SaveChanges();
        }

        public void DeleteWallet(int id)
        {
            Wallet? entity = _db.Wallets.FirstOrDefault(w => w.Id == id);

            if (entity == null)
                throw new InvalidOperationException("Wallet not found.");

            entity.IsDeleted = true;
            _db.SaveChanges();
        }

        public bool DeductStock(WalletDto wallet, int qty)
        {
            if (!wallet.HasSufficientStock(qty))
                return false;

            Wallet? entity = _db.Wallets.FirstOrDefault(w => w.Id == wallet.Id);

            if (entity == null)
                return false;

            entity.StockQuantity -= qty;
            _db.SaveChanges();

            wallet.StockQuantity -= qty;
            return true;
        }

        public void ShowWallets()
        {
            List<WalletDto> wallets = GetAllWallets();

            if (wallets.Count == 0)
            {
                Console.WriteLine("No wallets available.");
                return;
            }

            Console.WriteLine("\n--- Wallet List ---");
            foreach (WalletDto item in wallets)
            {
                Console.WriteLine(
                    $"[{item.ProductCode}] {item.Name} | Brand: {item.Brand} | " +
                    $"Gender: {item.Gender} | Color: {item.Color} | " +
                    $"Price: {item.Price.ToCurrency()} | Stock: {item.StockQuantity}");
            }
            Console.WriteLine("-------------------");
        }

        private static WalletDto ToDto(Wallet w) => new WalletDto
        {
            Id            = w.Id,
            ProductCode   = w.ProductCode,
            Name          = w.Name,
            Brand         = w.Brand,
            Gender        = (GenderType)w.Gender,
            Color         = w.Color,
            Price         = w.Price,
            StockQuantity = w.StockQuantity,
            IsDeleted     = w.IsDeleted
        };
    }
}
