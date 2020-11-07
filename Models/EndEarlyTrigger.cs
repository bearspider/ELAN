using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class EndEarlyTrigger : INotifyPropertyChanged
    {
        private string _searchtext;
        private Boolean _useregex;
        public EndEarlyTrigger()
        {
            _searchtext = "";
            _useregex = false;
        }
        public Boolean UseRegex
        {
            get { return _useregex; }
            set
            {
                _useregex = value;
                RaisedOnPropertyChanged("UseRegex");
            }
        }
        public String SearchText
        {
            get { return _searchtext; }
            set
            {
                _searchtext = value;
                RaisedOnPropertyChanged("SearchText");
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
