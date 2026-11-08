namespace Sep2026.WalletShop
{
    public class WalletShopApp
    {
        private readonly IWalletService _walletService;
        private readonly ISaleService   _saleService;

        public WalletShopApp(IWalletService walletService, ISaleService saleService)
        {
            _walletService = walletService;
            _saleService   = saleService;
        }

        public void Run()
        {
            Start:

            Console.WriteLine(@"
============================
     Wallet Shop System     
============================
1. Add Wallet
2. Sell Wallet
3. Edit Wallet
4. Delete Wallet
5. Show All Wallets
6. View Sales Report
7. Exit");
            Console.Write("Please select an option: ");
            string option = Console.ReadLine() ?? "";

            if (!Enum.TryParse(option, out MenuOption menuOption))
            {
                Console.WriteLine("Invalid option. Please try again.");
                goto Start;
            }

            switch (menuOption)
            {
                case MenuOption.AddWallet:
                    HandleAddWallet();
                    _walletService.ShowWallets();
                    break;

                case MenuOption.SellWallet:
                    HandleSellWallet();
                    break;

                case MenuOption.EditWallet:
                    HandleEditWallet();
                    _walletService.ShowWallets();
                    break;

                case MenuOption.DeleteWallet:
                    HandleDeleteWallet();
                    _walletService.ShowWallets();
                    break;

                case MenuOption.ShowWallets:
                    _walletService.ShowWallets();
                    break;

                case MenuOption.SalesReport:
                    _saleService.ShowSalesReport();
                    break;

                case MenuOption.Exit:
                    goto End;
            }

            goto Start;

            End:
            Console.WriteLine("Thank you for using Wallet Shop. Goodbye!");
        }

        private void HandleAddWallet()
        {
            Console.Write("Enter wallet name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter brand: ");
            string brand = Console.ReadLine() ?? "";

            Console.WriteLine(@"Select gender:
  1 = Men
  2 = Women
  3 = Unisex");
            Console.Write("Enter choice: ");
            string genderInput = Console.ReadLine() ?? ((int)GenderType.Unisex).ToString();

            GenderType gender = Enum.TryParse(genderInput, out GenderType g)
                ? g
                : GenderType.Unisex;

            Console.Write("Enter color: ");
            string color = Console.ReadLine() ?? "";

            Console.Write("Enter price: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter stock quantity: ");
            int stockQty = int.Parse(Console.ReadLine() ?? "0");

            WalletCreateDto wallet = new WalletCreateDto
            {
                Name = name,
                Brand = brand,
                Gender = gender,
                Color = color,
                Price = price,
                StockQuantity = stockQty
            };

            _walletService.AddWallet(wallet);
        }

        private void HandleSellWallet()
        {
            Console.Write("Enter wallet name or product code (e.g. WAL-0001): ");
            string nameOrCode = Console.ReadLine() ?? "";

            WalletDto? found = _walletService.FindWallet(nameOrCode);
            if (found == null)
            {
                Console.WriteLine("Wallet not found. Please check the name or product code.");
                return;
            }

            Console.WriteLine(
                $"Product: [{found.ProductCode}] {found.Name} | Brand: {found.Brand} | " +
                $"Color: {found.Color} | Price: {found.Price.ToCurrency()} | Stock: {found.StockQuantity}");

            Console.Write("Enter quantity to sell: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");

            _saleService.SellWallet(found, qty);
        }

        private void HandleEditWallet()
        {
            _walletService.ShowWallets();

            Console.Write("Enter product code to edit: ");
            string code = Console.ReadLine() ?? "";

            WalletDto? found = _walletService.FindWallet(code);
            if (found == null)
            {
                Console.WriteLine("Wallet not found.");
                return;
            }

            Console.WriteLine($"Editing [{found.ProductCode}] {found.Name} — Press Enter to keep current value.");

            Console.Write($"Name [{found.Name}]: ");
            string input = Console.ReadLine() ?? "";
            string name = input.Length > 0 ? input : found.Name;

            Console.Write($"Brand [{found.Brand}]: ");
            input = Console.ReadLine() ?? "";
            string brand = input.Length > 0 ? input : found.Brand;

            Console.WriteLine($@"Select gender (current: {found.Gender}):
  1 = Men
  2 = Women
  3 = Unisex");
            Console.Write("Enter choice (or Enter to keep): ");
            input = Console.ReadLine() ?? "";
            GenderType gender = Enum.TryParse(input, out GenderType g) ? g : found.Gender;

            Console.Write($"Color [{found.Color}]: ");
            input = Console.ReadLine() ?? "";
            string color = input.Length > 0 ? input : found.Color;

            Console.Write($"Price [{found.Price}]: ");
            input = Console.ReadLine() ?? "";
            decimal price = input.Length > 0 ? decimal.Parse(input) : found.Price;

            Console.Write($"Stock Quantity [{found.StockQuantity}]: ");
            input = Console.ReadLine() ?? "";
            int stockQty = input.Length > 0 ? int.Parse(input) : found.StockQuantity;

            WalletEditDto wallet = new WalletEditDto
            {
                Id            = found.Id,
                Name          = name,
                Brand         = brand,
                Gender        = gender,
                Color         = color,
                Price         = price,
                StockQuantity = stockQty
            };

            _walletService.EditWallet(wallet);
        }

        private void HandleDeleteWallet()
        {
            _walletService.ShowWallets();

            Console.Write("Enter product code to delete: ");
            string code = Console.ReadLine() ?? "";

            WalletDto? found = _walletService.FindWallet(code);
            if (found == null)
            {
                Console.WriteLine("Wallet not found.");
                return;
            }

            Console.Write($"Delete [{found.ProductCode}] {found.Name}? (y/n): ");
            string confirm = Console.ReadLine() ?? "";

            if (confirm.ToLower() == "y")
                _walletService.DeleteWallet(found.Id);
            else
                Console.WriteLine("Delete cancelled.");
        }
    }
}
