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
            if (!_walletService.DeductStock(wallet, qty))
                return;

            decimal totalAmount = wallet.CalculateTotalAmount(qty);

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

            Console.WriteLine($"Sale successful! {qty} x {wallet.Name} ({wallet.ProductCode}) = {totalAmount.ToCurrency()}. Remaining stock: {wallet.StockQuantity}");
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
                Console.WriteLine($"""
                    Sale ID : {sale.SaleId}
                      Product  : [{sale.ProductCode}] {sale.WalletName}
                      Qty Sold : {sale.SoldQty}
                      Amount   : {sale.TotalAmount.ToCurrency()}
                      Date     : {sale.SaleDate:yyyy-MM-dd HH:mm}
                      ----------------------------------
                    """);
            }

            decimal grandTotal = Sales.Sum(x => x.TotalAmount);
            Console.WriteLine($"  Grand Total Sales : {grandTotal.ToCurrency()}");
            Console.WriteLine("==================================");
        }
    }
}
