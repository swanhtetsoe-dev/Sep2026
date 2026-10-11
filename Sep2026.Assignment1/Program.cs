using Sep2026.Assignment1;

WalletService walletService = new WalletService();
SaleService saleService = new SaleService(walletService);

Start:

Console.WriteLine(@"
============================
     Wallet Shop System     
============================
1. Add Wallet
2. Sell Wallet
3. View Sales Report
4. Show All Wallets
5. Exit");
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

        walletService.AddWallet(name, brand, gender, color, price, stockQty);
        walletService.ShowWallets();
        break;

    case MenuOption.SellWallet:
        Console.Write("Enter wallet name or product code (e.g. WAL-001): ");
        string nameOrCode = Console.ReadLine() ?? "";

        WalletDto? found = walletService.FindWallet(nameOrCode);
        if (found == null)
        {
            Console.WriteLine("Wallet not found. Please check the name or product code.");
            break;
        }

        Console.WriteLine($"Product: [{found.ProductCode}] {found.Name} | Brand: {found.Brand} | Color: {found.Color} | Price: {found.Price.ToCurrency()} | Stock: {found.StockQuantity}");

        Console.Write("Enter quantity to sell: ");
        int qty = int.Parse(Console.ReadLine() ?? "0");

        saleService.SellWallet(found, qty);
        break;

    case MenuOption.SalesReport:
        saleService.ShowSalesReport();
        break;

    case MenuOption.ShowWallets:
        walletService.ShowWallets();
        break;

    case MenuOption.Exit:
        goto End;
}

goto Start;

End:
Console.WriteLine("Thank you for using Wallet Shop. Goodbye!");
