using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class GINATrigger : INotifyPropertyChanged
    {
        private string _name;
        private string _triggertext;
        private string _comments;
        private Boolean _enableregex;
        private Boolean _usetext;
        private string _displaytext;
        private Boolean _copytoclipboard;
        private string _clipboardtext;
        private Boolean _usetexttovoice;
        private Boolean _interruptspeech;
        private string _texttovoicetext;
        private Boolean _playmediafile;
        private string _timertype;
        private string _timername;
        private Boolean _restartbasedontimername;
        private string _timermilliseconduration;
        private string _timerduration;
        private string _timervisibleduration;
        private string _timerstartbehavior;
        private string _timerendingtime;
        private Boolean _usetimerending;
        private Boolean _usetimerended;
        private Boolean _usecounterresettimer;
        private string _counterresetduration;
        private string _category;
        private string _modified;
        private Boolean _usefastcheck;
        private ObservableCollection<EndEarlyTrigger> _earlyenders;
        
        public GINATrigger()
        {
            Name = "";
            TriggerText = "";
            Comments = "";
            EnableRegex = false;
            UseText = false;
            DisplayText = "";
            CopyToClipboard = false;
            ClipboardText = "";
            UseTextToVoice = false;
            InterruptSpeech = false;
            TextToVoiceText = "";
            PlayMediaFile = false;
            TimerType = "";
            TimerName = "";
            RestartBasedOnTimerName = false;
            TimerMillisecondDuration = "";
            TimerDuration = "";
            TimerVisibleDuration = "";
            TimerStartBehavior = "";
            TimerEndingTime = "";
            UseTimerEnding = false;
            UseTimerEnded = false;
            UseCounterResetTimer = false;
            CounterResetDuration = "";
            Category = "";
            Modified = "";
            UseFastCheck = false;
            TimerEarlyEnders = new ObservableCollection<EndEarlyTrigger>();
        }

        public string Name { get; set; }
        public string TriggerText { get; set; }
        public string Comments { get; set; }
        public Boolean EnableRegex { get; set; }
        public Boolean UseText { get; set; }
        public string DisplayText { get; set; }
        public Boolean CopyToClipboard { get; set; }
        public string ClipboardText { get; set; }
        public Boolean UseTextToVoice { get; set; }
        public Boolean InterruptSpeech { get; set; }
        public string TextToVoiceText { get; set; }
        public Boolean PlayMediaFile { get; set; }
        public string TimerType { get; set; }
        public string TimerName { get; set; }
        public Boolean RestartBasedOnTimerName { get; set; }
        public string TimerMillisecondDuration { get; set; }
        public string TimerDuration { get; set; }
        public string TimerVisibleDuration { get; set; }
        public string TimerStartBehavior { get; set; }
        public string TimerEndingTime { get; set; }
        public Boolean UseTimerEnding { get; set; }
        public Boolean UseTimerEnded { get; set; }
        public Boolean UseCounterResetTimer { get; set; }
        public string CounterResetDuration { get; set; }
        public string Category { get; set; }
        public string Modified { get; set; }
        public Boolean UseFastCheck { get; set; }
        public ObservableCollection<EndEarlyTrigger> TimerEarlyEnders { get; set; }

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
