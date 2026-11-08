namespace Sep2026.WalletShop
{
    public class WalletServiceProxy : IWalletService
    {
        private readonly IWalletService _inner;

        public WalletServiceProxy(IWalletService inner)
        {
            _inner = inner;
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            try
            {
                _inner.AddWallet(wallet);
                Console.WriteLine("Wallet added successfully.");
            }
            catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex)                 { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public void EditWallet(WalletEditDto wallet)
        {
            try
            {
                _inner.EditWallet(wallet);
                Console.WriteLine("Wallet updated successfully.");
            }
            catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex)                 { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public void DeleteWallet(int id)
        {
            try
            {
                _inner.DeleteWallet(id);
                Console.WriteLine("Wallet deleted successfully.");
            }
            catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex)                 { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            try
            {
                return _inner.FindWallet(nameOrCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public bool DeductStock(WalletDto wallet, int qty)
        {
            try
            {
                bool ok = _inner.DeductStock(wallet, qty);
                if (!ok)
                    Console.WriteLine($"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
                return ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public List<WalletDto> GetAllWallets()
        {
            try
            {
                return _inner.GetAllWallets();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<WalletDto>();
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
    }
}
