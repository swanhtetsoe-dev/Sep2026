using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class WalletService
    {
        public List<WalletDto> GetAllWallets()
        {
            List<WalletDto> wallets = new List<WalletDto>();

            DataTable dt = new DataTable();
            var conn = DbHelper.GetOpenConnection();
            try
            {
                string selectSql = @"
                    SELECT [Id],[ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted]
                    FROM [dbo].[Wallets]
                    WHERE [IsDeleted] = 0";

                SqlCommand cmd = new SqlCommand(selectSql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            foreach (DataRow row in dt.Rows)
            {
                wallets.Add(MapRow(row));
            }

            return wallets;
        }

        public void AddWallet(WalletCreateDto wallet)
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand getMaxId = new SqlCommand(
                    "SELECT ISNULL(MAX([Id]), 0) FROM [dbo].[Wallets]", conn);
                int newId = Convert.ToInt32(getMaxId.ExecuteScalar()) + 1;

                string productCode = $"WAL-{newId:D4}";

                string insertSql = @"
                    INSERT INTO [dbo].[Wallets]
                        ([ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted])
                    VALUES
                        (@ProductCode,@Name,@Brand,@Gender,@Color,@Price,@StockQuantity,0)";

                SqlCommand cmd = new SqlCommand(insertSql, conn);

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
            finally
            {
                conn.Close();
            }
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                string searchSql = @"
                    SELECT TOP 1
                        [Id],[ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted]
                    FROM [dbo].[Wallets]
                    WHERE [IsDeleted] = 0
                        AND ([ProductCode] LIKE @Search OR [Name] LIKE @Search)";

                SqlCommand cmd = new SqlCommand(searchSql, conn);

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
            finally
            {
                conn.Close();
            }
        }

        public void EditWallet(WalletEditDto wallet)
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                string updateSql = @"
                    UPDATE [dbo].[Wallets]
                    SET [Name]          = @Name,
                        [Brand]         = @Brand,
                        [Gender]        = @Gender,
                        [Color]         = @Color,
                        [Price]         = @Price,
                        [StockQuantity] = @StockQuantity
                    WHERE [Id] = @Id AND [IsDeleted] = 0";

                SqlCommand cmd = new SqlCommand(updateSql, conn);

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
            finally
            {
                conn.Close();
            }
        }

        public void DeleteWallet(int id)
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                string deleteSql = @"
                    UPDATE [dbo].[Wallets]
                    SET [IsDeleted] = 1
                    WHERE [Id] = @Id";

                SqlCommand cmd = new SqlCommand(deleteSql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();

                Console.WriteLine("Wallet deleted successfully.");
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

        public bool DeductStock(WalletDto wallet, int qty)
        {
            if (!HasSufficientStock(wallet, qty))
            {
                Console.WriteLine(
                    $"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
                return false;
            }

            var conn = DbHelper.GetOpenConnection();
            try
            {
                string deductSql = @"
                    UPDATE [dbo].[Wallets]
                    SET    [StockQuantity] = [StockQuantity] - @Qty
                    WHERE  [Id] = @Id";

                SqlCommand cmd = new SqlCommand(deductSql, conn);

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
            finally
            {
                conn.Close();
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
