using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Timers;
using System.Reflection;
using System.Windows;

namespace EQAudioTriggers.Models
{
    public class TriggerTimer : INotifyPropertyChanged
    {
        public string Character { get { return _character; } set { _character = value; NotifyPropertyChanged("Character"); } }
        //public DispatcherTimer WindowTimer;
        public String TimerDescription { get { return _timerdescription; } set { _timerdescription = value; NotifyPropertyChanged("TimerDescription"); } }
        public int TimerDuration { get { return _timerduration; } set { _timerduration = value; NotifyPropertyChanged("TimerDuration"); } }
        public ProgressBar Progress { get { return _progress; } set { _progress = value; NotifyPropertyChanged("Progress"); } }
        public Boolean Direction { get { return _direction; } set { _direction = value; NotifyPropertyChanged("Direction"); } }//false is count down, true is count up
        public double ProgressValue { get { return _progressvalue; } set { _progressvalue = value; NotifyPropertyChanged("ProgressValue"); } }
        public string Barcolor { get { return _barcolor; } set { _barcolor = value; NotifyPropertyChanged("Barcolor"); } }
        public string Textcolor { get { return _textcolor; } set { _textcolor = value; NotifyPropertyChanged("Textcolor"); } }
        public string TriggerId { get { return _triggerid; } set { _triggerid = value; NotifyPropertyChanged("TriggerId"); } }
        public string Id { get { return _id; } set { _id = value; NotifyPropertyChanged("id"); } }
        public System.Timers.Timer WindowTimer { get { return _windowtimer; } set { _windowtimer = value; NotifyPropertyChanged("Windowtimer"); } }
        public DateTime TriggeredTime { get { return _triggeredtime; } set { _triggeredtime = value; NotifyPropertyChanged("TriggeredTime"); } }

        private DateTime _triggeredtime;
        private string _character;
        private System.Timers.Timer _windowtimer;
        private string _timerdescription;
        private int _timerduration;
        private ProgressBar _progress;
        private Boolean _direction;
        private double _progressvalue;
        private string _textcolor;
        private string _triggerid;
        private string _id;
        private string _barcolor;
        public event PropertyChangedEventHandler PropertyChanged;

        public TriggerTimer()
        {
            _windowtimer = new System.Timers.Timer();
            _windowtimer.Interval = 1000; //interval every 1 second
            _windowtimer.Elapsed += _windowtimer_Elapsed;
            _triggeredtime = DateTime.Now;
            _timerdescription = "New Timer";
            _timerduration = 0;
            _direction = false;
            _character = "";
            _barcolor = "";
            _textcolor = "";
            _triggerid = "";
            _id = "";
            _progress = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 1
            };
        }

        private void _windowtimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Tick down");
        }
        private void Tick()
        {
            TimeSpan elapsed = TriggeredTime - DateTime.Now;
            if (Direction)
            {
                Console.WriteLine(Convert.ToInt32(Math.Abs(elapsed.TotalSeconds)));
                Progress.Value = Convert.ToInt32(Math.Abs(elapsed.TotalSeconds));
            }
            else
            {
                Progress.Value = Convert.ToInt32(elapsed.TotalSeconds) + TimerDuration;

            }
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
            TimeSpan elapsed = TriggeredTime - DateTime.Now;

            if (Direction)
            {
                Console.WriteLine(Convert.ToInt32(Math.Abs(elapsed.TotalSeconds)));
                Progress.Value = Convert.ToInt32(Math.Abs(elapsed.TotalSeconds));
            }
            else
            {
                Progress.Value = Convert.ToInt32(elapsed.TotalSeconds) + TimerDuration;

            }

            NotifyPropertyChanged("Value");
        }
        public void SetTimer(String description, int duration, Boolean count)
        {
            //count: true = count up, false = count down
            TimerDescription = description;
            TimerDuration = duration;
            if (count)
            {
                Progress.Value = 0;
            }
            else
            {
                Progress.Value = duration;
            }
            Direction = count;
            
        }
        public void StartTimer()
        {
            WindowTimer.Start();
        }
        public void StopTimer()
        {
            WindowTimer.Stop();
        }
        public void SetProgress(int minimum, int maximum)
        {
            Progress.Minimum = minimum;
            Progress.Maximum = maximum;
        }

        void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
