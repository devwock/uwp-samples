using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Settings
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string time = value as string;
            string[] timeValue = time.Split(":");
            if (time == null || timeValue.Length != 3)
            {
                return new TimeSpan(0, 0, 0);
            }

            int hour = 0;
            int min = 0;
            int sec = 0;

            if (!int.TryParse(timeValue[0], out hour) || !int.TryParse(timeValue[1], out min) || !int.TryParse(timeValue[2], out sec))
            {
                return new TimeSpan(0, 0, 0);
            }

            return new TimeSpan(hour, min, sec);
        }
    }
}