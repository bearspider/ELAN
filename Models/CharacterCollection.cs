using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EQAudioTriggers.Models
{
    public class CharacterCollection : INotifyPropertyChanged
    {
        private string _name;
        private Character _characterprofile;
        private ObservableCollection<string> _activetriggers;

        public CharacterCollection()
        {
            _name = "";
            _characterprofile = new Character();
            _activetriggers = new ObservableCollection<string>();
        }
        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public Character CharacterProfile { get { return _characterprofile; } set { _characterprofile = value; NotifyPropertyChanged("CharacterProfile"); } }
        public ObservableCollection<string> ActiveTriggers { get { return _activetriggers; } set { _activetriggers = value; NotifyPropertyChanged("ActiveTriggers"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
