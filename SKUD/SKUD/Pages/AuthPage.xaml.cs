using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SKUD.DataBase;

namespace SKUD.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;

            if (DB.entities.accounts.FirstOrDefault(c => c.login == login && c.password == password) != null)
            {
                account acc = DB.entities.accounts.FirstOrDefault(c => c.login == login && c.password == password);
                string a = DB.entities.employeers.FirstOrDefault(c => c.account.account_id == acc.account_id).department.title;
                if (a == "Администрация")
                    NavigationService.Navigate(new MainPage());

                if (a == "Охрана")
                    NavigationService.Navigate(new SecurityPage());
            }
        }
    }
}
