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
            List<WalletDto> wallets = new List<WalletDto>();

            try
            {
                wallets = _db.Wallets
                    .Where(w => !w.IsDeleted)
                    .Select(w => ToDto(w))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return wallets;
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            try
            {
                int maxId = _db.Wallets.Any() ? _db.Wallets.Max(w => w.Id) : 0;
                int newId = maxId + 1;

                string productCode = $"WAL-{newId:D4}";

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

                Console.WriteLine($"Wallet added successfully. Product Code: {productCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            try
            {
                Wallet? wallet = _db.Wallets
                    .Where(w => !w.IsDeleted)
                    .Where(w => w.ProductCode.Contains(nameOrCode) || w.Name.Contains(nameOrCode))
                    .FirstOrDefault();

                return wallet == null ? null : ToDto(wallet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public void EditWallet(WalletEditDto wallet)
        {
            try
            {
                Wallet? entity = _db.Wallets
                    .FirstOrDefault(w => w.Id == wallet.Id && !w.IsDeleted);

                if (entity == null)
                {
                    Console.WriteLine("Wallet not found.");
                    return;
                }

                entity.Name          = wallet.Name;
                entity.Brand         = wallet.Brand;
                entity.Gender        = (byte)wallet.Gender;
                entity.Color         = wallet.Color;
                entity.Price         = wallet.Price;
                entity.StockQuantity = wallet.StockQuantity;

                _db.SaveChanges();

                Console.WriteLine("Wallet updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void DeleteWallet(int id)
        {
            try
            {
                Wallet? entity = _db.Wallets.FirstOrDefault(w => w.Id == id);

                if (entity == null)
                {
                    Console.WriteLine("Wallet not found.");
                    return;
                }

                entity.IsDeleted = true;
                _db.SaveChanges();

                Console.WriteLine("Wallet deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public bool DeductStock(WalletDto wallet, int qty)
        {
            if (!HasSufficientStock(wallet, qty))
            {
                Console.WriteLine(
                    $"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
                return false;
            }

            try
            {
                Wallet? entity = _db.Wallets.FirstOrDefault(w => w.Id == wallet.Id);

                if (entity == null)
                {
                    Console.WriteLine("Wallet not found.");
                    return false;
                }

                entity.StockQuantity -= qty;
                _db.SaveChanges();

                wallet.StockQuantity -= qty;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
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

        private static bool HasSufficientStock(WalletDto wallet, int qty)
            => wallet.StockQuantity > 0 && wallet.StockQuantity >= qty;

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
