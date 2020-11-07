using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class TriggerGroupProperty : NotificationObject,INotifyPropertyChanged
    {
        private string _name;
        private string _comments;
        private Boolean _defaultenabled;
        private string _fullpath;

        public TriggerGroupProperty()
        {
            _name = "";
            _comments = "";
            _defaultenabled = false;
            _fullpath = "";
        }

        public string FullPath
        {
            get { return _fullpath; }
            set
            {
                _fullpath = value;
                RaisedOnPropertyChanged("FullPath");
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
