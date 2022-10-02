using EQAudioTriggers.Models;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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

namespace EQAudioTriggers.Views
{
    public class FontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Drawing.Color newcolor = ColorTranslator.FromHtml(value.ToString());
            return newcolor;
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
        
        public OverlayTextWindow()
        {
            InitializeComponent();
            Items = new ObservableCollection<OverlayTextItem>();
            DataContext = this;
        }
        public OverlayTextWindow(OverlayText windowproperties)
        {
            InitializeComponent();
            WindowProperties = windowproperties;
            Items = new ObservableCollection<OverlayTextItem>();
            icTriggers.ItemsSource = Items;
            DataContext = this;
        }
        public void AddItem(OverlayTextItem oti)
        {
            Items.Add(oti);
            oti.StartTimer();
            RaisedOnPropertyChanged("AddItem");
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
