using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EQAudioTriggers.Models
{
    public class EQTrigger : INotifyPropertyChanged
    {
        #region private definitions
        private string _name;
        private string _searchtext;
        private ObservableCollection<String> _activecharacters;
        private string _path;
        private Boolean _useregex;
        private Boolean _fastcheck;
        private string _category;
        private string _comments;
        private Boolean _usebasictext;
        private Boolean _useclipboardtext;
        private string _basictext;
        private string _basicclipboardtext;
        private Boolean _radiobasicnosound;
        private Boolean _radiobasictts;
        private Boolean _radiobasicplay;
        private Boolean _usebasicinterrupt;
        private string _basictts;
        private string _basicplayfile;
        private string _timertype;
        private string _timername;
        private int _timerhours;
        private int _timerminutes;
        private int _timerseconds;
        private string _timertriggered;
        private ObservableCollection<EndEarlyTrigger> _endearlytriggers;
        private Boolean _endingnotify;
        private int _endinghours;
        private int _endingminutes;
        private int _endingseconds;
        private Boolean _useendingtext;
        private Boolean _useendingclipboard;
        private string _endingtext;
        private string _endingclipboard;
        private Boolean _radioendingnosound;
        private Boolean _radioendingtts;
        private Boolean _radioendingsound;
        private string _endingtts;
        private Boolean _endinginterrupt;
        private string _endingsoundfile;
        private Boolean _endednotify;
        private Boolean _useendedtext;
        private Boolean _useendedclipboard;
        private string _endeddisplaytext;
        private string _endedclipboard;
        private Boolean _radioendednosound;
        private Boolean _radioendedtts;
        private Boolean _radioendedsound;
        private string _endedtts;
        private Boolean _endedinterrupt;
        private string _endedsoundfile;
        private Boolean _counterreset;
        private int _resethours;
        private int _resetminutes;
        private int _resetseconds;
        private string _digest;
        private string _activezone;
        private Boolean _global;
        private string _id;
        private Boolean _restartontimerid;
        private string _groupid;
        #endregion

        public EQTrigger()
        {
            _name = "";
            _searchtext = "";
            _activecharacters = new ObservableCollection<string>();
            _endearlytriggers = new ObservableCollection<EndEarlyTrigger>();
            _path = "";
            _timertype = "No Timer";        
            _useregex = false;
            _fastcheck = false;
            _category = "";
            _comments = "";
            _usebasictext = false;
            _useclipboardtext = false;
            _basictext = "";
            _basicclipboardtext = "";
            _radiobasicnosound = true;
            _radiobasictts = false;
            _radiobasicplay = false;
            _usebasicinterrupt = false;
            _basictts = "";
            _basicplayfile = "";
            _timertype = "";
            _timername = "";
            _timerhours = 0;
            _timerminutes = 0;
            _timerseconds = 0;
            _timertriggered = "Do Nothing";
            _endingnotify = false;
            _endinghours = 0;
            _endingminutes = 0;
            _endingseconds = 0;
            _useendingtext = false;
            _useendingclipboard = false;
            _endingtext = "";
            _endingclipboard = "";
            _radioendingnosound = true;
            _radioendingtts = false;
            _radioendingsound = false;
            _endingtts = "";
            _endinginterrupt = false;
            _endingsoundfile = "";
            _endednotify = false;
            _useendedtext = false;
            _useendedclipboard = false;
            _endeddisplaytext = "";
            _endedclipboard = "";
            _radioendednosound = true;
            _radioendedtts = false;
            _radioendedsound = false;
            _endedtts = "";
            _endedinterrupt = false;
            _endedsoundfile = "";
            _counterreset = false;
            _resethours = 0;
            _resetminutes = 0;
            _resetseconds = 0;
            _global = true;
            _activezone = "";
            _id = Guid.NewGuid().ToString();
            _restartontimerid = false;
            _groupid = "";
        }
        public EQTrigger(EQTrigger copyfrom)
        {
            _name = copyfrom.Name;
            _searchtext = copyfrom.SearchText;
            _activecharacters = copyfrom.ActiveCharacters;
            _endearlytriggers = copyfrom.EndEarlyTriggers;
            _path = copyfrom.Path;
            _timertype = copyfrom.TimerType;
            _useregex = copyfrom.UseRegex;
            _fastcheck = copyfrom.FastCheck;
            _category = copyfrom.Category;
            _comments = copyfrom.Comments;
            _usebasictext = copyfrom.UseBasicText;
            _useclipboardtext = copyfrom.UseClipboardText;
            _basictext = copyfrom.BasicText;
            _basicclipboardtext = copyfrom.BasicClipboardText;
            _radiobasicnosound = copyfrom.RadioBasicNoSound;
            _radiobasictts = copyfrom.RadioBasicTTS;
            _radiobasicplay = copyfrom.RadioBasicPlay;
            _usebasicinterrupt = copyfrom.UseBasicInterrupt;
            _basictts = copyfrom.BasicTTS;
            _basicplayfile = copyfrom.BasicPlayFile;
            _timertype = copyfrom.TimerType;
            _timername = copyfrom.TimerName;
            _timerhours = copyfrom.TimerHours;
            _timerminutes = copyfrom.TimerMinutes;
            _timerseconds = copyfrom.TimerSeconds;
            _timertriggered = copyfrom.TimerTriggered;
            _endingnotify = copyfrom.EndingNotify;
            _endinghours = copyfrom.EndingHours;
            _endingminutes = copyfrom.EndingMinutes;
            _endingseconds = copyfrom.EndingSeconds;
            _useendingtext = copyfrom.UseEndingText;
            _useendingclipboard = copyfrom.UseEndingClipboard;
             _endingtext = copyfrom.EndingText;
            _endingclipboard = copyfrom.EndingClipboard;
            _radioendingnosound = copyfrom.RadioEndingNoSound;
            _radioendingtts = copyfrom.RadioEndingTTS;
            _radioendingsound = copyfrom.RadioEndingSound;
            _endingtts = copyfrom.EndingTTS;
            _endinginterrupt = copyfrom.EndingInterrupt;
            _endingsoundfile = copyfrom.EndingSoundFile;
            _endednotify = copyfrom.EndedNotify;
            _useendedtext = copyfrom.UseEndedText;
            _useendedclipboard = copyfrom.UseEndedClipboard;
            _endeddisplaytext = copyfrom.EndedDisplayText;
            _endedclipboard = copyfrom.EndedClipboard;
            _radioendednosound = copyfrom.RadioEndedNoSound;
            _radioendedtts = copyfrom.RadioEndedTTS;
            _radioendedsound = copyfrom.RadioEndedSound;
            _endedtts = copyfrom.EndedTTS;
            _endedinterrupt = copyfrom.EndedInterrupt;
            _endedsoundfile = copyfrom.EndedSoundFile;
            _counterreset = copyfrom.CounterReset;
            _resethours = copyfrom.ResetHours;
            _resetminutes = copyfrom.ResetMinutes;
            _resetseconds = copyfrom.ResetSeconds;
            _global = copyfrom.Global;
            _activezone = copyfrom.ActiveZone;
            _id = Guid.NewGuid().ToString();
            _restartontimerid = copyfrom.RestartOnTimerId;
            _groupid = copyfrom.GroupId;
        }
        #region Public Access
        public string GroupId
        {
            get { return _groupid; }
            set
            {
                _groupid = value;
                RaisedOnPropertyChanged("GroupId");
            }
        }
        public Boolean RestartOnTimerId
        {
            get { return _restartontimerid; }
            set
            {
                _restartontimerid = value;
                RaisedOnPropertyChanged("RestartOnTimerId");
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
        public string ActiveZone
        {
            get { return _activezone; }
            set
            {
                _activezone = value;
                RaisedOnPropertyChanged("ActiveZone");
            }
        }
        public Boolean Global
        {
            get { return _global; }
            set
            {
                _global = value;
                RaisedOnPropertyChanged("Global");
            }
        }
        public string Digest
        {
            get { return _digest; }
            set
            {
                _digest = value;
                RaisedOnPropertyChanged("Digest");
            }
        }
        public int ResetSeconds
        {
            get { return _resetseconds; }
            set { _resetseconds = value; RaisedOnPropertyChanged("ResetSeconds"); }
        }
        public int ResetMinutes
        {
            get { return _resetminutes; }
            set { _resetminutes = value; RaisedOnPropertyChanged("ResetMinutes"); }
        }
        public int ResetHours
        {
            get { return _resethours; }
            set { _resethours = value; RaisedOnPropertyChanged("ResetHours"); }
        }
        public Boolean CounterReset
        {
            get { return _counterreset; }
            set
            {
                _counterreset = value; RaisedOnPropertyChanged("CounterReset");
            }
        }
        public Boolean RadioEndedNoSound
        {
            get { return _radioendednosound; }
            set { _radioendednosound = value; RaisedOnPropertyChanged("RadioEndedNoSound"); }
        }
        public Boolean RadioEndedTTS
        {
            get { return _radioendedtts; }
            set { _radioendedtts = value; RaisedOnPropertyChanged("RadioEndedTTS"); }
        }
        public Boolean RadioEndedSound
        {
            get { return _radioendedsound; }
            set { _radioendedsound = value; RaisedOnPropertyChanged("RadioEndedSound"); }
        }
        public string EndedTTS
        {
            get { return _endedtts; }
            set { _endedtts = value; RaisedOnPropertyChanged("EndedTTS"); }
        }
        public Boolean EndedInterrupt
        {
            get { return _endedinterrupt; }
            set { _endedinterrupt = value; RaisedOnPropertyChanged("EndedInterrupt"); }
        }
        public string EndedSoundFile
        {
            get { return _endedsoundfile; }
            set { _endedsoundfile = value; RaisedOnPropertyChanged("EndedSoundFile"); }
        }
        public String EndedDisplayText
        {
            get { return _endeddisplaytext; }
            set { _endeddisplaytext = value; RaisedOnPropertyChanged("EndedDisplayText"); }
        }
        public String EndedClipboard
        {
            get { return _endedclipboard; }
            set { _endedclipboard = value; RaisedOnPropertyChanged("EndedClipboard"); }
        }
        public Boolean UseEndedText
        {
            get { return _useendedtext; }
            set { _useendedtext = value; RaisedOnPropertyChanged("UseEndedText"); }
        }
        public Boolean UseEndedClipboard
        {
            get { return _useendedclipboard; }
            set { _useendedclipboard = value; RaisedOnPropertyChanged("UseEndedClipboard"); }
        }
        public Boolean EndedNotify
        {
            get { return _endednotify; }
            set { _endednotify = value; RaisedOnPropertyChanged("EndedNotify"); }
        }
        public string EndingSoundFile
        {
            get { return _endingsoundfile; }
            set
            {
                _endingsoundfile = value;
                RaisedOnPropertyChanged("EndingSoundFile");
            }
        }
        public Boolean EndingInterrupt
        {
            get { return _endinginterrupt; }
            set
            {
                _endinginterrupt = value;
                RaisedOnPropertyChanged("EndingInterrupt");
            }
        }
        public string EndingTTS
        {
            get { return _endingtts; }
            set { _endingtts = value; RaisedOnPropertyChanged("EndingTTS"); }
        }
        public Boolean RadioEndingSound
        {
            get { return _radioendingsound; }
            set
            {
                _radioendingsound = value;
                RaisedOnPropertyChanged("RadioEndingSound");
            }
        }
        public Boolean RadioEndingTTS
        {
            get { return _radioendingtts; }
            set
            {
                _radioendingtts = value;
                RaisedOnPropertyChanged("RadioEndingTTS");
            }
        }
        public Boolean RadioEndingNoSound
        {
            get { return _radioendingnosound; }
            set
            {
                _radioendingnosound = value;
                RaisedOnPropertyChanged("RadioEndingNoSound");
            }
        }
        public Boolean UseEndingText
        {
            get { return _useendingtext; }
            set
            {
                _useendingtext = value;
                RaisedOnPropertyChanged("UseEndingText");
            }
        }
        public Boolean UseEndingClipboard
        {
            get { return _useendingclipboard; }
            set
            {
                _useendingclipboard = value;
                RaisedOnPropertyChanged("UseEndingClipboard");
            }
        }
        public String EndingText
        {
            get { return _endingtext; }
            set
            {
                _endingtext = value;
                RaisedOnPropertyChanged("EndingText");
            }
        }
        public String EndingClipboard
        {
            get { return _endingclipboard; }
            set
            {
                _endingclipboard = value;
                RaisedOnPropertyChanged("EndingClipboard");
            }
        }
        public int EndingSeconds
        {
            get { return _endingseconds; }
            set
            {
                _endingseconds = value; RaisedOnPropertyChanged("EndingSeconds");
            }
        }
        public int EndingMinutes
        {
            get { return _endingminutes; }
            set { _endingminutes = value; RaisedOnPropertyChanged("EndingMinutes"); }
        }
        public int EndingHours
        {
            get { return _endinghours; }
            set
            {
                _endinghours = value;
                RaisedOnPropertyChanged("EndingHours");
            }
        }
        public Boolean EndingNotify
        {
            get { return _endingnotify; }
            set
            {
                _endingnotify = value;
                RaisedOnPropertyChanged("EndingNotify");
            }
        }
        public ObservableCollection<EndEarlyTrigger> EndEarlyTriggers
        {
            get { return _endearlytriggers; }
            set
            {
                _endearlytriggers = value;
                RaisedOnPropertyChanged("EndEarlyTriggers");
            }
        }
        public string TimerTriggered
        {
            get { return _timertriggered; }
            set
            {
                _timertriggered = value;
                RaisedOnPropertyChanged("TimerTriggered");
            }
        }
        public int TimerHours
        {
            get { return _timerhours; }
            set
            {
                _timerhours = value; RaisedOnPropertyChanged("TimerHours");
            }
        }
        public int TimerMinutes
        {
            get { return _timerminutes; }
            set
            {
                _timerminutes = value; RaisedOnPropertyChanged("TimerMinutes");
            }
        }
        public int TimerSeconds
        {
            get { return _timerseconds; }
            set
            {
                _timerseconds = value; RaisedOnPropertyChanged("TimerSeconds");
            }
        }
        public string TimerName
        {
            get { return _timername; }
            set { _timername = value; RaisedOnPropertyChanged("TimerName"); }
        }
        public string TimerType
        {
            get { return _timertype; }
            set { _timertype = value;
                RaisedOnPropertyChanged("TimerType");
            }
        }
        public string BasicPlayFile
        {
            get { return _basicplayfile; }
            set
            {
                _basicplayfile = value;
                RaisedOnPropertyChanged("BasicPlayFile");
            }
        }
        public Boolean UseBasicInterrupt
        {
            get { return _usebasicinterrupt; }
            set { _usebasicinterrupt = value; RaisedOnPropertyChanged("UseBasicInterrupt"); }
        }
        public string BasicTTS
        {
            get { return _basictts; }
            set { _basictts = value;RaisedOnPropertyChanged("BasicTTS"); }
        }
        public Boolean RadioBasicNoSound
        {
            get { return _radiobasicnosound; }
            set 
            {
                _radiobasicnosound = value;
                RaisedOnPropertyChanged("RadioBasicNoSound");
            }
        }
        public Boolean RadioBasicTTS
        {
            get { return _radiobasictts; }
            set 
            {
                _radiobasictts = value;
                RaisedOnPropertyChanged("RadioBasicTTS");
            }
        }
        public Boolean RadioBasicPlay
        {
            get { return _radiobasicplay; }
            set 
            {
                _radiobasicplay = value;
                RaisedOnPropertyChanged("RadioBasicPlay");
            }
        }
        public string BasicClipboardText
        {
            get { return _basicclipboardtext; }
            set
            {
                _basicclipboardtext = value;
                RaisedOnPropertyChanged("BasicClipboardText");
            }
        }
        public string BasicText
        {
            get { return _basictext; }
            set
            {
                _basictext = value;
                RaisedOnPropertyChanged("BasicText");
            }
        }
        public Boolean UseBasicText
        {
            get { return _usebasictext; }
            set
            {
                _usebasictext = value;
                RaisedOnPropertyChanged("UseBasicText");
            }
        }
        public Boolean UseClipboardText
        {
            get { return _useclipboardtext; }
            set
            {
                _useclipboardtext = value;
                RaisedOnPropertyChanged("UseClipboardText");
            }
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
        public Boolean FastCheck
        {
            get { return _fastcheck; }
            set
            {
                _fastcheck = value;
                RaisedOnPropertyChanged("FastCheck");
            }
        }
        public String Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisedOnPropertyChanged("Category");
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
        public string Name { get { return _name; } set { _name = value; RaisedOnPropertyChanged("Name"); } }
        public string SearchText { get { return _searchtext; } set { _searchtext = value; RaisedOnPropertyChanged("SearchText"); } }
        public ObservableCollection<string> ActiveCharacters { get { return _activecharacters; } set { _activecharacters = value; RaisedOnPropertyChanged("ActiveCharacters"); } }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisedOnPropertyChanged("Path");
            }
        }
        #endregion
        public void AddCharacter(string newchar)
        {
            ActiveCharacters.Add(newchar);
            RaisedOnPropertyChanged("Added Character");
        }
        public void RemoveCharacter(string oldchar)
        {
            if (ActiveCharacters.Contains(oldchar))
            {
                ActiveCharacters.Remove(oldchar);
                RaisedOnPropertyChanged("RemovedCharacter");
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
