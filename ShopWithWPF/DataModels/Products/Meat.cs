using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Products;

public class Meat : ProductForShow
{

    public override ProductGenres Type { get; set; }

    public override string ImagePath { get; set; } = "../Image/meat.jfif";//Här tilldelas bilden till rätt produkt. ../Image gör att den söker efter den mappen i hela projektet.
    public Meat(string name, double price, ProductGenres type) : base(name, price, type)
    {
        Type = ProductGenres.Meat;
        Name = name;
        Price = price;
    }
    
}