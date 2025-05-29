using System.Windows;
using System.Windows.Controls;
using ShopWithWPF.DataModels.Users;
using ShopWithWPF.Managers;

namespace ShopWithWPF.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()//Rensar fälten på info i View.
        {
            LoginName.Clear();
            LoginPwd.Clear();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)//Kontrollsteg för vem som ska logga in.
        {
            var user = UserManager.Users.FirstOrDefault(u => u.Name == LoginName.Text);//Kollar genom listan om namnet finns.

            if (user != null)
            {
                if (user.Authenticate(LoginPwd.Password))//kontroll på lösenord mot listan i vald kategori.
                {
                    MessageBox.Show("User and password found");
                    UserManager.ChangeCurrentUser(user.Name, user.Password, user.Type);
                }
                else
                {
                    MessageBox.Show("Invalid password.");
                }
            }
            else
            {
                MessageBox.Show("Invalid username.");
            }
        }



        private void RegisterAdminBtn_Click(object sender, RoutedEventArgs e)//Reg för Admin och tilldelning av Type = behörighet.
        {
            if (!UserManager.Users.Any(u => u.Name == RegisterName.Text))
            {
                if (UserManager.Users is List<User> adminList)
                {
                    adminList.Add(new Admin(RegisterName.Text, RegisterPwd.Password));
                    RegisterName.Clear();
                    RegisterPwd.Clear();
                }

                MessageBox.Show("Admin added");
            }
            else
            {
                MessageBox.Show("AdminUser already exists.");
            }
        }


        private void RegisterCustomerBtn_OnClick(object sender, RoutedEventArgs e)//Reg för Customer och tilldelning av Type = behörighet.
        {
            if (!UserManager.Users.Any(u => u.Name == RegisterName.Text))
            {
                if (UserManager.Users is List<User> customerList)
                {

                    customerList.Add(new Customer(RegisterName.Text, RegisterPwd.Password));
                    RegisterName.Clear();
                    RegisterPwd.Clear();
                }
                MessageBox.Show("Customer added");
            }
            else
            {
                MessageBox.Show("CustomerUser already exists.");
            }
        }
    }
}
