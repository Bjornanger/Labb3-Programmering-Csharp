using ShopWithWPF.Managers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopWithWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()//Här startas själv användargränssnittet upp.
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()//Här tilldelas synligheten beroende på vem som loggar in.
        {
            if (UserManager.IsAdminLoggedIn)
            {
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Collapsed;
                AdminTab.Focus();
            }
            else if (UserManager.IsCustomerLoggedIn)
            {
                ShopTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Collapsed;
                ShopTab.Focus();
            }
            else//Denna flik styr när någon har Loggat ut så att man kan logga in igen som en ny användare.
            {
                ShopTab.Visibility = Visibility.Collapsed;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Visible;
                LoginTab.Focus();
            }
        }
    }
}