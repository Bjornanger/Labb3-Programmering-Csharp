using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Products;

public abstract class Product
{
    
    public string Name { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public abstract ProductGenres Type { get; set; }

    public virtual string ImagePath { get; set; } = string.Empty;
    public Product(string name, double price, ProductGenres type)
    {
        Name = name;
        Price = price;
        Type = (ProductGenres)type;
    }
}