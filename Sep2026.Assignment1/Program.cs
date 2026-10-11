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

switch (option)
{
    case "1":
        Console.Write("Enter wallet name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter brand: ");
        string brand = Console.ReadLine() ?? "";

        Console.WriteLine(@"Select gender:
  1 = Men
  2 = Women
  3 = Unisex");
        Console.Write("Enter choice: ");
        string genderInput = Console.ReadLine() ?? "3";
        GenderType gender = genderInput switch
        {
            "1" => GenderType.Men,
            "2" => GenderType.Women,
            _   => GenderType.Unisex
        };

        Console.Write("Enter color: ");
        string color = Console.ReadLine() ?? "";

        Console.Write("Enter price: ");
        decimal price = decimal.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter stock quantity: ");
        int stockQty = int.Parse(Console.ReadLine() ?? "0");

        walletService.AddWallet(name, brand, gender, color, price, stockQty);
        walletService.ShowWallets();
        break;

    case "2":
        Console.Write("Enter wallet name or product code (e.g. WAL-001): ");
        string nameOrCode = Console.ReadLine() ?? "";

        Console.Write("Enter quantity to sell: ");
        int qty = int.Parse(Console.ReadLine() ?? "0");

        saleService.SellWallet(nameOrCode, qty);
        break;

    case "3":
        saleService.ShowSalesReport();
        break;

    case "4":
        walletService.ShowWallets();
        break;

    case "5":
        goto End;

    default:
        Console.WriteLine("Invalid option. Please try again.");
        break;
}

goto Start;

End:
Console.WriteLine("Thank you for using Wallet Shop. Goodbye!");
