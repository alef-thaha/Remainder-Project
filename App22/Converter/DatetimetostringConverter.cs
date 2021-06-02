using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace App22.Converter
{
     public class DatetimetostringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] sep = value.ToString().Split(" ");
            string sepdate = sep[0];
            string septime = sep[1];

            string formatString = parameter as string;
            if (sepdate == DateTimeOffset.MinValue.Date.ToShortDateString() && septime != DateTimeOffset.MinValue.Date.ToShortTimeString())
            {
                return string.Empty;
            }

            else if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(formatString, value);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
