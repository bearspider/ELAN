using EQAudioTriggers.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EQAudioTriggers.Models
{
    public class CharacterCollection : ObservableCollection<EQTrigger>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private string _name;
        private Character _characterprofile;
        private ObservableCollection<EQTrigger> _activetriggers;

        public CharacterCollection()
        {
            _name = "";
            _characterprofile = new Character();
            _activetriggers = new ObservableCollection<EQTrigger>();
            CollectionChanged += OnCollectionChanged;            

        }
        public string Name { get { return _name; } set { _name = value; } }
        public Character CharacterProfile { get { return _characterprofile; } set { _characterprofile = value; } }
        public ObservableCollection<EQTrigger> ActiveTriggers { get { return _activetriggers; } set { _activetriggers = value; } }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                Console.Write("Adding New Items");
            }
            if(e.OldItems != null)
            {
                Console.WriteLine("Modifying Old Items");
            }
            Console.Write("Changing Collection");
        }       
    }
}
