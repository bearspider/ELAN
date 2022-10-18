using EQAudioTriggers.Models;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Syncfusion.Lic.util.encoders;
using Syncfusion.Windows.Controls;
using System.Windows.Threading;

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
            Color inverted = Color.FromArgb(mycolor.A,(byte)(255 - mycolor.R), (byte)(255 - mycolor.G), (byte)(255 - mycolor.B));
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
    public partial class OverlayTextWindow : Window,INotifyPropertyChanged
    {
        private OverlayText _windowproperties;
        private ObservableCollection<OverlayTextItem> _items;
        public OverlayText WindowProperties { get { return _windowproperties; } set { _windowproperties = value; RaisedOnPropertyChanged("WindowProperties"); } }
        public ObservableCollection<OverlayTextItem> Items { get { return _items; } set { _items = value; RaisedOnPropertyChanged("Strings"); } }
        private ObservableCollection<EQTrigger> _triggers;
        public ObservableCollection<EQTrigger> Triggers { get { return _triggers; } set { _triggers = value; RaisedOnPropertyChanged("Triggers"); } }
        private int _duration;
        private ProgressBar _progress;
        private double _progressvalue;
        private DispatcherTimer _dispatcher;
        public int Duration { get { return _duration; } set { _duration = value; RaisedOnPropertyChanged("Duration"); } }
        public ProgressBar Progress { get { return _progress; } set { _progress = value; RaisedOnPropertyChanged("Progress"); } }
        public double ProgressValue { get { return _progressvalue; } set { _progressvalue = value; RaisedOnPropertyChanged("ProgressValue"); } }
        public DispatcherTimer Dispatcher { get { return _dispatcher; } set { _dispatcher = value; RaisedOnPropertyChanged("DispatcherTimer"); } }

        public OverlayTextWindow()
        {
            InitializeComponent();
            _items = new ObservableCollection<OverlayTextItem>();
            _triggers = new ObservableCollection<EQTrigger>();
            _windowproperties = new OverlayText();
            icTriggers.ItemsSource = Triggers;
            _duration = 10;
            _progress = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 1
            };
            _dispatcher = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)//hours,minutes,seconds
            };
            DataContext = this;
        }
        public OverlayTextWindow(OverlayText windowproperties)
        {
            InitializeComponent();
            _windowproperties = windowproperties;
            _items = new ObservableCollection<OverlayTextItem>();
            _triggers = new ObservableCollection<EQTrigger>();
            icTriggers.ItemsSource = Triggers;
            _duration = 10;
            _progress = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 1
            };
            _dispatcher = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, WindowProperties.Delay)//hours,minutes,seconds
            };
            DataContext = this;
        }
        public void AddItem(OverlayTextItem oti)
        {
            Items.Add(oti);
            oti.StartTimer();
            RaisedOnPropertyChanged("AddItem");
        }
        public void AddTrigger(EQTrigger trigger)
        {
            Triggers.Add(trigger);
            RaisedOnPropertyChanged("TriggerAdd");
        }
        public double Minimum
        {
            get { return Progress.Minimum; }
        }
        public double Maximum
        {
            get { return Progress.Maximum; }
        }
        public double GetProgress()
        {
            return Progress.Value;
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Progress.Value--;
            RaisedOnPropertyChanged("Value");
        }
        public void SetTimer(int duration)
        {
            Duration = duration;
            Progress.Value = duration;
            Dispatcher.Tick += DispatcherTimer_Tick;
        }
        public void StartTimer()
        {
            Dispatcher.Start();
        }
        public void StopTimer()
        {
            Dispatcher.Stop();
        }
        public void SetProgress(int minimum, int maximum)
        {
            Progress.Minimum = minimum;
            Progress.Maximum = maximum;
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
