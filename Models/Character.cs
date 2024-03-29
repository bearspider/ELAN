﻿using EQAudioTriggers.Views;
using System;
using System.ComponentModel;
using System.Speech.Synthesis;
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
        private Boolean _monitoring;
        private string _id;
        private Boolean _default;

        public event PropertyChangedEventHandler PropertyChanged;

        public Character()
        {
            _name = "ChangeMe";
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
            _monitoring = _monitor;
            _default = false;
            _id = Guid.NewGuid().ToString();
        }

        public Character(Character newchar)
        {
            _name = newchar.Name;
            _logfile = newchar.LogFile;
            _profile = newchar.Profile;
            _monitor = newchar.Monitor;
            _textfontcolor = newchar.TextFontColor;
            _timerfontcolor = newchar.TimerFontColor;
            _timerbarcolor = newchar.TimerBarColor;
            _audiovolume = newchar.AudioVolume;
            _audiovoice = newchar.AudioVoice;
            _voicespeed = newchar.VoiceSpeed;
            _phoneticname = newchar.PhoenticName;
            _synth = new SpeechSynthesizer();
            _synth.Rate = newchar.VoiceSpeed;
            _synth.Volume = newchar.AudioVolume;
            _synth.SelectVoice(newchar.AudioVoice);
            _lastlogposition = newchar.LastLogPosition;
            _monitoring = newchar.Monitoring;
            _default = newchar.Default;
            _id = newchar.Id;
        }

        public string Id { get { return _id; } set { _id = value; NotifyPropertyChanged("Id"); } }
        public Boolean Default { get { return _default; } set { _default = value; NotifyPropertyChanged("Default"); } }
        public Boolean Monitoring { get { return _monitoring; } set { _monitoring = value; NotifyPropertyChanged("Monitoring"); } }
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

        public void Delete()
        {

        }

        public void EditCharacter()
        {
            Character newchar = new Character(this);
            CharacterEdit chareditor = new CharacterEdit(this);
            Boolean rval = (Boolean)chareditor.ShowDialog();
            //Poor man's cancel edit revert
            if (!rval)
            {
                Id = newchar.Id;
                Name = newchar.Name;
                LogFile = newchar.LogFile;
                Profile = newchar.Profile;
                Monitor = newchar.Monitor;
                TextFontColor = newchar.TextFontColor;
                TimerFontColor = newchar.TimerFontColor;
                TimerBarColor = newchar.TimerBarColor;
                AudioVolume = newchar.AudioVolume;
                AudioVoice = newchar.AudioVoice;
                VoiceSpeed = newchar.VoiceSpeed;
                PhoenticName = newchar.PhoenticName;
                _synth = new SpeechSynthesizer();
                _synth.Rate = VoiceSpeed;
                _synth.Volume = AudioVolume;
                _synth.SelectVoice(AudioVoice);
                LastLogPosition = newchar.LastLogPosition;
                Default = newchar.Default;
                Monitoring = newchar.Monitoring;
            }
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
