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
    /// Interaction logic for OverlayTimerEditor.xaml
    /// </summary>
    public partial class OverlayTimerEditor : ChromelessWindow, INotifyPropertyChanged
    {
        private OverlayTimer _ot;
        public OverlayTimer OT { get { return _ot; } set { _ot = value; RaisedOnPropertyChanged("OT"); } }
        public OverlayTimerEditor()
        {
            InitializeComponent();
            OT = new OverlayTimer();
            overlayeditor.DataContext = OT;
            DataContext = this;
            SetBackground((Color)ColorConverter.ConvertFromString(_ot.Faded));
        }

        public OverlayTimerEditor(OverlayTimer ot)
        {
            InitializeComponent();
            OT = ot;
            DataContext = this;
            overlayeditor.DataContext = OT;
            SetBackground((Color)ColorConverter.ConvertFromString(OT.Faded));
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
                    //Need this because laptops will say it's not the primary mouse
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void buttonInvert_Click(object sender, RoutedEventArgs e)
        {
            clrPckrFont.Color = InvertColor(ClrPckerBg.Color);
        }

        private void ChromelessWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OT.WindowHeight = this.Height;
            OT.WindowWidth = this.Width;
        }

        private void ClrPckerFaded_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetBackground(ClrPckerFaded.Color);
        }
    }
}
