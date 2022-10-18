using Syncfusion.Windows.Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EQAudioTriggers.Models
{
    public class TriggerGroupProperty : NotificationObject, INotifyPropertyChanged
    {
        private string _name;
        private string _id;
        private string _comments;
        private Boolean _defaultenabled;
        private ObservableCollection<string> _subgroups;
        private ObservableCollection<string> _triggers;
        private string _parentid;

        public TriggerGroupProperty()
        {
            _name = "";
            _comments = "";
            _defaultenabled = false;
            _id = Guid.NewGuid().ToString();
            _subgroups = new ObservableCollection<string>();
            _triggers = new ObservableCollection<string>();
            _parentid = "";
        }
        public string ParentId
        {
            get { return _parentid; }
            set
            {
                _parentid = value;
                RaisedOnPropertyChanged("ParentId");
            }
        }
        public ObservableCollection<string> Triggers
        {
            get { return _triggers; }
            set
            {
                _triggers = value;
                RaisedOnPropertyChanged("Triggers");
            }
        }
        public ObservableCollection<string> SubGroups
        {
            get { return _subgroups; }
            set
            {
                _subgroups = value;
                RaisedOnPropertyChanged("SubGroups");
            }
        }
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisedOnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisedOnPropertyChanged("Name");
            }
        }
        public string Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                RaisedOnPropertyChanged("Comments");
            }
        }
        public Boolean DefaultEnabled
        {
            get { return _defaultenabled; }
            set
            {
                _defaultenabled = value;
                RaisedOnPropertyChanged("DefaultEnabled");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
    }
}
