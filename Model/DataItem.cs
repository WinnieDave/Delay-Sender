using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MvvmLight6.Model
{
    public class DataItem:ObservableObject
    {
        public DataItem()
        {
        }
        private string from;
        private string to;
        private string subject;
        private string body;
        private string password;
        private DateTime when;

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
            get { return from; }
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

        //Письмо
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
    }
}
