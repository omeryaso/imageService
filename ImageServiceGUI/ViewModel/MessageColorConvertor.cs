using Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.ViewModel
{
    class MessageColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("Only Brush type is allowd");

            MessageTypeEnum messageType = (MessageTypeEnum)value;
            switch (messageType)
            {
                case MessageTypeEnum.INFO:
                    return Brushes.Green;
                case MessageTypeEnum.FAIL:
                    return Brushes.Red;
                case MessageTypeEnum.WARNING:
                    return Brushes.Yellow;
                default:
                    return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
