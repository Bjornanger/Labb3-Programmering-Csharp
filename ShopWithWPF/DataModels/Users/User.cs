using ShopWithWPF.DataModels.Products;
using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Users;

public abstract class User
{
    public string Name { get; }

    public string Password { get; }

    public abstract UserTypes Type { get; }
    public List<ProductForShow> _cart;

    public List<ProductForShow> Cart
    {
        get { return _cart; }

        set => _cart = value;
    }
    protected User(string name, string password)
    {
        Name = name;
        Password = password;
        _cart = new List<ProductForShow>();

    }

    public bool Authenticate(string password)
    {
        return Password.Equals(password);
    }
}