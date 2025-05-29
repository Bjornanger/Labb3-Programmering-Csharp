using ShopWithWPF.Managers;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ShopWithWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)//Detta sker innan MainWindow aktiveras så att filer med info laddas upp.
        {
            var tasks = new List<Task>();
            tasks.Add(UserManager.LoadUsersFromFile());
            tasks.Add(ProductManager.LoadProductsFromFile());
            await Task.WhenAll(tasks);

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)//Detta sker när man avslutar programmet genom att tryck X så ingen info förloras.
        {
            var tasks = new List<Task>();
            tasks.Add(UserManager.SaveUsersToFile());
            tasks.Add(ProductManager.SaveProductsToFile());
            await Task.WhenAll(tasks);

            base.OnExit(e);
        }
    }

}
