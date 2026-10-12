namespace Sep2026.Assignment1
{
    public static class CurrencyExtensions
    {
        public static string ToCurrency(this decimal value) => value.ToString("C0");
    }
}
