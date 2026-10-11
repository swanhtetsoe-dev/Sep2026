using System;
using System.Collections.Generic;
using System.Linq;

namespace Sep2026.Assignment1
{
    public class WalletService
    {
        public static List<WalletDto> Wallets = new List<WalletDto>();

        static WalletService()
        {
            var seed = new WalletService();
            seed.AddWallet("Slim Bifold",       "Louis Vuitton",  GenderType.Men,    "Black",       45000m, 20, silent: true);
            seed.AddWallet("Zip Around",        "Gucci",          GenderType.Women,  "Pink",        52000m, 15, silent: true);
            seed.AddWallet("Card Holder",       "Prada",          GenderType.Unisex, "Brown",       18000m, 30, silent: true);
            seed.AddWallet("Long Clutch",       "Coach",          GenderType.Women,  "Red",         38000m, 12, silent: true);
            seed.AddWallet("Trifold Classic",   "Tommy Hilfiger", GenderType.Men,    "Navy Blue",   22000m, 25, silent: true);
            seed.AddWallet("Mini Crossbody",    "Kate Spade",     GenderType.Women,  "Cream",       29000m, 18, silent: true);
            seed.AddWallet("Travel Wallet",     "Fossil",         GenderType.Unisex, "Dark Brown",  25000m, 10, silent: true);
            seed.AddWallet("Velcro Sport",      "Nike",           GenderType.Men,    "Black",        8500m, 40, silent: true);
            seed.AddWallet("Coin Purse",        "Burberry",       GenderType.Women,  "Beige",       31000m, 22, silent: true);
            seed.AddWallet("Magic Wallet",      "Secrid",         GenderType.Unisex, "Gunmetal",    19500m, 17, silent: true);
            seed.AddWallet("RFID Shield",       "Tumi",           GenderType.Men,    "Charcoal",    55000m,  8, silent: true);
            seed.AddWallet("Flap Wallet",       "Michael Kors",   GenderType.Women,  "Rose Gold",   34000m, 14, silent: true);
            seed.AddWallet("Minimalist Slim",   "Bellroy",        GenderType.Unisex, "Caramel",     21000m, 35, silent: true);
            seed.AddWallet("Checkbook Wallet",  "Levi's",         GenderType.Men,    "Tan",         12000m, 28, silent: true);
            seed.AddWallet("Wristlet Zip",      "Vera Bradley",   GenderType.Women,  "Lavender",    16500m, 20, silent: true);
        }

        public void AddWallet(string name, string brand, GenderType gender, string color, decimal price, int stockQty, bool silent = false)
        {
            int maxId = Wallets.Count == 0 ? 0 : Wallets.Max(x => x.Id);
            int newId = maxId + 1;
            string productCode = $"WAL-{newId:D4}";

            WalletDto wallet = new WalletDto
            {
                Id = newId,
                ProductCode = productCode,
                Name = name,
                Brand = brand,
                Gender = gender,
                Color = color,
                Price = price,
                StockQuantity = stockQty
            };

            Wallets.Add(wallet);
            if (!silent)
                Console.WriteLine($"Wallet added successfully. Product Code: {productCode}");
        }

        public WalletDto? FindWallet(string nameOrCode)
        {
            return Wallets.FirstOrDefault(x =>
                x.ProductCode.Equals(nameOrCode, StringComparison.OrdinalIgnoreCase) ||
                x.Name.Equals(nameOrCode, StringComparison.OrdinalIgnoreCase));
        }

        public void ShowWallets()
        {
            if (Wallets.Count == 0)
            {
                Console.WriteLine("No wallets available.");
                return;
            }
            Console.WriteLine("\n--- Wallet List ---");
            foreach (var item in Wallets)
            {
                Console.WriteLine($"[{item.ProductCode}] {item.Name} | Brand: {item.Brand} | Gender: {item.Gender} | Color: {item.Color} | Price: {item.Price:C} | Stock: {item.StockQuantity}");
            }
            Console.WriteLine("-------------------");
        }
    }
}
