using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class AdoNetWalletService : IWalletService
    {
        public List<WalletDto> GetAllWallets()
        {
            List<WalletDto> wallets = new List<WalletDto>();

            DataTable dt = new DataTable();
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(WalletQueries.SelectAll, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            foreach (DataRow row in dt.Rows)
            {
                wallets.Add(MapRow(row));
            }

            return wallets;
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand getMaxId = new SqlCommand(WalletQueries.GetMaxId, conn);
                int newId = Convert.ToInt32(getMaxId.ExecuteScalar()) + 1;

                string productCode = $"WAL-{newId:D4}";

                SqlCommand cmd = new SqlCommand(WalletQueries.Insert, conn);

                cmd.Parameters.AddWithValue("@ProductCode",   productCode);
                cmd.Parameters.AddWithValue("@Name",          wallet.Name);
                cmd.Parameters.AddWithValue("@Brand",         wallet.Brand);
                cmd.Parameters.AddWithValue("@Gender",        (byte)wallet.Gender);
                cmd.Parameters.AddWithValue("@Color",         wallet.Color);
                cmd.Parameters.AddWithValue("@Price",         wallet.Price);
                cmd.Parameters.AddWithValue("@StockQuantity", wallet.StockQuantity);

                cmd.ExecuteNonQuery();

                Console.WriteLine($"Wallet added successfully. Product Code: {productCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(WalletQueries.FindByNameOrCode, conn);

                cmd.Parameters.AddWithValue("@Search", $"%{nameOrCode}%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                    return null;

                return MapRow(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public void EditWallet(WalletEditDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(WalletQueries.Update, conn);

                cmd.Parameters.AddWithValue("@Name",          wallet.Name);
                cmd.Parameters.AddWithValue("@Brand",         wallet.Brand);
                cmd.Parameters.AddWithValue("@Gender",        (byte)wallet.Gender);
                cmd.Parameters.AddWithValue("@Color",         wallet.Color);
                cmd.Parameters.AddWithValue("@Price",         wallet.Price);
                cmd.Parameters.AddWithValue("@StockQuantity", wallet.StockQuantity);
                cmd.Parameters.AddWithValue("@Id",            wallet.Id);

                cmd.ExecuteNonQuery();

                Console.WriteLine("Wallet updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void DeleteWallet(int id)
        {
            using var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(WalletQueries.SoftDelete, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();

                Console.WriteLine("Wallet deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public bool DeductStock(WalletDto wallet, int qty)
        {
            if (!HasSufficientStock(wallet, qty))
            {
                Console.WriteLine(
                    $"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
                return false;
            }

            using var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(WalletQueries.DeductStock, conn);

                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@Id",  wallet.Id);

                cmd.ExecuteNonQuery();

                wallet.StockQuantity -= qty;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public void ShowWallets()
        {
            List<WalletDto> wallets = GetAllWallets();

            if (wallets.Count == 0)
            {
                Console.WriteLine("No wallets available.");
                return;
            }

            Console.WriteLine("\n--- Wallet List ---");
            foreach (WalletDto item in wallets)
            {
                Console.WriteLine(
                    $"[{item.ProductCode}] {item.Name} | Brand: {item.Brand} | " +
                    $"Gender: {item.Gender} | Color: {item.Color} | " +
                    $"Price: {item.Price.ToCurrency()} | Stock: {item.StockQuantity}");
            }
            Console.WriteLine("-------------------");
        }

        private static bool HasSufficientStock(WalletDto wallet, int qty)
            => wallet.StockQuantity > 0 && wallet.StockQuantity >= qty;

        private static WalletDto MapRow(DataRow row) => new WalletDto
        {
            Id            = Convert.ToInt32(row["Id"]),
            ProductCode   = row["ProductCode"].ToString()!,
            Name          = row["Name"].ToString()!,
            Brand         = row["Brand"].ToString()!,
            Gender        = (GenderType)Convert.ToByte(row["Gender"]),
            Color         = row["Color"].ToString()!,
            Price         = Convert.ToDecimal(row["Price"]),
            StockQuantity = Convert.ToInt32(row["StockQuantity"]),
            IsDeleted     = Convert.ToBoolean(row["IsDeleted"])
        };
    }
}
