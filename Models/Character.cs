using EQAudioTriggers.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class Character : INotifyPropertyChanged
    {
        private string _name;
        private string _logfile;
        private string _profile;
        private Boolean _monitor;
        private string _textfontcolor;
        private string _timerfontcolor;
        private string _timerbarcolor;
        private int _audiovolume;
        private string _audiovoice;
        private int _voicespeed;
        private string _phoneticname;
        private SpeechSynthesizer _synth;
        private long _lastlogposition;

        public event PropertyChangedEventHandler PropertyChanged;

        public Character()
        {
            _name = "";
            _logfile = "";
            _profile = "";
            _monitor = false;
            _textfontcolor = "Black";
            _timerfontcolor = "Blue";
            _timerbarcolor = "Lime";
            _audiovolume = 90;
            _audiovoice = "Microsoft David Desktop";
            _voicespeed = 0;
            _phoneticname = "";
            _synth = new SpeechSynthesizer();
            _synth.Rate = _voicespeed;
            _synth.Volume = _audiovolume;
            _synth.SelectVoice(_audiovoice);
            _lastlogposition = 0;
        }

        public long LastLogPosition { get { return _lastlogposition; } set { _lastlogposition = value; NotifyPropertyChanged("LastLogPostion"); } }
        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public string LogFile { get { return _logfile; } set { _logfile = value; NotifyPropertyChanged("LogFile"); } }
        public string Profile { get { return _profile; } set { _profile = value; NotifyPropertyChanged("Profile"); } }
        public Boolean Monitor { get { return _monitor; } set { _monitor = value; NotifyPropertyChanged("Monitor"); } }
        public string TextFontColor { get { return _textfontcolor; } set { _textfontcolor = value; NotifyPropertyChanged("TextFontColor"); } }
        public string TimerFontColor { get { return _timerfontcolor; } set { _timerfontcolor = value; NotifyPropertyChanged("TimerFontColor"); } }
        public string TimerBarColor { get { return _timerbarcolor; } set { _timerbarcolor = value; NotifyPropertyChanged("TimerBarColor"); } }
        public int AudioVolume { get { return _audiovolume; } set { _audiovolume = value; NotifyPropertyChanged("AudioVolume"); } }
        public string AudioVoice { get { return _audiovoice; } set { _audiovoice = value; NotifyPropertyChanged("AudioVoice"); } }
        public int VoiceSpeed { get { return _voicespeed; } set { _voicespeed = value; NotifyPropertyChanged("VoiceSpeed"); } }
        public string PhoenticName { get { return _phoneticname; } set { _phoneticname = value; NotifyPropertyChanged("PhoenticName"); } }

        public void EditCharacter()
        {
            CharacterEdit chareditor = new CharacterEdit(this);
            chareditor.ShowDialog();
        }

        public async void Speak(string output)
        {
            await Task.Run(() =>
            {
                _synth.Speak(output);
            });
        }

        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
