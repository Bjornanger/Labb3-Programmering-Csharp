using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Products;

public class ProductForShow(string name, double price, ProductGenres type) : Product(name, price, type)
{
    //private ProductGenres type;

    public override ProductGenres Type { get; set; }

    //public ProductForShow(string name, double price, ProductGenres type) : base(name, price, type)
    //{
    //    Name = name;
    //    Price = price;
    //    this.type = type;
    //}


}