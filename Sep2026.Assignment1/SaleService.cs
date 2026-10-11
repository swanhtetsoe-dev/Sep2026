using System;
using System.Collections.Generic;
using System.Linq;

namespace Sep2026.Assignment1
{
    public class SaleService
    {
        public static List<SaleDto> Sales = new List<SaleDto>();

        private readonly WalletService _walletService;

        public SaleService(WalletService walletService)
        {
            _walletService = walletService;
        }

        public void SellWallet(WalletDto wallet, int qty)
        {
            if (wallet.StockQuantity <= 0 || wallet.StockQuantity < qty)
            {
                Console.WriteLine($"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
                return;
            }

            wallet.StockQuantity -= qty;
            decimal totalAmount = wallet.Price * qty;

            int maxSaleId = Sales.Count == 0 ? 0 : Sales.Max(x => x.SaleId);
            SaleDto sale = new SaleDto
            {
                SaleId = maxSaleId + 1,
                WalletId = wallet.Id,
                ProductCode = wallet.ProductCode,
                WalletName = wallet.Name,
                SoldQty = qty,
                TotalAmount = totalAmount,
                SaleDate = DateTime.Now
            };
            Sales.Add(sale);

            Console.WriteLine($"Sale successful! {qty} x {wallet.Name} ({wallet.ProductCode}) = {totalAmount:C}. Remaining stock: {wallet.StockQuantity}");
        }

        public void ShowSalesReport()
        {
            if (Sales.Count == 0)
            {
                Console.WriteLine("No sales records found.");
                return;
            }

            Console.WriteLine("\n========== Sales Report ==========");
            foreach (var sale in Sales)
            {
                Console.WriteLine($"Sale ID : {sale.SaleId}");
                Console.WriteLine($"  Product : [{sale.ProductCode}] {sale.WalletName}");
                Console.WriteLine($"  Qty Sold : {sale.SoldQty}");
                Console.WriteLine($"  Amount   : {sale.TotalAmount:C}");
                Console.WriteLine($"  Date     : {sale.SaleDate:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"  ----------------------------------");
            }

            decimal grandTotal = Sales.Sum(x => x.TotalAmount);
            Console.WriteLine($"  Grand Total Sales : {grandTotal:C}");
            Console.WriteLine("==================================");
        }
    }
}
