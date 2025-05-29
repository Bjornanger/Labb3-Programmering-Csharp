using System.IO;
using System.Text.Json;
using ShopWithWPF.DataModels.Products;
using ShopWithWPF.Views;

namespace ShopWithWPF.Managers;

public static class ProductManager
{
    private static readonly IEnumerable<Product>? _products = new List<Product>();
    public static IEnumerable<Product>? Products => _products;


    public static event Action ProductListChanged; //Event som gör att listor i Admin/ShopView uppdateras när metoderna add/remove används i P-M.


    public static void AddProduct(Product product)
    {

        if (Products is List<Product> products)
        {
            products.Add(product);
        }

        ProductListChanged.Invoke();
    }

    public static void RemoveProduct(Product product)
    {
        if (Products is List<Product> products)
        {
            products.Remove(product);
        }
        ProductListChanged.Invoke();
    }

    public static async Task SaveProductsToFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb3Folder.Json");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "ProductList.json");
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.WriteIndented = true;

        var json = JsonSerializer.Serialize(AdminView.AdminProductList, jsonOptions);

        using (var sw = new StreamWriter(filepath))
        {
            sw.WriteLine(json);
        };

    }//Sparar ner AdminView's lista då det är här alla förändring för affären sker.

    public static async Task LoadProductsFromFile()
    {

        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb3Folder.Json");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "ProductList.json");

        if (File.Exists(filepath))
        {

            var json = await File.ReadAllTextAsync(filepath);
            var deserialisedProduct = new List<Product>();

            using (var jsonDoc = JsonDocument.Parse(json))
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var jsonElement in jsonDoc.RootElement.EnumerateArray())
                    {
                        Product p;
                        switch (jsonElement.GetProperty("Type").GetInt32())
                        {
                            case 0:
                                p = jsonElement.Deserialize<Meat>();
                                deserialisedProduct.Add(p);
                                break;
                            case 1:
                                p = jsonElement.Deserialize<Fish>();
                                deserialisedProduct.Add(p);
                                break;
                            case 2:
                                p = jsonElement.Deserialize<Vegetables>();
                                deserialisedProduct.Add(p);
                                break;
                            case 3:
                                p = jsonElement.Deserialize<Dairy>();
                                deserialisedProduct.Add(p);
                                break;
                            case 4:
                                p = jsonElement.Deserialize<Pantry>();
                                deserialisedProduct.Add(p);
                                break;
                        }
                    }
                }

            ((List<Product>)Products).Clear();
            foreach (var product in deserialisedProduct)
            {
                if (_products is List<Product> products)
                {
                    products.Add(product);
                }
            }
            ProductListChanged?.Invoke();
        }

    }
}