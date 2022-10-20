using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EQAudioTriggers.Models
{
    public class Category : INotifyPropertyChanged
    {
        private string _id;
        private String _name;
        private String _textOverlay;
        private String _timerOverlay;
        private String _textFontColor;
        private String _timerFontColor;
        private String _timerBarColor;
        private Boolean _defaultCategory;
        private Boolean _textUseCharacter;
        private Boolean _textUseColor;
        private Boolean _timerUseCharacter;
        private Boolean _timerUseColor;
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public Category()
        {
            Id = Guid.NewGuid().ToString();
            CategoryName = "Default";
            TextOverlay = "default";
            TimerOverlay = "default";
            TextFontColor = "Yellow";
            TimerFontColor = "Gray";
            TimerBarColor = "Blue";
            DefaultCategory = false;
            TextUseCharacter = false;
            TextUseColor = true;
            TimerUseCharacter = false;
            TimerUseColor = true;
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged("Id"); }
        }
        public String CategoryName
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        public String TextOverlay
        {
            get { return _textOverlay; }
            set { _textOverlay = value; NotifyPropertyChanged("TextOverlay"); }
        }
        public String TimerOverlay
        {
            get { return _timerOverlay; }
            set { _timerOverlay = value; NotifyPropertyChanged("TimerOverlay"); }
        }
        public String TextFontColor
        {
            get { return _textFontColor; }
            set { _textFontColor = value; NotifyPropertyChanged("TextFontColor"); }
        }
        public String TimerFontColor
        {
            get { return _timerFontColor; }
            set { _timerFontColor = value; NotifyPropertyChanged("TimerFontColor"); }
        }
        public String TimerBarColor
        {
            get { return _timerBarColor; }
            set { _timerBarColor = value; NotifyPropertyChanged("TimerBarColor"); }
        }
        public Boolean DefaultCategory
        {
            get { return _defaultCategory; }
            set { _defaultCategory = value; NotifyPropertyChanged("DefaultCategory"); }
        }
        public Boolean TextUseCharacter
        {
            get { return _textUseCharacter; }
            set { _textUseCharacter = value; NotifyPropertyChanged("TextColors"); }
        }
        public Boolean TextUseColor
        {
            get { return _textUseColor; }
            set { _textUseColor = value; NotifyPropertyChanged("TextThis"); }
        }
        public Boolean TimerUseCharacter
        {
            get { return _timerUseCharacter; }
            set { _timerUseCharacter = value; NotifyPropertyChanged("TimerColors"); }
        }
        public Boolean TimerUseColor
        {
            get { return _timerUseColor; }
            set { _timerUseColor = value; NotifyPropertyChanged("TimerThis"); }
        }
        public bool Equals(Category compareto)
        {
            bool rval = false;
            if (
                _id == compareto.Id
                && _name == compareto.CategoryName
                && _textOverlay == compareto.TextOverlay
                && _timerOverlay == compareto.TimerOverlay
                && _textFontColor == compareto.TextFontColor
                && _timerFontColor == compareto.TimerFontColor
                && _timerBarColor == compareto.TimerBarColor
                && _defaultCategory == compareto.DefaultCategory
                && _textUseCharacter == compareto.TextUseCharacter
                && _textUseColor == compareto.TextUseColor
                && _timerUseCharacter == compareto.TimerUseCharacter
                && _timerUseColor == compareto.TimerUseColor)
            {
                rval = true;
            }
            else
            {
                return false;
            }
            return rval;
        }
    }
}
