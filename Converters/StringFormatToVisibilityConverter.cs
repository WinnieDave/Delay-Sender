using Microsoft.Practices.ServiceLocation;
using MvvmLight6.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
namespace MvvmLight6.Converters
{
    class StringFormatToVisibilityConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var input = value as string;
            if (input != null && Regex.IsMatch(input, @"^(\.?[a-zA-Z0-9])+@mail.ru$"))
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
