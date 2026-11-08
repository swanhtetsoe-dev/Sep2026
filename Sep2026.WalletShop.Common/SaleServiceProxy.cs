namespace Sep2026.WalletShop
{
    public class SaleServiceProxy : ISaleService
    {
        private readonly ISaleService _inner;

        public SaleServiceProxy(ISaleService inner)
        {
            _inner = inner;
        }

        public void SellWallet(WalletDto wallet, int qty)
        {
            try
            {
                int stockBefore = wallet.StockQuantity;
                _inner.SellWallet(wallet, qty);

                decimal totalAmount = wallet.CalculateTotalAmount(qty);
                Console.WriteLine(
                    $"Sale successful! {qty} x {wallet.Name} ({wallet.ProductCode}) = " +
                    $"{totalAmount.ToCurrency()}. Remaining stock: {wallet.StockQuantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public List<SaleDto> GetSalesReport()
            => _inner.GetSalesReport();

        public void ShowSalesReport()
        {
            List<SaleDto> sales;

            try
            {
                sales = _inner.GetSalesReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
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
