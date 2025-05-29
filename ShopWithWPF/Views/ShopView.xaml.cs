using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ShopWithWPF.DataModels.Products;
using ShopWithWPF.DataModels.Users;
using ShopWithWPF.Enums;
using ShopWithWPF.Managers;

namespace ShopWithWPF.Views
{
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : INotifyPropertyChanged
    {
        public ObservableCollection<Product> ShopViewProductList { get; set; } = new();

        public ObservableCollection<ProductForShow> CustomerCart { get; set; } = new();//Kopplad till ej Abstrakt-klass för att skriva Kundvagnen till fil.

        public event Action CustomerCartView;//Event för att uppdatera kundvagnen varje gång innehållet förändras.


        private ProductForShow _shopSelectedProduct;
        public ProductForShow ShopSelectedProduct
        {
            get { return _shopSelectedProduct; }
            set
            {
                if (_shopSelectedProduct != value)
                {
                    _shopSelectedProduct = value;
                    OnPropertyChanged();
                }
            }

        }

        private ProductForShow _customerSelectedProduct;
        public ProductForShow CustomerSelectedProduct
        {
            get { return _customerSelectedProduct; }
            set
            {
                if (_customerSelectedProduct != value)
                {
                    _customerSelectedProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        public ShopView()
        {
            InitializeComponent();

            DataContext = this;


            foreach (var pro in ProductManager.Products)//Shoplistans spegling av MAIN-listan i P-M.
            {
                ShopViewProductList.Add(pro);
            }

            CustomerCartView += ShopView_CustomerCartView;

            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;

            ShopComboBox.Items.Add("EveryProduct");//Filtreringsfuktion genom Combobox valet
            foreach (string s in Enum.GetNames(typeof(ProductGenres)))
            {
                ShopComboBox.Items.Add(s);
            }

        }

        private void ShopView_CustomerCartView()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(CustomerCart);
            view.Refresh();
        }

        private void ProductManager_ProductListChanged()//Varje gång MAIN-listan av Produkter förändras uppdateras det även Shopens utbud.
        {
            ShopViewProductList.Clear();
            foreach (var prod in ProductManager.Products)
            {
                ShopViewProductList.Add(prod);
            }
            OnPropertyChanged(nameof(ShopViewProductList));
        }

        private void UserManager_CurrentUserChanged()//Här kontrolleras det om Kunden har en liggande kundvagn öppen. Då förändras innehållet i listan i CartList.
        {

            if (UserManager.CurrentUser is Customer customer)
            {
                if (CustomerCart != null)
                {
                    CustomerCart.Clear();
                    foreach (var product in customer.Cart)
                    {
                        CustomerCart.Add(product);
                    }
                }
                else
                {
                    return;
                }

            }
            OnPropertyChanged(nameof(CustomerCart));
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerSelectedProduct is null)
            {
                return;
            }

            if (CustomerSelectedProduct.Amount == 1)
            {
                CustomerCart.Remove(CustomerSelectedProduct);
                CustomerCartView();
            }
            else if (CustomerCart.Contains(CustomerSelectedProduct))
            {
                CustomerSelectedProduct.Amount--;
                CustomerCartView();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShopSelectedProduct is null)
            {
                return;
            }

            if (!CustomerCart.Contains(ShopSelectedProduct))
            {

                ShopSelectedProduct.Amount = 1;
                CustomerCart.Add(ShopSelectedProduct);
                CustomerCartView();
            }
            else
            {
                ShopSelectedProduct.Amount++;
                CustomerCartView();
            }

            if (UserManager.CurrentUser is Customer customer)//Här läggs varor till i Customers egna Cart till namnet.
            {
                var cartContent = CustomerCart.ToList();
                customer.Cart = cartContent;
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            UserManager.LogOut();
        }

        private void CheckoutBtn_Click(object sender, RoutedEventArgs e)//Här ligger infon om hur mycket slutsumman blir.
        {
                double sum = 0;
                var uniqueProducts = CustomerCart.DistinctBy(p => p.Name);
                foreach (var product in uniqueProducts)
                {
                    var productG = product;
                    var count = CustomerCart.Count(p => p.Name == productG.Name);
                    sum += count * productG.Price * productG.Amount;
                }
                MessageBox.Show($"You have just paid ${sum} Continue to buy stuff or press Logout.");
                CustomerCart.Clear();
        }


        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShopSelectedProduct is null)
            {
                return;
            }
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

        private void ShopSortBtn_OnClick(object sender, RoutedEventArgs e)//Filtreringsmöjligheten i Shopen för att hitta varorna snabbare genom Genre.
        {
            if (ShopComboBox.SelectedItem is null)
            {
                return;
            }

            ShopViewProductList.Clear();
            if (ShopComboBox.SelectedItem.ToString() == "EveryProduct")
            {
                foreach (var product in ProductManager.Products)
                {
                    ShopViewProductList.Add(product);
                    var x = ShopViewProductList.FirstOrDefault(e => e.Name.StartsWith('a'));
                }
            }
            else
            {
                foreach (var product in ProductManager.Products.Where(p => p.Type.ToString() == ShopComboBox.SelectedValue.ToString()))
                {
                    ShopViewProductList.Add(product);
                }
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(ShopViewProductList);
            view.Refresh();
            OnPropertyChanged(nameof(ShopViewProductList));
        }
    }
}
