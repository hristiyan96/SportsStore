using SportsStore.Core.Entities;

namespace SportsStore.Data.Seeding;

public static class CatalogSeed
{
    public static readonly DateTime SeededOn = new(2026, 11, 18, 0, 0, 0, DateTimeKind.Utc);

    public static Category[] Categories { get; } =
    [
        new() { Id = 1, Name = "Footwear", Description = "Boots, trainers, and court shoes.", IsActive = true, CreatedOn = SeededOn },
        new() { Id = 2, Name = "Team kit", Description = "Jerseys, shorts, and training tops.", IsActive = true, CreatedOn = SeededOn },
        new() { Id = 3, Name = "Equipment", Description = "Balls, rackets, and match equipment.", IsActive = true, CreatedOn = SeededOn },
        new() { Id = 4, Name = "Accessories", Description = "Bags and the small items that leave the kit bag first.", IsActive = true, CreatedOn = SeededOn }
    ];

    public static Brand[] Brands { get; } =
    [
        new() { Id = 1, Name = "Nike", Country = "USA", IsActive = true, CreatedOn = SeededOn },
        new() { Id = 2, Name = "Adidas", Country = "Germany", IsActive = true, CreatedOn = SeededOn },
        new() { Id = 3, Name = "Wilson", Country = "USA", IsActive = true, CreatedOn = SeededOn },
        new() { Id = 4, Name = "Select", Country = "Denmark", IsActive = true, CreatedOn = SeededOn }
    ];

    public static Supplier[] Suppliers { get; } =
    [
        new()
        {
            Id = 1,
            Name = "Balkan Sports Supply",
            Email = "orders@balkansports.example",
            Phone = "+359 2 555 0101",
            Address = "18 Warehouse Road, Sofia",
            IsActive = true,
            CreatedOn = SeededOn
        },
        new()
        {
            Id = 2,
            Name = "Pitchline Distribution",
            Email = "desk@pitchline.example",
            Phone = "+359 2 555 0190",
            Address = "4 Depot Street, Plovdiv",
            IsActive = true,
            CreatedOn = SeededOn
        }
    ];

    public static Product[] Products { get; } =
    [
        Product(1, "Nike Mercurial Firm Ground Boots", "Lightweight boots for firm natural grass.", "BOOT-NIKE-MRC", 189.90m, 14, 1, 1, 1),
        Product(2, "Adidas Predator Firm Ground Boots", "A stable boot for players who want grip through the turn.", "BOOT-ADI-PRD", 179.00m, 11, 1, 2, 1),
        Product(3, "Nike Revolution Road Trainers", "Daily trainers for easy runs and warm-ups.", "SHOE-NIKE-REV", 89.90m, 22, 1, 1, 1),
        Product(4, "Adidas Court Style Shoes", "Indoor court shoes with a flat sole for basketball and handball.", "SHOE-ADI-CRT", 99.00m, 16, 1, 2, 2),
        Product(5, "Home Match Jersey", "Short-sleeve match jersey for a club home kit.", "KIT-ADI-HOME", 49.90m, 40, 2, 2, 2),
        Product(6, "Away Match Jersey", "Short-sleeve match jersey for a club away kit.", "KIT-ADI-AWAY", 49.90m, 36, 2, 2, 2),
        Product(7, "Training Shorts", "Light shorts for sessions on the pitch or in the hall.", "KIT-NIKE-SHT", 24.90m, 48, 2, 1, 1),
        Product(8, "Wilson Indoor Basketball", "Size 7 ball for indoor courts.", "BALL-WIL-BB7", 39.90m, 18, 3, 3, 2),
        Product(9, "Select Match Football", "A match ball for natural and artificial grass.", "BALL-SEL-FB", 29.90m, 30, 3, 4, 1),
        Product(10, "Wilson Tennis Racket", "An adult racket for club players.", "RKT-WIL-TEN", 119.00m, 9, 3, 3, 2),
        Product(11, "Nike Team Kit Bag", "A holdall for boots, kit, and a change of clothes.", "BAG-NIKE-TEAM", 44.90m, 15, 4, 1, 1),
        Product(12, "Training Water Bottle", "A 750 ml bottle that fits a standard kit bag pocket.", "ACC-BTL-750", 12.50m, 60, 4, 4, 2)
    ];

    private static Product Product(
        int id,
        string name,
        string description,
        string sku,
        decimal price,
        int stock,
        int categoryId,
        int brandId,
        int supplierId)
    {
        return new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Sku = sku,
            Price = price,
            StockQuantity = stock,
            IsActive = true,
            CategoryId = categoryId,
            BrandId = brandId,
            SupplierId = supplierId,
            CreatedOn = SeededOn
        };
    }
}
