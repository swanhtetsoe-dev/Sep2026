using Microsoft.EntityFrameworkCore;
using Sep2026.Database.Models;

namespace Sep2026.WalletShop
{
    public class SaleService : ISaleService
    {
        private readonly IWalletService _walletService;
        private readonly AppDbContext   _db;

        public SaleService(IWalletService walletService, AppDbContext db)
        {
            _walletService = walletService;
            _db            = db;
        }

        public void SellWallet(WalletDto wallet, int qty)
        {
            if (!_walletService.DeductStock(wallet, qty))
                return;

            decimal totalAmount = wallet.CalculateTotalAmount(qty);

            try
            {
                Sale sale = new Sale
                {
                    WalletId    = wallet.Id,
                    ProductCode = wallet.ProductCode,
                    WalletName  = wallet.Name,
                    SoldQty     = qty,
                    TotalAmount = totalAmount,
                    SaleDate    = DateTime.Now
                };

                _db.Sales.Add(sale);
                _db.SaveChanges();

                Console.WriteLine(
                    $"Sale successful! {qty} x {wallet.Name} ({wallet.ProductCode}) = " +
                    $"{totalAmount.ToCurrency()}. Remaining stock: {wallet.StockQuantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void ShowSalesReport()
        {
            List<SaleDto> sales = new List<SaleDto>();

            try
            {
                sales = _db.Sales
                    .OrderBy(s => s.SaleId)
                    .Select(s => new SaleDto
                    {
                        SaleId      = s.SaleId,
                        WalletId    = s.WalletId,
                        ProductCode = s.ProductCode,
                        WalletName  = s.WalletName,
                        SoldQty     = s.SoldQty,
                        TotalAmount = s.TotalAmount,
                        SaleDate    = s.SaleDate
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (sales.Count == 0)
            {
                Console.WriteLine("No sales records found.");
                return;
            }

            Console.WriteLine("\n========== Sales Report ==========");

            decimal grandTotal = 0m;
            foreach (SaleDto sale in sales)
            {
                grandTotal += sale.TotalAmount;

                Console.WriteLine($"""
                    Sale ID : {sale.SaleId}
                      Product  : [{sale.ProductCode}] {sale.WalletName}
                      Qty Sold : {sale.SoldQty}
                      Amount   : {sale.TotalAmount.ToCurrency()}
                      Date     : {sale.SaleDate:yyyy-MM-dd HH:mm}
                      ----------------------------------
                    """);
            }

            Console.WriteLine($"  Grand Total Sales : {grandTotal.ToCurrency()}");
            Console.WriteLine("==================================");
        }
    }
}
