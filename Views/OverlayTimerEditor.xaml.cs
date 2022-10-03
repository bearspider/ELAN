using EQAudioTriggers.Models;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// Interaction logic for OverlayTimerEditor.xaml
    /// </summary>
    public partial class OverlayTimerEditor : ChromelessWindow,INotifyPropertyChanged
    {
        private OverlayTimer _ot;
        public OverlayTimer OT { get { return _ot; } set { _ot = value; RaisedOnPropertyChanged("OT"); } }
        public OverlayTimerEditor()
        {
            InitializeComponent();
            _ot = new OverlayTimer();
            DataContext = this;
            SetBackground((Color)ColorConverter.ConvertFromString(_ot.Faded));
        }

        public OverlayTimerEditor(OverlayTimer ot)
        {
            InitializeComponent();
            _ot = ot;
            DataContext = this;
            SetBackground((Color)ColorConverter.ConvertFromString(_ot.Faded));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
        private void SetBackground(Color bgcolor)
        {
            //Can't bind this control because it's normally transparent and we need to see it.
            Brush brush = new SolidColorBrush((Color)bgcolor);
            if (brush.ToString() == "#00FFFFFF")
            {
                brush = Brushes.LightGray;
                this.Opacity = 0.7;
            }
            else
            {
                this.Opacity = 1.0;
            }
            this.Background = brush;
        }
        public void SetTheme(string theme)
        {
            SfSkinManager.SetTheme(this, new Theme(theme));
        }
        private Color InvertColor(Color mycolor)
        {
            return Color.FromArgb(mycolor.A, (byte)(255 - mycolor.R), (byte)(255 - mycolor.G), (byte)(255 - mycolor.B));
        }
        private void ClrPckerBg_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //SetBackground(ClrPckerBg.Color);
            RaisedOnPropertyChanged("Background");
        }

        private void ClrPckerFaded_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //SetBackground(ClrPckerFaded.Color);
            RaisedOnPropertyChanged("Faded");
        }

        private void ChromelessWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    this.DragMove();
                }
                catch(Exception ex)
                {

                }                
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void comboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buttonInvert_Click(object sender, RoutedEventArgs e)
        {
            clrPckrFont.Color = InvertColor(ClrPckerBg.Color);
        }
    }
}
