using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class SaleService
    {
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

            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO [dbo].[Sales]
                        ([WalletId],[ProductCode],[WalletName],[SoldQty],[TotalAmount],[SaleDate])
                      VALUES
                        (@WalletId,@ProductCode,@WalletName,@SoldQty,@TotalAmount,@SaleDate)", conn);

                cmd.Parameters.AddWithValue("@WalletId",    wallet.Id);
                cmd.Parameters.AddWithValue("@ProductCode", wallet.ProductCode);
                cmd.Parameters.AddWithValue("@WalletName",  wallet.Name);
                cmd.Parameters.AddWithValue("@SoldQty",     qty);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@SaleDate",    DateTime.Now);

                cmd.ExecuteNonQuery();

                Console.WriteLine(
                    $"Sale successful! {qty} x {wallet.Name} ({wallet.ProductCode}) = " +
                    $"{totalAmount.ToCurrency()}. Remaining stock: {wallet.StockQuantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        public void ShowSalesReport()
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT [SaleId],[WalletId],[ProductCode],[WalletName],
                             [SoldQty],[TotalAmount],[SaleDate]
                      FROM   [dbo].[Sales]
                      ORDER BY [SaleId]", conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    Console.WriteLine("No sales records found.");
                    return;
                }

                Console.WriteLine("\n========== Sales Report ==========");

                decimal grandTotal = 0m;
                foreach (DataRow row in dt.Rows)
                {
                    int      saleId      = Convert.ToInt32(row["SaleId"]);
                    string   productCode = row["ProductCode"].ToString()!;
                    string   walletName  = row["WalletName"].ToString()!;
                    int      soldQty     = Convert.ToInt32(row["SoldQty"]);
                    decimal  totalAmount = Convert.ToDecimal(row["TotalAmount"]);
                    DateTime saleDate    = Convert.ToDateTime(row["SaleDate"]);

                    grandTotal += totalAmount;

                    Console.WriteLine($"""
                        Sale ID : {saleId}
                          Product  : [{productCode}] {walletName}
                          Qty Sold : {soldQty}
                          Amount   : {totalAmount.ToCurrency()}
                          Date     : {saleDate:yyyy-MM-dd HH:mm}
                          ----------------------------------
                        """);
                }

                Console.WriteLine($"  Grand Total Sales : {grandTotal.ToCurrency()}");
                Console.WriteLine("==================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
