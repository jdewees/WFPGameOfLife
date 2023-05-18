using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WFPGameOfLife
{
    internal class LifeColourConverter : IValueConverter
    {
        public SolidColorBrush AliveColour { get; set; }
        public SolidColorBrush DeadColour { get; set; }

        public LifeColourConverter(SolidColorBrush aliveColour, SolidColorBrush deadColour)
        {
            AliveColour = aliveColour;
            DeadColour = deadColour;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool alive = false;
            if (value is bool)
            {
                alive = (bool)value;
            }
            return alive ? AliveColour : DeadColour;
        
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
            {
                return((SolidColorBrush)value) == AliveColour;
            }

            return false;
        }
    }
}
