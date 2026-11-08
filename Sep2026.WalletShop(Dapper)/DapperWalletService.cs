using Dapper;

namespace Sep2026.WalletShop
{
    public class DapperWalletService : IWalletService
    {
        public List<WalletDto> GetAllWallets()
        {
            using var conn = DbHelper.GetOpenConnection();
            return conn.Query<WalletDto>(WalletQueries.SelectAll).ToList();
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();

            int newId = conn.ExecuteScalar<int>(WalletQueries.GetMaxId) + 1;

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
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            using var conn = DbHelper.GetOpenConnection();

            var param = new { Search = $"%{nameOrCode}%" };
            return conn.QueryFirstOrDefault<WalletDto>(WalletQueries.FindByNameOrCode, param);
        }

        public void EditWallet(WalletEditDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();

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

            int rows = conn.Execute(WalletQueries.Update, param);

            if (rows == 0)
                throw new InvalidOperationException("Wallet not found.");
        }

        public void DeleteWallet(int id)
        {
            using var conn = DbHelper.GetOpenConnection();

            int rows = conn.Execute(WalletQueries.SoftDelete, new { Id = id });

            if (rows == 0)
                throw new InvalidOperationException("Wallet not found.");
        }

        public bool DeductStock(WalletDto wallet, int qty)
        {
            if (wallet.StockQuantity <= 0 || wallet.StockQuantity < qty)
                return false;

            using var conn = DbHelper.GetOpenConnection();

            conn.Execute(WalletQueries.DeductStock, new { Qty = qty, Id = wallet.Id });

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
    }
}
