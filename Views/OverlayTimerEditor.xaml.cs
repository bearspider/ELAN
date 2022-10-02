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
            comboFont.Text = _ot.Font;
            sliderSize.Value = _ot.Size;
            ClrPckerBg.Color = (Color)ColorConverter.ConvertFromString(_ot.BG);
            ClrPckerFaded.Color = (Color)ColorConverter.ConvertFromString(_ot.Faded);
            ClrPckerEmpty.Color = (Color)ColorConverter.ConvertFromString(_ot.Emptycolor);
            checkTimer.IsChecked = _ot.Showtimer;
            checkStandardize.IsChecked = _ot.Standardize;
            checkGroup.IsChecked = _ot.Group;
            comboSort.Text = _ot.Sortby;
            SetBackground((Color)ColorConverter.ConvertFromString(_ot.Faded));
        }

        public OverlayTimerEditor(OverlayTimer ot)
        {
            InitializeComponent();
            _ot = ot;
            DataContext = this;
            comboFont.Text = _ot.Font;
            sliderSize.Value = _ot.Size;
            ClrPckerBg.Color = (Color)ColorConverter.ConvertFromString(_ot.BG);
            ClrPckerFaded.Color = (Color)ColorConverter.ConvertFromString(_ot.Faded);
            ClrPckerEmpty.Color = (Color)ColorConverter.ConvertFromString(_ot.Emptycolor);
            checkTimer.IsChecked = _ot.Showtimer;
            checkStandardize.IsChecked = _ot.Standardize;
            checkGroup.IsChecked = _ot.Group;
            comboSort.Text = _ot.Sortby;
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
        private void ClrPckerBg_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetBackground(ClrPckerBg.Color);
            RaisedOnPropertyChanged("Background");
        }

        private void ClrPckerFaded_SelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetBackground(ClrPckerFaded.Color);
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
            _ot.Name = textDemo.Text;
            _ot.Font = comboFont.SelectedItem.ToString();
            _ot.Size = Convert.ToInt32(sliderSize.Value);
            _ot.Sortby = comboSort.SelectionBoxItem.ToString();
            _ot.Standardize = (bool)checkStandardize.IsChecked;
            _ot.Group = (bool)checkGroup.IsChecked;
            _ot.Showtimer = (bool)checkTimer.IsChecked;
            _ot.Faded = ClrPckerFaded.Color.ToString();
            _ot.WindowHeight = this.Height;
            _ot.WindowWidth = this.Width;
            _ot.WindowX = this.Left;
            _ot.WindowY = this.Top;
            this.DialogResult = true;
            this.Close();
        }

        private void comboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
