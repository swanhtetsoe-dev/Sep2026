using Dapper;

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
        }

        public List<SaleDto> GetSalesReport()
        {
            using var conn = DbHelper.GetOpenConnection();
            return conn.Query<SaleDto>(SaleQueries.SelectAll).ToList();
        }

        public void ShowSalesReport()
        {
            List<SaleDto> sales = GetSalesReport();

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
