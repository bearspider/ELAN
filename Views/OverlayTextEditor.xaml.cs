using EQAudioTriggers.Models;
using Newtonsoft.Json;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EQAudioTriggers.Views
{
    /// <summary>
    /// Interaction logic for OverlayTextEditor.xaml
    /// </summary>
    public partial class OverlayTextEditor : ChromelessWindow,INotifyPropertyChanged
    {
        private OverlayText _overlaytext;
        public OverlayText Overlay { get { return _overlaytext; } set { _overlaytext = value; RaisedOnPropertyChanged("OverlayText"); } }
        public OverlayTextEditor()
        {            
            InitializeComponent();
            _overlaytext = new OverlayText();
            SetBackground((Color)ColorConverter.ConvertFromString(_overlaytext.Faded));
        }
        public OverlayTextEditor(OverlayText overlayText)
        {
            InitializeComponent();
            _overlaytext = overlayText;
            textDemo.Text = _overlaytext.Name;
            this.Width = _overlaytext.WindowWidth;
            this.Height = _overlaytext.WindowHeight;
            this.Left = _overlaytext.WindowX;
            this.Top = _overlaytext.WindowY;
            SetBackground((Color)ColorConverter.ConvertFromString(_overlaytext.Faded));
            comboFont.Text = _overlaytext.Font;
            sliderSize.Value = _overlaytext.Size;
            sliderDelay.Value = _overlaytext.Delay;
            ClrPckerBg.Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_overlaytext.BG));
            ClrPckerFaded.Brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_overlaytext.Faded));            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
        public void SetTheme(string theme)
        {
            SfSkinManager.SetTheme(this, new Theme(theme));
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
        private void ClrPckerBg_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ClrPckerFaded_ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetBackground(ClrPckerFaded.Color);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            _overlaytext.Name = textDemo.Text;
            _overlaytext.Font = comboFont.SelectedItem.ToString();
            _overlaytext.Size = Convert.ToInt32(sliderSize.Value);
            _overlaytext.Delay = Convert.ToInt32(sliderDelay.Value);
            _overlaytext.BG = ClrPckerBg.Color.ToString();
            _overlaytext.Faded = ClrPckerFaded.Color.ToString();
            _overlaytext.WindowHeight = this.Height;
            _overlaytext.WindowWidth = this.Width;
            _overlaytext.WindowX = this.Left;
            _overlaytext.WindowY = this.Top;
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
    }
}
