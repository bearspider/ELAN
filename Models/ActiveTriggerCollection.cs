using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class ActiveTriggerCollection : ObservableCollection<EQTrigger>
    {
        private ObservableCollection<EQTrigger> _collection;

        public ActiveTriggerCollection()
        {
            _collection = new ObservableCollection<EQTrigger>();
            CollectionChanged += ActiveTriggerCollection_CollectionChanged;
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

        public ObservableCollection<EQTrigger> Collection { get { return _collection; } set { _collection = value; } }
    }
}
