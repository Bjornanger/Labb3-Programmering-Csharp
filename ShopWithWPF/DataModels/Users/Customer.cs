using ShopWithWPF.DataModels.Products;
using ShopWithWPF.Enums;

namespace ShopWithWPF.DataModels.Users;

public class Customer : User
{
    public override UserTypes Type { get; } = UserTypes.Customer;//Så att nyregistrerade och befintliga profiler med Customer får rätt behörighet.

  


    public Customer(string name, string password) : base(name, password)
    {
       _cart = new List<ProductForShow>();
    }
    
}