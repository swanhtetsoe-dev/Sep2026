using Microsoft.Data.SqlClient;
using System.Data;

namespace Sep2026.WalletShop
{
    public class WalletService
    {
        public List<WalletDto> GetAllWallets()
        {
            List<WalletDto> wallets = new List<WalletDto>();

            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT [Id],[ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted]
                      FROM [dbo].[Wallets]
                      WHERE [IsDeleted] = 0", conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    wallets.Add(MapRow(row));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return wallets;
        }

        public void AddWallet(string name, string brand, GenderType gender,
                              string color, decimal price, int stockQty)
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand getMaxId = new SqlCommand(
                    "SELECT ISNULL(MAX([Id]), 0) FROM [dbo].[Wallets]", conn);
                int newId = Convert.ToInt32(getMaxId.ExecuteScalar()) + 1;

                string productCode = $"WAL-{newId:D4}";

                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO [dbo].[Wallets]
                        ([ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted])
                      VALUES
                        (@ProductCode,@Name,@Brand,@Gender,@Color,@Price,@StockQuantity,0)", conn);

                cmd.Parameters.AddWithValue("@ProductCode",   productCode);
                cmd.Parameters.AddWithValue("@Name",          name);
                cmd.Parameters.AddWithValue("@Brand",         brand);
                cmd.Parameters.AddWithValue("@Gender",        (byte)gender);
                cmd.Parameters.AddWithValue("@Color",         color);
                cmd.Parameters.AddWithValue("@Price",         price);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQty);

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
                SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP 1
                        [Id],[ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted]
                      FROM [dbo].[Wallets]
                      WHERE [IsDeleted] = 0
                        AND ([ProductCode] LIKE @Search OR [Name] LIKE @Search)", conn);

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

        public void EditWallet(int id, string name, string brand, GenderType gender,
                               string color, decimal price, int stockQty)
        {
            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE [dbo].[Wallets]
                      SET [Name]          = @Name,
                          [Brand]         = @Brand,
                          [Gender]        = @Gender,
                          [Color]         = @Color,
                          [Price]         = @Price,
                          [StockQuantity] = @StockQuantity
                      WHERE [Id] = @Id AND [IsDeleted] = 0", conn);

                cmd.Parameters.AddWithValue("@Name",          name);
                cmd.Parameters.AddWithValue("@Brand",         brand);
                cmd.Parameters.AddWithValue("@Gender",        (byte)gender);
                cmd.Parameters.AddWithValue("@Color",         color);
                cmd.Parameters.AddWithValue("@Price",         price);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQty);
                cmd.Parameters.AddWithValue("@Id",            id);

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
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE [dbo].[Wallets]
                      SET [IsDeleted] = 1
                      WHERE [Id] = @Id", conn);

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
            if (wallet.StockQuantity <= 0 || wallet.StockQuantity < qty)
            {
                Console.WriteLine(
                    $"Out of stock or insufficient stock. Available stock: {wallet.StockQuantity}");
                return false;
            }

            var conn = DbHelper.GetOpenConnection();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE [dbo].[Wallets]
                      SET    [StockQuantity] = [StockQuantity] - @Qty
                      WHERE  [Id] = @Id", conn);

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
