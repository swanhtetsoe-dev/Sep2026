using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class DapperWalletService : IWalletService
    {
        public List<WalletDto> GetAllWallets()
        {
            List<WalletDto> wallets = new List<WalletDto>();

            using var conn = DbHelper.GetOpenConnection();
            try
            {
                wallets = conn.Query<WalletDto>(WalletQueries.SelectAll).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return wallets;
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                int maxId = conn.ExecuteScalar<int>(WalletQueries.GetMaxId);
                int newId = maxId + 1;

                string productCode = $"WAL-{newId:D4}";

                var param = new
                {
                    ProductCode   = productCode,
                    Name          = wallet.Name,
                    Brand         = wallet.Brand,
                    Gender        = (byte)wallet.Gender,
                    Color         = wallet.Color,
                    Price         = wallet.Price,
                    StockQuantity = wallet.StockQuantity
                };

                conn.Execute(WalletQueries.Insert, param);

                Console.WriteLine($"Wallet added successfully. Product Code: {productCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                var param = new { Search = $"%{nameOrCode}%" };

                return conn.QueryFirstOrDefault<WalletDto>(WalletQueries.FindByNameOrCode, param);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public void EditWallet(WalletEditDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                var param = new
                {
                    Name          = wallet.Name,
                    Brand         = wallet.Brand,
                    Gender        = (byte)wallet.Gender,
                    Color         = wallet.Color,
                    Price         = wallet.Price,
                    StockQuantity = wallet.StockQuantity,
                    Id            = wallet.Id
                };

                conn.Execute(WalletQueries.Update, param);

                Console.WriteLine("Wallet updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void DeleteWallet(int id)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                var param = new { Id = id };

                conn.Execute(WalletQueries.SoftDelete, param);

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

            using var conn = DbHelper.GetOpenConnection();
            try
            {
                var param = new { Qty = qty, Id = wallet.Id };

                conn.Execute(WalletQueries.DeductStock, param);

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
    }
}
