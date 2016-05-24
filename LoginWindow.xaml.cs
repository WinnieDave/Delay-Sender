using Microsoft.Practices.ServiceLocation;
using MvvmLight6.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
namespace MvvmLight6
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        public LoginWindow()
        {

            InitializeComponent();  
            ServiceLocator.Current.GetInstance<MainViewModel>().Logged += LoginWindow_Logged;
            ServiceLocator.Current.GetInstance<MainViewModel>().LoggingFailed += LoginWindow_LoggingFailed;
        }

        void LoginWindow_LoggingFailed(object sender, EventArgs e)
        {
            if (ServiceLocator.Current.GetInstance<MainViewModel>().CurrentLanguage == "en")
                this.ShowMessageAsync("Error", "Logging failed.");
            else
                this.ShowMessageAsync("Помилка", "Не вдалося увійти.Спробуйте ще раз.");
        }

        void LoginWindow_Logged(object sender, EventArgs e)
        {
            var b = new MainWindow();  
            this.Close();
            b.Show();

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().Password = (sender as PasswordBox).Password;
        }
    }
}
