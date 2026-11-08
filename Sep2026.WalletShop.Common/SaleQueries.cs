namespace Sep2026.WalletShop
{
    public static class SaleQueries
    {
        public const string Insert = @"
                    INSERT INTO [dbo].[Sales]
                        ([WalletId],[ProductCode],[WalletName],[SoldQty],[TotalAmount],[SaleDate])
                    VALUES
                        (@WalletId,@ProductCode,@WalletName,@SoldQty,@TotalAmount,@SaleDate)";

        public const string SelectAll = @"
                    SELECT [SaleId],[WalletId],[ProductCode],[WalletName],
                           [SoldQty],[TotalAmount],[SaleDate]
                    FROM   [dbo].[Sales]
                    ORDER BY [SaleId]";
    }
}
