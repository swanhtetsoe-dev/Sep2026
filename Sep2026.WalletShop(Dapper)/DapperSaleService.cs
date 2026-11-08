using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class DapperSaleService : ISaleService
    {
        private readonly IWalletService _walletService;

        public DapperSaleService(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public void SellWallet(WalletDto wallet, int qty)
        {
            if (!_walletService.DeductStock(wallet, qty))
                return;

            decimal totalAmount = wallet.CalculateTotalAmount(qty);

            using var conn = DbHelper.GetOpenConnection();
            try
            {
                var param = new
                {
                    WalletId    = wallet.Id,
                    ProductCode = wallet.ProductCode,
                    WalletName  = wallet.Name,
                    SoldQty     = qty,
                    TotalAmount = totalAmount,
                    SaleDate    = DateTime.Now
                };

                conn.Execute(SaleQueries.Insert, param);

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

            using var conn = DbHelper.GetOpenConnection();
            try
            {
                sales = conn.Query<SaleDto>(SaleQueries.SelectAll).ToList();
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
