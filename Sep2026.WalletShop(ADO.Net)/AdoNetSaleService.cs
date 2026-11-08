using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class AdoNetSaleService : ISaleService
    {
        public void SellWallet(WalletDto wallet, int qty)
        {
            decimal totalAmount = wallet.CalculateTotalAmount(qty);

            using var conn = DbHelper.GetOpenConnection();

            SqlCommand cmd = new SqlCommand(SaleQueries.Insert, conn);

            cmd.Parameters.AddWithValue("@WalletId",    wallet.Id);
            cmd.Parameters.AddWithValue("@ProductCode", wallet.ProductCode);
            cmd.Parameters.AddWithValue("@WalletName",  wallet.Name);
            cmd.Parameters.AddWithValue("@SoldQty",     qty);
            cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
            cmd.Parameters.AddWithValue("@SaleDate",    DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public List<SaleDto> GetSalesReport()
        {
            List<SaleDto> sales = new List<SaleDto>();

            DataTable dt = new DataTable();
            using var conn = DbHelper.GetOpenConnection();

            SqlCommand cmd = new SqlCommand(SaleQueries.SelectAll, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                sales.Add(new SaleDto
                {
                    SaleId      = Convert.ToInt32(row["SaleId"]),
                    ProductCode = row["ProductCode"].ToString()!,
                    WalletName  = row["WalletName"].ToString()!,
                    SoldQty     = Convert.ToInt32(row["SoldQty"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    SaleDate    = Convert.ToDateTime(row["SaleDate"])
                });
            }

            return sales;
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
