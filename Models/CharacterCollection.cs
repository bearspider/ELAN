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
    public class CharacterCollection : ObservableCollection<Character>, INotifyPropertyChanged
    {
        private ObservableCollection<Character> _collection;

        public CharacterCollection()
        {
            _collection = new ObservableCollection<Character>();
            CollectionChanged += OnCollectionChanged;
        }
        public event PropertyChangedEventHandler PropertyChanged;
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
        public ObservableCollection<Character> Collection { get { return _collection; } set { _collection = value; } }

        public Character CreateCharacter()
        {
            CharacterEdit chareditor = new CharacterEdit();
            Boolean rval = (bool)chareditor.ShowDialog();
            if(chareditor.ReturnChar != null)
            {
                return chareditor.ReturnChar;
            }
            else
            {
                return null;
            }
        }
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
    }
}
