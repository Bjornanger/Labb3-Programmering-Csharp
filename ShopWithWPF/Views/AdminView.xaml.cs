using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ShopWithWPF.DataModels.Products;
using ShopWithWPF.Enums;
using ShopWithWPF.Managers;

namespace ShopWithWPF.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : INotifyPropertyChanged
    {
        private string _editProductName;

        public string EditProductName
        {
            get { return _editProductName; }
            set
            {
                _editProductName = value;
                OnPropertyChanged();
            }
        }

        private double _editProductPrice;
        public double EditProductPrice
        {
            get { return _editProductPrice; }
            set
            {
                _editProductPrice = value;
                OnPropertyChanged();
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        public static ObservableCollection<Product> AdminProductList { get; set; } = new ();//Direkt spegling av MAIN-listan i P-M.

        public AdminView()
        {
            InitializeComponent();
            DataContext = this;

            foreach (var prod in ProductManager.Products)//Spegling av MAIN-listan i P-M.
            {
                AdminProductList.Add(prod);
            }

            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            
            productComboBox.ItemsSource = Enum.GetValues(typeof(ProductGenres));

            SortingComboBox.Items.Add("EveryProduct");//Filtreringsfuktion genom Combobox valet
            foreach (string s in Enum.GetNames(typeof(ProductGenres)))
            {
                SortingComboBox.Items.Add(s);
            }
        }



        private void ProductManager_ProductListChanged()//Varje gång MAIN-listan av Produkter förändras uppdateras det även här.
        {
            AdminProductList.Clear();
            foreach (var prod in ProductManager.Products)
            {
                AdminProductList.Add(prod);
            }
            OnPropertyChanged(nameof(AdminProductList));
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.IsAdminLoggedIn == true)
            {
                MessageBox.Show("Welcome! We open in 5 minutes. Get the inventory in place asap.");
            }
            else if(UserManager.IsCustomerLoggedIn)
            {
                MessageBox.Show("Welcome to Majorna Exclusive Grocery Store!!");
            }
        }

        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)//För att kunna röra sig fritt i butikens Admin-del.
        {
            SelectedProduct = (Product)ProdList.SelectedItem;
            if (SelectedProduct is null)
            {
                return;
            }
            ProductNameBox.Text = SelectedProduct.Name;
            ProductPriceBox.Text = SelectedProduct.Price.ToString();
            productComboBox.SelectedValue = SelectedProduct.Type;
        }
        private void UpdateBtn_OnClick(object sender, RoutedEventArgs e)//För att kunna ändra produkten utan att behöva lägga till en ytterligare.
        {
            
            if (SelectedProduct != null)
            {
                SelectedProduct.Name = ProductNameBox.Text;
                SelectedProduct.Price = double.Parse(ProductPriceBox.Text);
                SelectedProduct.Type = (ProductGenres)productComboBox.SelectedValue;

            }
            ICollectionView view = CollectionViewSource.GetDefaultView(AdminProductList);
            view.Refresh();
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)//Här läggs alla produkter till i Shopen. Denna metod är kopplad direkt till P-Ms Mainlista.
        {

            if (ProductNameBox.Text is null || ProductPriceBox.Text is null || productComboBox.SelectedItem is null)
            {
                return;
            }
            var productPriceController = double.TryParse(ProductPriceBox.Text, out double newProductPrice);//KOntroll på att man endast skrivetr in siffror

            if (!productPriceController)
            {
                return;
            }

            EditProductName = ProductNameBox.Text;
            EditProductPrice = newProductPrice;

            if (AdminProductList.Any(p => p.Name.ToLower() == EditProductName.ToLower()))//Kontroll så inte några dubletter läggs till.
            {
                MessageBox.Show("This product already exists.");
                return;
            }

            switch ((ProductGenres)productComboBox.SelectedItem)//Här tilldelas alla beståndsdelar i en product och även bild till rätt genre.
            {
                case ProductGenres.Meat:
                    Meat newMeat = new Meat(EditProductName, EditProductPrice, ProductGenres.Meat)
                    {
                        Type = ProductGenres.Meat,
                        Name = EditProductName,
                        Price = EditProductPrice,
                        ImagePath = "../Image/meat.jfif"
                    };
                    ProductManager.AddProduct(newMeat);
                    break;
                case ProductGenres.Fish:
                    Fish newFish = new Fish(EditProductName, EditProductPrice, ProductGenres.Fish)
                    {
                        Type = ProductGenres.Fish,
                        Name = EditProductName,
                        Price = EditProductPrice,
                        ImagePath = "../Image/fish.jfif"
                    };
                    ProductManager.AddProduct(newFish);
                    break;
                case ProductGenres.Dairy:
                    Dairy newDairy = new Dairy(EditProductName, EditProductPrice, ProductGenres.Dairy)
                    {
                        Type = ProductGenres.Dairy,
                        Name = EditProductName,
                        Price = EditProductPrice,
                        ImagePath = "../Image/dairy.jfif"
                    };
                    ProductManager.AddProduct(newDairy);
                    break;
                case ProductGenres.Pantry:
                    Dairy newPantry = new Dairy(EditProductName, EditProductPrice, ProductGenres.Pantry)
                    {
                        Type = ProductGenres.Pantry,
                        Name = EditProductName,
                        Price = EditProductPrice,
                        ImagePath = "../Image/pantry.jfif"
                    };
                    ProductManager.AddProduct(newPantry);
                    break;
                case ProductGenres.Vegetables:
                    Vegetables newVegetable = new Vegetables(EditProductName, EditProductPrice, ProductGenres.Vegetables)
                    {
                        Type = ProductGenres.Vegetables,
                        Name = EditProductName,
                        Price = EditProductPrice,
                        ImagePath = "../Image/vegetable.jfif"
                    };
                    ProductManager.AddProduct(newVegetable);
                    break;
            }
            ProductNameBox.Clear();
            ProductPriceBox.Text = "0";
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)//Här tas alla produkter för Shopen bort. Denna metod är kopplad direkt till P-Ms Mainlista.
        {

            if (SelectedProduct is null)
            {
                return;
            }
            ProductManager.RemoveProduct(SelectedProduct);

        }
        private void SortBtn_OnClick(object sender, RoutedEventArgs e)//Filtrering genom Genre-typ.
        {
            if (SortingComboBox.SelectedItem is null)
            {
                return;
            }
            AdminProductList.Clear();
            if (SortingComboBox.SelectedItem.ToString() == "EveryProduct")
            {
                foreach (var product in ProductManager.Products)
                {
                    AdminProductList.Add(product);
                    var x = AdminProductList.FirstOrDefault(e => e.Name.StartsWith('a'));
                }
            }
            else
            {
                foreach (var product in ProductManager.Products.Where(p => p.Type.ToString() == SortingComboBox.SelectedValue.ToString()))
                {
                    AdminProductList.Add(product);
                }
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(AdminProductList);
            view.Refresh();
            OnPropertyChanged(nameof(AdminProductList));
        }


        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            UserManager.LogOut();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


       
    }
}
