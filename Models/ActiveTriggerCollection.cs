using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace EQAudioTriggers.Models
{
    public class ActiveTriggerCollection : ObservableCollection<EQTrigger>, INotifyPropertyChanged
    {
        private ObservableCollection<EQTrigger> _collection;

        public ActiveTriggerCollection()
        {
            _collection = new ObservableCollection<EQTrigger>();
            CollectionChanged += ActiveTriggerCollection_CollectionChanged;
            PropertyChanged += ActiveTriggerCollection_PropertyChanged;
        }
        public ObservableCollection<EQTrigger> Collection { get { return _collection; } set { _collection = value; RaisedOnPropertyChanged("Collection Changed"); } }
        private void ActiveTriggerCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.Write("Modified a trigger in the Trigger Collection");
        }
        private void ActiveTriggerCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                Console.Write("Adding New Trigger");
            }
            if (e.OldItems != null)
            {
                Console.WriteLine("Modifying Trigger");
            }
            Console.Write("Changing Trigger Collection");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
        public void Refactor(ObservableCollection<EQTrigger> masterlist)
        {
            Collection = new ObservableCollection<EQTrigger>(masterlist.Where(i => i.ActiveCharacters.Count > 0));
            Console.WriteLine($"Updating Active List: Curent Subscribed [{Collection.Count}]");
        }
    }
}
