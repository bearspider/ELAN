using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class ActivatedTriggerCollection : ObservableCollection<ActivatedTrigger>
    {
        private ObservableCollection<ActivatedTrigger> _collection;

        public ActivatedTriggerCollection()
        {
            _collection = new ObservableCollection<ActivatedTrigger>();
            CollectionChanged += ActivatedTriggerCollection_CollectionChanged;
        }

        private void ActivatedTriggerCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                Console.Write("Adding New ActivatedTrigger");
            }
            if (e.OldItems != null)
            {
                Console.WriteLine("Modifying Old - Should be read only");
            }
            Console.Write("Changing ActivatedTrigger Collection");
        }

            public ObservableCollection<ActivatedTrigger> Collection { get { return _collection; } set { _collection = value; } }
    }
}
