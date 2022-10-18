using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using EQAudioTriggers.Views;

namespace EQAudioTriggers.Models
{
    public class OverlayTextItem : INotifyPropertyChanged
    {
        private EQTrigger _trigger;
        private OverlayTextWindow _otw;
        private int _fontsize;
        private string _fontcolor;
        private string _fontfamily;
        private int _duration;
        private ProgressBar _progress;
        private double _progressvalue;
        private DispatcherTimer _dispatcher;

        public EQTrigger Trigger { get { return _trigger; } set { _trigger = value; NotifyPropertyChanged("Trigger"); } }
        public int FontSize { get { return _fontsize; } set { _fontsize = value; NotifyPropertyChanged("FontSize"); } }
        public String FontColor { get { return _fontcolor; } set { _fontcolor = value; NotifyPropertyChanged("FontColor"); } }
        public String FontFamily { get { return _fontfamily; } set { _fontfamily = value; NotifyPropertyChanged("FontFamily"); } }
        public int Duration { get { return _duration; } set { _duration = value; NotifyPropertyChanged("Duration"); } }
        public ProgressBar Progress { get { return _progress; } set { _progress = value; NotifyPropertyChanged("Progress"); } }
        public double ProgressValue { get { return _progressvalue; } set { _progressvalue = value; NotifyPropertyChanged("ProgressValue"); } }
        public DispatcherTimer Dispatcher { get { return _dispatcher; } set { _dispatcher = value; NotifyPropertyChanged("DispatcherTimer"); } }
        public OverlayTextWindow OTW { get { return _otw; } set { _otw = value; NotifyPropertyChanged("OTW"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public OverlayTextItem()
        {
            _trigger = new EQTrigger();
            _dispatcher = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)//hours,minutes,seconds
            };
            _fontsize = 20;
            _fontcolor = "black";
            _fontfamily = "Seqoe UI";
            _duration = 10;
            _progress = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 1
            };
        }
        public OverlayTextItem(EQTrigger trigger, OverlayTextWindow otw)
        {
            Trigger = trigger;
            OTW = otw;
            FontSize = otw.WindowProperties.Size;
            FontFamily = otw.WindowProperties.Font;
            FontColor = otw.WindowProperties.FontColor;
            Duration = otw.WindowProperties.Delay;
        }
        public OverlayTextItem(EQTrigger trigger)
        {
            _trigger = trigger;
            _dispatcher = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)//hours,minutes,seconds
            };
            _fontsize = 20;
            _fontcolor = "black";
            _fontfamily = "Seqoe UI";
            _duration = 10;
            _progress = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 1
            };
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
            NotifyPropertyChanged("Value");
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
    }
}
