using EQAudioTriggers.Models;
using Syncfusion.Windows.Shared;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EQAudioTriggers.Views
{
    /// <summary>
    /// Interaction logic for OverlayTextEditor.xaml
    /// </summary>
    public partial class OverlayTextEditor : ChromelessWindow, INotifyPropertyChanged
    {
        private OverlayText _overlaytext;
        public OverlayText Overlay { get { return _overlaytext; } set { _overlaytext = value; RaisedOnPropertyChanged("OverlayText"); } }
        public OverlayTextEditor()
        {
            InitializeComponent();
            Overlay = new OverlayText();
            SetBackground((Color)ColorConverter.ConvertFromString(_overlaytext.Faded));
            overlayeditor.DataContext = Overlay;
            DataContext = this;
        }
        public OverlayTextEditor(OverlayText overlayText)
        {
            InitializeComponent();
            Overlay = overlayText;
            SetBackground((Color)ColorConverter.ConvertFromString(_overlaytext.Faded));
            overlayeditor.DataContext = Overlay;
            DataContext = this;
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
        private Color InvertColor(Color mycolor)
        {
            return Color.FromArgb(mycolor.A, (byte)(255 - mycolor.R), (byte)(255 - mycolor.G), (byte)(255 - mycolor.B));
        }

        private void ClrPckerFaded_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetBackground(ClrPckerFaded.Color);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ChromelessWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    this.DragMove();
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void buttonInverse_Click(object sender, RoutedEventArgs e)
        {
            clrPckerFont.Color = InvertColor(ClrPckerBg.Color);
        }
        private void ChromelessWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Overlay.WindowHeight = this.Height;
            Overlay.WindowWidth = this.Width;
        }
    }
}
