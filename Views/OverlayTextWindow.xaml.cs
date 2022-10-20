using EQAudioTriggers.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EQAudioTriggers.Views
{
    public class FontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush newbrush = new SolidColorBrush((Color)value);
            return newbrush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class FontInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color mycolor = (Color)value;
            Color inverted = Color.FromArgb(mycolor.A, (byte)(255 - mycolor.R), (byte)(255 - mycolor.G), (byte)(255 - mycolor.B));
            Brush myBrush = new SolidColorBrush((Color)inverted);
            return myBrush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Interaction logic for OverlayTextWindow.xaml
    /// </summary>
    public partial class OverlayTextWindow : Window, INotifyPropertyChanged
    {
        private OverlayText _windowproperties;
        public OverlayText WindowProperties { get { return _windowproperties; } set { _windowproperties = value; RaisedOnPropertyChanged("WindowProperties"); } }
        private ObservableCollection<OverlayTextItem> _overlays;
        public ObservableCollection<OverlayTextItem> Overlays { get { return _overlays; } set { _overlays = value; RaisedOnPropertyChanged("Overlays"); } }

        public OverlayTextWindow()
        {
            InitializeComponent();
            _overlays = new ObservableCollection<OverlayTextItem>();
            _windowproperties = new OverlayText();
            icTriggers.ItemsSource = Overlays;
            DataContext = this;
        }
        public OverlayTextWindow(OverlayText windowproperties)
        {
            InitializeComponent();
            _windowproperties = windowproperties;
            _overlays = new ObservableCollection<OverlayTextItem>();
            icTriggers.ItemsSource = Overlays;
            DataContext = this;
        }
        public async void AddOverlay(OverlayTextItem newoverlay)
        {

            Overlays.Add(newoverlay);
            RaisedOnPropertyChanged("TriggerAdd");
            //Start async process
            await (Task.Delay(new TimeSpan(0, 0, WindowProperties.Delay)));
            //remove trigger
            Overlays.Remove(newoverlay);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
    }
}
