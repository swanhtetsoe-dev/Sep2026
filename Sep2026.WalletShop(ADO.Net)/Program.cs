using Sep2026.WalletShop;

WalletService walletService = new WalletService();
SaleService   saleService   = new SaleService(walletService);

Start:

Console.WriteLine(@"
============================
     Wallet Shop System     
============================
1. Add Wallet
2. Sell Wallet
3. View Sales Report
4. Show All Wallets
5. Edit Wallet
6. Delete Wallet
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
        walletService.ShowWallets();
        break;

    case MenuOption.SellWallet:
        HandleSellWallet();
        break;

    case MenuOption.SalesReport:
        saleService.ShowSalesReport();
        break;

    case MenuOption.ShowWallets:
        walletService.ShowWallets();
        break;

    case MenuOption.EditWallet:
        HandleEditWallet();
        walletService.ShowWallets();
        break;

    case MenuOption.DeleteWallet:
        HandleDeleteWallet();
        walletService.ShowWallets();
        break;

    case MenuOption.Exit:
        goto End;
}

goto Start;

End:
Console.WriteLine("Thank you for using Wallet Shop. Goodbye!");


void HandleAddWallet()
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

    walletService.AddWallet(name, brand, gender, color, price, stockQty);
}

void HandleSellWallet()
{
    Console.Write("Enter wallet name or product code (e.g. WAL-0001): ");
    string nameOrCode = Console.ReadLine() ?? "";

    WalletDto? found = walletService.FindWallet(nameOrCode);
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

    saleService.SellWallet(found, qty);
}

void HandleEditWallet()
{
    walletService.ShowWallets();

    Console.Write("Enter product code to edit: ");
    string code = Console.ReadLine() ?? "";

    WalletDto? found = walletService.FindWallet(code);
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

    walletService.EditWallet(found.Id, name, brand, gender, color, price, stockQty);
}

void HandleDeleteWallet()
{
    walletService.ShowWallets();

    Console.Write("Enter product code to delete: ");
    string code = Console.ReadLine() ?? "";

    WalletDto? found = walletService.FindWallet(code);
    if (found == null)
    {
        Console.WriteLine("Wallet not found.");
        return;
    }

    Console.Write($"Delete [{found.ProductCode}] {found.Name}? (y/n): ");
    string confirm = Console.ReadLine() ?? "";

    if (confirm.ToLower() == "y")
        walletService.DeleteWallet(found.Id);
    else
        Console.WriteLine("Delete cancelled.");
}
