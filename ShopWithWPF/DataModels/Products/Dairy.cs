using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Products;

public class Dairy : ProductForShow
{

    public override ProductGenres Type { get; set; }

    public override string ImagePath { get; set; } = "../Image/dairy.jfif";//Här tilldelas bilden till rätt produkt. ../Image gör att den söker efter den mappen i hela projektet.

    public Dairy(string name, double price, ProductGenres type) : base(name, price, type)
    {
        Type = ProductGenres.Dairy;
    }
}