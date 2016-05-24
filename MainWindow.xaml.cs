using System.Windows;
using MvvmLight6.ViewModel;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
namespace MvvmLight6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ServiceLocator.Current.GetInstance<MainViewModel>().MessageSendingFailed += MainWindow_MessageSendingFailed;
            ServiceLocator.Current.GetInstance<MainViewModel>().MessageSent += MainWindow_MessageSent;
        }
        void MainWindow_MessageSent(object sender, System.EventArgs e)
        {
           if(ServiceLocator.Current.GetInstance<MainViewModel>().CurrentLanguage=="en")
            this.ShowMessageAsync("Notification","Message sent");
           else
             this.ShowMessageAsync("Сповіщення", "Листа надіслано");
        }

        void MainWindow_MessageSendingFailed(object sender, System.EventArgs e)
        {
            if (ServiceLocator.Current.GetInstance<MainViewModel>().CurrentLanguage == "en")
                this.ShowMessageAsync("Error", "Can`t send this email.Try again.");
            else
                this.ShowMessageAsync("Помилка", "Не вдалося надіслати листа");
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().Password = (sender as PasswordBox).Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = new LoginWindow();
            b.Show();
            this.Close();
        }
    }
}