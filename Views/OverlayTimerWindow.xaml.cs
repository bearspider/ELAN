using EQAudioTriggers.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace EQAudioTriggers.Views
{
    /// <summary>
    /// Interaction logic for OverlayTimers.xaml
    /// </summary>
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan t = TimeSpan.FromSeconds(System.Convert.ToInt32(value.ToString()));
            return t.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class OverlayTimerWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TriggerTimer> _timers = new ObservableCollection<TriggerTimer>();
        private OverlayTimer _windowproperties = new OverlayTimer();

        public ObservableCollection<TriggerTimer> TimerBars
        {
            get { return _timers; }
            set { _timers = value; RaisedOnPropertyChanged("TimerBars"); }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; RaisedOnPropertyChanged("Id"); }
        }
        public OverlayTimer WindowProperties { get { return _windowproperties; } set { _windowproperties = value; RaisedOnPropertyChanged("WindowProperties"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
        public OverlayTimerWindow()
        {
            InitializeComponent();
            WindowProperties = new OverlayTimer();
            TimerBars = new ObservableCollection<TriggerTimer>();
            var listener = OcPropertyChangedListener.Create(_timers);
            listviewTimers.ItemsSource = TimerBars;
            DataContext = this;
        }
        public OverlayTimerWindow(OverlayTimer overlay)
        {
            InitializeComponent();
            _windowproperties = overlay;
            _timers = new ObservableCollection<TriggerTimer>();
            var listener = OcPropertyChangedListener.Create(_timers);
            listviewTimers.ItemsSource = TimerBars;
            DataContext = this;
        }
        public async void AddTimer(EQTrigger firedtrigger, Character character, Category triggeredcategory)
        {
            //type: true = count up, false = count down
            Boolean direction = true;
            if (firedtrigger.TimerType == "Count Down")
            {
                direction = false;
            }
            TriggerTimer newTimer = new TriggerTimer();
            newTimer.TriggerId = firedtrigger.Id;
            newTimer.Id = firedtrigger.Id;
            newTimer.Character = character.Name;
            newTimer.SetProgress(0, firedtrigger.TimerSeconds);
            newTimer.SetTimer(firedtrigger.TimerName, firedtrigger.TimerSeconds, direction);
            newTimer.PropertyChanged += Listener_PropertyChanged;            
            newTimer.Barcolor = triggeredcategory.TimerBarColor;
            newTimer.Textcolor = triggeredcategory.TimerFontColor;
            TimerBars.Add(newTimer);
            newTimer.WindowTimer.Start();
            await (Task.Delay(new TimeSpan(0, 0, firedtrigger.TimerSeconds)));
            TimerBars.Remove(newTimer);
        }
        public void ContainsTimer(EQTrigger firedtrigger, Boolean remove)
        {
            List<TriggerTimer> toremove = new List<TriggerTimer>();
            foreach (TriggerTimer timer in TimerBars)
            {
                if (firedtrigger.Id == timer.TriggerId)
                {
                    if (remove)
                    {
                        toremove.Add(timer);
                    }
                }
            }
            if (remove && toremove.Count > 0)
            {
                foreach (TriggerTimer timer in toremove)
                {
                    TimerBars.Remove(timer);
                }
            }
        }
        public void RemoveTimer(String character)
        {
            List<TriggerTimer> toremove = new List<TriggerTimer>();
            foreach (TriggerTimer timer in TimerBars)
            {
                if (timer.Character == character)
                {
                    toremove.Add(timer);
                }
            }
            foreach (TriggerTimer removeitem in toremove)
            {
                TimerBars.Remove(removeitem);
            }
        }
        public void RemoveTimerById(string id)
        {
            List<TriggerTimer> timerlist = TimerBars.ToList();
            TriggerTimer toremove = timerlist.Find(x => x.Id == id);
            TimerBars.Remove(toremove);

        }
        private void Listener_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TriggerTimer s = (TriggerTimer)sender;

            if ((s.Direction && (s.Progress.Value == s.TimerDuration)) || (!(s.Direction) && (s.Progress.Value == 0)))
            {
                s.StopTimer();
                TimerBars.Remove(s);
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

            }
        }

    }
    #region ObservableCollectionListener
    public class OcPropertyChangedListener<T> : INotifyPropertyChanged where T : INotifyPropertyChanged
    {
        private readonly ObservableCollection<T> _collection;
        private readonly string _propertyName;
        private readonly Dictionary<T, int> _items = new Dictionary<T, int>(new ObjectIdentityComparer());
        public OcPropertyChangedListener(ObservableCollection<T> collection, string propertyName = "")
        {
            _collection = collection;
            _propertyName = propertyName ?? "";
            AddRange(collection);
            CollectionChangedEventManager.AddHandler(collection, CollectionChanged);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddRange(e.NewItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveRange(e.OldItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    AddRange(e.NewItems.Cast<T>());
                    RemoveRange(e.OldItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void AddRange(IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                if (_items.ContainsKey(item))
                {

                    _items[item]++;
                }
                else
                {
                    _items.Add(item, 1);
                    PropertyChangedEventManager.AddHandler(item, ChildPropertyChanged, _propertyName);
                }
            }
        }

        private void RemoveRange(IEnumerable<T> oldItems)
        {
            foreach (T item in oldItems)
            {
                _items[item]--;
                if (_items[item] == 0)
                {

                    _items.Remove(item);
                    PropertyChangedEventManager.RemoveHandler(item, ChildPropertyChanged, _propertyName);
                }
            }
        }

        private void Reset()
        {
            foreach (T item in _items.Keys.ToList())
            {
                PropertyChangedEventManager.RemoveHandler(item, ChildPropertyChanged, _propertyName);
                _items.Remove(item);
            }
            AddRange(_collection);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(sender, e);
        }

        private class ObjectIdentityComparer : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return object.ReferenceEquals(x, y);
            }
            public int GetHashCode(T obj)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            }
        }
    }

    public static class OcPropertyChangedListener
    {
        public static OcPropertyChangedListener<T> Create<T>(ObservableCollection<T> collection, string propertyName = "") where T : INotifyPropertyChanged
        {
            return new OcPropertyChangedListener<T>(collection, propertyName);
        }
    }
    #endregion
}
