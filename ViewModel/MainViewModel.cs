using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight6.Model;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmLight6.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private string from;
        private string to;
        private string subject;
        private string body;
        private string password;
        private DateTime when;   
 
        //ивенты для сообщения о результате отправки письма(возбуждалки внизу)
        public event EventHandler MessageSent;
        public event EventHandler MessageSendingFailed;
        /// <summary>
        /// Комманда включения таймера(биндиться кнопка в главном окне)
        /// </summary>
        public RelayCommand RunTimer { get; set; }
        public RelayCommand ChangeLanguageToUkrainian { get; set; }
        public RelayCommand ChangeLanguageToEnglish { get; set; }
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    from = item.From;
                    to = item.To;
                    subject = item.Subject;
                    body = item.Body;
                    password = item.Password;
                    When = item.When;
                    RunTimer = new RelayCommand(Foo);
                    ChangeLanguageToUkrainian = new RelayCommand(ChangeLanguageToUkr);
                    ChangeLanguageToEnglish = new RelayCommand(ChangeLanguageToEngl);
                    
                });
        }  
        


        /// <summary>
        /// Время отправки письма
        /// </summary>
        public DateTime When
        {
            get { return when; }
            set
            {
                if (when == value)
                    return;
                when = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Емайл отправителя
        /// </summary>
        public string From
        {
            get{return from;}
            set
            {
                if (from == value)
                    return;
                from = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Емайл получателя
        /// </summary>
        public string To
        {
            get { return to; }
            set
            {
                if (to == value)
                    return;
                to = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Тема
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set
            {
                if (subject == value)
                    return;
                subject = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Письмо
        /// </summary>
        public string Body
        {
            get { return body; }
            set
            {
                if (body == value)
                    return;
                body = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///Пароль отправителя
        /// </summary>
        public string Password
        {
            get { return password; }
            set
            {
                if (password == value)
                    return;
                password = value;
                RaisePropertyChanged();
            }
        }


        //Возбуждалки для событий отправки/фейла письма
        protected virtual void RaiseMessageSendingFailed()
        {
            if (MessageSendingFailed != null)
                MessageSendingFailed(this, EventArgs.Empty);
        }
        protected virtual void RaiseMessageSent()
        {
            if (MessageSent != null)
                MessageSent(this, EventArgs.Empty);
        }

        /// <summary>
        /// Костыль,чтобы запихнуть таймер в RelayCommand
        /// </summary>
        private async void Foo()
        {
           await Timer();
        }

        /// <summary>
        /// Меняет язык на укр
        /// </summary>
        private void ChangeLanguageToUkr()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new System.Uri(@"Languages\Ukrainian.xaml", System.UriKind.Relative);
            App.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void ChangeLanguageToEngl()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new System.Uri(@"Languages\English.xaml", System.UriKind.Relative);
            App.Current.Resources.MergedDictionaries.Add(dict);
        }

        /// <summary>
        /// Таймер
        /// </summary>
        public async Task Timer()
        {
            var now = DateTime.Now;
            if (When > now)
                await Task.Delay(when - now);
            Send();
        }

        /// <summary>
        /// Функция отправки письма
        /// </summary>
        private void Send()
        {    
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(From);
                mail.To.Add(To);
                mail.Subject = Subject;
                mail.Body = Body;
                var client = new SmtpClient("smtp.mail.ru");
                client.Port = 25;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(From, Password);
                client.Send(mail);
                RaiseMessageSent();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                RaiseMessageSendingFailed();
            }
        }

        public override void Cleanup()
        {
            // Clean up if needed
            this.from = string.Empty;
            this.to = string.Empty;
            this.subject = string.Empty;
            this.body = string.Empty;
            this.password = string.Empty;
            base.Cleanup();
        }
    }
}