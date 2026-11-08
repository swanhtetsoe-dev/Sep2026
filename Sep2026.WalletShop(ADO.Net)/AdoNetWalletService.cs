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

            SqlCommand cmd = new SqlCommand(WalletQueries.SelectAll, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                wallets.Add(MapRow(row));
            }

            return wallets;
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();

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
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            using var conn = DbHelper.GetOpenConnection();

            SqlCommand cmd = new SqlCommand(WalletQueries.FindByNameOrCode, conn);
            cmd.Parameters.AddWithValue("@Search", $"%{nameOrCode}%");

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        public void EditWallet(WalletEditDto wallet)
        {
            using var conn = DbHelper.GetOpenConnection();

            SqlCommand cmd = new SqlCommand(WalletQueries.Update, conn);

            cmd.Parameters.AddWithValue("@Name",          wallet.Name);
            cmd.Parameters.AddWithValue("@Brand",         wallet.Brand);
            cmd.Parameters.AddWithValue("@Gender",        (byte)wallet.Gender);
            cmd.Parameters.AddWithValue("@Color",         wallet.Color);
            cmd.Parameters.AddWithValue("@Price",         wallet.Price);
            cmd.Parameters.AddWithValue("@StockQuantity", wallet.StockQuantity);
            cmd.Parameters.AddWithValue("@Id",            wallet.Id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                throw new InvalidOperationException("Wallet not found.");
        }

        public void DeleteWallet(int id)
        {
            using var conn = DbHelper.GetOpenConnection();

            SqlCommand cmd = new SqlCommand(WalletQueries.SoftDelete, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                throw new InvalidOperationException("Wallet not found.");
        }

        public bool DeductStock(WalletDto wallet, int qty)
        {
            if (wallet.StockQuantity <= 0 || wallet.StockQuantity < qty)
                return false;

            using var conn = DbHelper.GetOpenConnection();

            SqlCommand cmd = new SqlCommand(WalletQueries.DeductStock, conn);
            cmd.Parameters.AddWithValue("@Qty", qty);
            cmd.Parameters.AddWithValue("@Id",  wallet.Id);

            cmd.ExecuteNonQuery();

            wallet.StockQuantity -= qty;
            return true;
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
