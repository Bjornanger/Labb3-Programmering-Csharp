using System.IO;
using System.Text.Json;
using System.Windows;
using ShopWithWPF.DataModels.Users;
using ShopWithWPF.Enums;
using Path = System.IO.Path;

namespace ShopWithWPF.Managers;

public static class UserManager
{
    private static readonly IEnumerable<User>? _users = new List<User>();
    private static User _currentUser;

    public static IEnumerable<User>? Users => _users;


    public static User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            CurrentUserChanged?.Invoke();
        }
    }

    public static event Action CurrentUserChanged;//Detta Event hanterar förändring inuti shop/Admin-sektionerna.


    public static bool IsAdminLoggedIn //Inbyggd kontroll för att dirigera Admins rätt.
    {
        get
        {
            if (CurrentUser is null)
            {
                return false;
            }
            return CurrentUser.Type is UserTypes.Admin;
        }
    }
    public static bool IsCustomerLoggedIn ////Inbyggd kontroll för att dirigera Customers rätt.
    {
        get
        {
            if (CurrentUser is null)
            {
                return false;
            }
            return CurrentUser.Type is UserTypes.Customer;
        }
    }

    public static void ChangeCurrentUser(string name, string password, UserTypes types) //Här tilldelas CurrentUser så att i nästa steg kan lokalisera rätt användare med attribut som kundvagn,Inventarie.
    {
        CurrentUser = Users.FirstOrDefault(u => u.Name == name);
    }

    public static void LogOut()//Här sätts den aktiva användaren till noll och man blir dirigerad till Login sektionen igen.

    {
        if (IsAdminLoggedIn)
        {
            MessageBoxResult userChoice = MessageBox.Show("Logout press Yes or add more inventory press No", "Logout", MessageBoxButton.YesNo);

            if (userChoice == MessageBoxResult.Yes)
            {
                MessageBox.Show("Have a nice Day!");
                CurrentUser = null;
            }
            else if (userChoice == MessageBoxResult.No)
            {
                MessageBox.Show("Now go and add some groceries to the shelf.");
            }
        }
        else if (IsCustomerLoggedIn)
        {
            MessageBoxResult userChoice = MessageBox.Show("To Logout press Yes or Continue press No", "Logout", MessageBoxButton.YesNo);

            if (userChoice == MessageBoxResult.Yes)
            {
                MessageBox.Show("Have a nice Day!");
                CurrentUser = null;

            }
            else if (userChoice == MessageBoxResult.No)
            {
                MessageBox.Show("No? Okey then. Keep on doing your stuff.");
            }
        }
    }

    public static async Task SaveUsersToFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Labb3Folder.Json");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "Labb3UserList.Json");
        var jsonOption = new JsonSerializerOptions();
        jsonOption.WriteIndented = true;

        var json = JsonSerializer.Serialize(Users, jsonOption);

        using (var sw = new StreamWriter(filepath))
        {
            sw.WriteLine((json));
        }

    }

    public static async Task LoadUsersFromFile()
    {

        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Labb3Folder.Json");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "Labb3UserList.Json");

        if (File.Exists(filepath))
        {

            var json = await File.ReadAllTextAsync(filepath);
            var deserialisedUser = new List<User>();

            using (var jsonDoc = JsonDocument.Parse(json))
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var jsonElement in jsonDoc.RootElement.EnumerateArray())
                    {

                        User a;
                        switch (jsonElement.GetProperty("Type").GetInt32())
                        {
                            case 0:
                                a = jsonElement.Deserialize<Admin>();
                                deserialisedUser.Add(a);
                                break;
                            case 1:
                                a = jsonElement.Deserialize<Customer>();
                                deserialisedUser.Add(a);
                                break;
                        }
                    }
                }

            foreach (var user in deserialisedUser)
            {
                if (_users is List<User> users)
                {
                    users.Add(user);
                }
            }

        }


    }
}