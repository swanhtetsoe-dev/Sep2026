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
            Wallets = new List<WalletDto>
            {
                new WalletDto { Id = 1,  ProductCode = "WAL-0001", Name = "Slim Bifold",      Brand = "Louis Vuitton",  Gender = GenderType.Men,    Color = "Black",      Price = 45000m, StockQuantity = 20 },
                new WalletDto { Id = 2,  ProductCode = "WAL-0002", Name = "Zip Around",       Brand = "Gucci",          Gender = GenderType.Women,  Color = "Pink",       Price = 52000m, StockQuantity = 15 },
                new WalletDto { Id = 3,  ProductCode = "WAL-0003", Name = "Card Holder",      Brand = "Prada",          Gender = GenderType.Unisex, Color = "Brown",      Price = 18000m, StockQuantity = 30 },
                new WalletDto { Id = 4,  ProductCode = "WAL-0004", Name = "Long Clutch",      Brand = "Coach",          Gender = GenderType.Women,  Color = "Red",        Price = 38000m, StockQuantity = 12 },
                new WalletDto { Id = 5,  ProductCode = "WAL-0005", Name = "Trifold Classic",  Brand = "Tommy Hilfiger", Gender = GenderType.Men,    Color = "Navy Blue",  Price = 22000m, StockQuantity = 25 },
                new WalletDto { Id = 6,  ProductCode = "WAL-0006", Name = "Mini Crossbody",   Brand = "Kate Spade",     Gender = GenderType.Women,  Color = "Cream",      Price = 29000m, StockQuantity = 18 },
                new WalletDto { Id = 7,  ProductCode = "WAL-0007", Name = "Travel Wallet",    Brand = "Fossil",         Gender = GenderType.Unisex, Color = "Dark Brown", Price = 25000m, StockQuantity = 10 },
                new WalletDto { Id = 8,  ProductCode = "WAL-0008", Name = "Velcro Sport",     Brand = "Nike",           Gender = GenderType.Men,    Color = "Black",      Price =  8500m, StockQuantity = 40 },
                new WalletDto { Id = 9,  ProductCode = "WAL-0009", Name = "Coin Purse",       Brand = "Burberry",       Gender = GenderType.Women,  Color = "Beige",      Price = 31000m, StockQuantity = 22 },
                new WalletDto { Id = 10, ProductCode = "WAL-0010", Name = "Magic Wallet",     Brand = "Secrid",         Gender = GenderType.Unisex, Color = "Gunmetal",   Price = 19500m, StockQuantity = 17 },
                new WalletDto { Id = 11, ProductCode = "WAL-0011", Name = "RFID Shield",      Brand = "Tumi",           Gender = GenderType.Men,    Color = "Charcoal",   Price = 55000m, StockQuantity =  8 },
                new WalletDto { Id = 12, ProductCode = "WAL-0012", Name = "Flap Wallet",      Brand = "Michael Kors",   Gender = GenderType.Women,  Color = "Rose Gold",  Price = 34000m, StockQuantity = 14 },
                new WalletDto { Id = 13, ProductCode = "WAL-0013", Name = "Minimalist Slim",  Brand = "Bellroy",        Gender = GenderType.Unisex, Color = "Caramel",    Price = 21000m, StockQuantity = 35 },
                new WalletDto { Id = 14, ProductCode = "WAL-0014", Name = "Checkbook Wallet", Brand = "Levi's",         Gender = GenderType.Men,    Color = "Tan",        Price = 12000m, StockQuantity = 28 },
                new WalletDto { Id = 15, ProductCode = "WAL-0015", Name = "Wristlet Zip",     Brand = "Vera Bradley",   Gender = GenderType.Women,  Color = "Lavender",   Price = 16500m, StockQuantity = 20 },
            };
        }

        public void AddWallet(string name, string brand, GenderType gender, string color, decimal price, int stockQty)
        {
            int newId = Wallets.Max(x => x.Id) + 1;
            string productCode = $"WAL-{newId:D4}";

            Wallets.Add(new WalletDto
            {
                Id = newId,
                ProductCode = productCode,
                Name = name,
                Brand = brand,
                Gender = gender,
                Color = color,
                Price = price,
                StockQuantity = stockQty
            });

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
