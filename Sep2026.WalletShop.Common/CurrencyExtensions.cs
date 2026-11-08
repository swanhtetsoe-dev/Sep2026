namespace Sep2026.WalletShop
{
    public static class CurrencyExtensions
    {
        public static string ToCurrency(this decimal value) => value.ToString("C0");
    }
}
