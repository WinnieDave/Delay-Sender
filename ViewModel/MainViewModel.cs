using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmLight6.Model;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using ImapX;
using ImapX.Authentication;
using System.Threading;
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
        #region Fields
        private string from;
        private string to;
        private string subject;
        private string body;
        private string password;
        private DateTime when;
        #endregion
        #region Events
        //ивенты для сообщения о результате отправки письма(возбуждалки внизу)
        public event EventHandler MessageSent;
        public event EventHandler MessageSendingFailed;
        public event EventHandler Logged;
        public event EventHandler LoggingFailed;
        #endregion
        #region Commands
        public RelayCommand RunTimer { get; set; }
        public RelayCommand ChangeLanguageToUkrainian { get; set; }
        public RelayCommand ChangeLanguageToEnglish { get; set; }
        public RelayCommand Login { get; set; }
        #endregion
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
                    Login = new RelayCommand(tryLogin);
                });
        }
        #region Properties
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
        /// Текущий язык прилаги
        /// </summary>
        public string CurrentLanguage { get; set; }
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
        #endregion
        #region EventRaisers
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
        protected virtual void RaiseLogged()
        {
            if (Logged != null)
                Logged(this, EventArgs.Empty);
        }
        protected virtual void RaiseLoggingFailed()
        {
            if (LoggingFailed != null)
                LoggingFailed(this, EventArgs.Empty);
        }
        #endregion
        private async void Foo()
        {
           await Timer();
        }
        #region LanguageFunctions
        private void ChangeLanguageToUkr()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new System.Uri(@"Languages\Ukrainian.xaml", System.UriKind.Relative);
            App.Current.Resources.MergedDictionaries.Add(dict);
            CurrentLanguage = "ua";
        }

        private void ChangeLanguageToEngl()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new System.Uri(@"Languages\English.xaml", System.UriKind.Relative);
            App.Current.Resources.MergedDictionaries.Add(dict);
            CurrentLanguage = "en";
        }
        #endregion
        #region CommonFuctions
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
        /// Меняет видимость тайм пикера
        /// </summary>
        /// <summary>
        /// Функция отправки письма
        /// </summary>
        private async void Send()
        {    
            try
            {
                var mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(From);
                mail.To.Add(To);
                mail.Subject = Subject;
                mail.Body = Body;
                var client = new SmtpClient("smtp.ukr.net");
                client.Port = 465;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(From, Password);
                await client.SendMailAsync(mail);
                RaiseMessageSent();
            }
            catch (Exception e)
            {
                RaiseMessageSendingFailed();
            }
        }
        private  void tryLogin()
        {
            var cl = new ImapClient("imap.ukr.net",true,false);
            cl.Host = "imap.ukr.net";
            cl.Port = 993;
            try
            {
                if (cl.Connect())
                {
                    if (!cl.Login(From, Password))
                    {
                        RaiseLogged();
                        return;
                    }
                    RaiseLoggingFailed();
                    return;
                } 
                RaiseLoggingFailed();
            }
            catch(Exception)
            {
                RaiseLoggingFailed();
            }
        }
        #endregion
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