namespace Sep2026.WalletShop
{
    public static class WalletQueries
    {
        public const string SelectAll = @"
                    SELECT [Id],[ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted]
                    FROM [dbo].[Wallets]
                    WHERE [IsDeleted] = 0";

        public const string GetMaxId = "SELECT ISNULL(MAX([Id]), 0) FROM [dbo].[Wallets]";

        public const string Insert = @"
                    INSERT INTO [dbo].[Wallets]
                        ([ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted])
                    VALUES
                        (@ProductCode,@Name,@Brand,@Gender,@Color,@Price,@StockQuantity,0)";

        public const string FindByNameOrCode = @"
                    SELECT TOP 1
                        [Id],[ProductCode],[Name],[Brand],[Gender],[Color],[Price],[StockQuantity],[IsDeleted]
                    FROM [dbo].[Wallets]
                    WHERE [IsDeleted] = 0
                        AND ([ProductCode] LIKE @Search OR [Name] LIKE @Search)";

        public const string Update = @"
                    UPDATE [dbo].[Wallets]
                    SET [Name]          = @Name,
                        [Brand]         = @Brand,
                        [Gender]        = @Gender,
                        [Color]         = @Color,
                        [Price]         = @Price,
                        [StockQuantity] = @StockQuantity
                    WHERE [Id] = @Id AND [IsDeleted] = 0";

        public const string SoftDelete = @"
                    UPDATE [dbo].[Wallets]
                    SET [IsDeleted] = 1
                    WHERE [Id] = @Id";

        public const string DeductStock = @"
                    UPDATE [dbo].[Wallets]
                    SET    [StockQuantity] = [StockQuantity] - @Qty
                    WHERE  [Id] = @Id";
    }
}
