using ShopWithWPF.DataModels.Products;
using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Users;

public class Admin : User
{
    public override UserTypes Type { get; } = UserTypes.Admin;//Så att nyregistrerade och befintliga profiler med Admin får rätt behörighet.

    public Admin(string name, string password) : base(name, password)
    {
        _cart = new List<ProductForShow>();
    }
}