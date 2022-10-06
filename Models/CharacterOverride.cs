using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class CharacterOverride : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public CharacterOverride()
        {
            _profilename = "";
            _profileid = "";
            _categoryid = "";
            _textoverlayselection = "Use category overlay";
            _textcolorselection = "Use category colors";
            _textcolorfont = "Aquamarine";
            _textoverlay = "Default";
            _textcolorselection = "Aquamarine";
            _timeroverlayselection = "Use category overlay";
            _timercolorselection = "Use category colors";
            _timeroverlay = "Default";
            _timercolorfont = "Aquamarine";
            _timercolorbar = "Blue";
            _textoverlaycategory = true;
            _textoverlaythis = false;
            _textcolorcategory = true;
            _textcolorcategory = false;
            _textcolorthis = false;
            _timeroverlaycategory = true;
            _timeroverlaythis = false;
            _timercolorcategory = true;
            _timercolorcharacter = false;
            _timercolorthis = false;
        }
        private string _profilename;
        private string _profileid;
        private string _categoryid;
        private string _textoverlayselection;
        private string _textcolorselection;
        private string _textcolorfont;
        private string _textoverlay;
        private string _timercolorselection;
        private string _timeroverlayselection;
        private string _timeroverlay;
        private string _timercolorfont;
        private string _timercolorbar;
        private Boolean _textoverlaycategory;
        private Boolean _textoverlaythis;
        private Boolean _textcolorcategory;
        private Boolean _textcolorcharacter;
        private Boolean _textcolorthis;
        private Boolean _timeroverlaycategory;
        private Boolean _timeroverlaythis;
        private Boolean _timercolorcategory;
        private Boolean _timercolorcharacter;
        private Boolean _timercolorthis;

        public string CategoryId { get { return _categoryid; } set { _categoryid = value; NotifyPropertyChanged("CategoryId"); } }
        public string ProfileId { get { return _profileid; } set { _profileid = value; NotifyPropertyChanged("ProfileId"); } }
        public string ProfileName { get { return _profilename; } set { _profilename = value; NotifyPropertyChanged("ProfileName"); } }
        public string TextOverlaySelection { get { return _textoverlayselection; } set { _textoverlayselection = value; NotifyPropertyChanged("TextOverlaySelection"); } }
        public string TextColorSelection { get { return _textcolorselection; } set { _textcolorselection = value; NotifyPropertyChanged("TextColorSelection"); } }
        public string TimerOverlaySelection { get { return _timeroverlayselection; } set { _timeroverlayselection = value; NotifyPropertyChanged("TimerOverlaySelection"); } }
        public string TimerColorSelection { get { return _timercolorselection; } set { _timercolorselection = value; NotifyPropertyChanged("TimerColorSelection"); } }
        public string TextOverlay { get { return _textoverlay; } set { _textoverlay = value; NotifyPropertyChanged("TextOverlay"); } }
        public string TextColorFont { get { return _textcolorfont; } set { _textcolorfont = value; NotifyPropertyChanged("TextColorFont"); } }
        public string TimerOverlay { get { return _timeroverlay; } set { _timeroverlay = value; NotifyPropertyChanged("TimerOverlay"); } }
        public string TimerColorFont { get { return _timercolorfont; } set { _timercolorfont = value; NotifyPropertyChanged("TimerColorFont"); } }
        public string TimerColorBar { get { return _timercolorbar; } set { _timercolorbar = value; NotifyPropertyChanged("TimerColorBar"); } }
        public Boolean TextOverlayCategory { get { return _textoverlaycategory; } set { _textoverlaycategory = value; NotifyPropertyChanged("TextOverlayCategory"); } }
        public Boolean TextOverlayThis { get { return _textoverlaythis; } set { _textoverlaythis = value; NotifyPropertyChanged("TextOverlayThis"); } }
        public Boolean TextColorCategory { get { return _textcolorcategory; } set { _textcolorcategory = value; NotifyPropertyChanged("TextColorCategory"); } }
        public Boolean TextColorCharacter { get { return _textcolorcharacter; } set { _textcolorcharacter = value; NotifyPropertyChanged("TextColorCharacter"); } }
        public Boolean TextColorThis { get { return _textcolorthis; } set { _textcolorthis = value; NotifyPropertyChanged("TextColorThis"); } }
        public Boolean TimerOverlayCategory { get { return _timeroverlaycategory; } set { _timeroverlaycategory = value; NotifyPropertyChanged("TimerOverlayCategory"); } }
        public Boolean TimerOverlayThis { get { return _timeroverlaythis; } set { _timeroverlaythis = value; NotifyPropertyChanged("TimerOverlayThis"); } }
        public Boolean TimerColorCategory { get { return _timercolorcategory; } set { _timercolorcategory = value; NotifyPropertyChanged("TimerColorCategory"); } }
        public Boolean TimerColorCharacter { get { return _timercolorcharacter; } set { _timercolorcharacter = value; NotifyPropertyChanged("TimerColorCharacter"); } }
        public Boolean TimerColorThis { get { return _timercolorthis; } set { _timercolorthis = value; NotifyPropertyChanged("TimerColorThis"); } }
        public bool Equals(CharacterOverride compareto)
        {
            bool rval = false;
            if (
                ProfileName == compareto.ProfileName
                && TextOverlaySelection == compareto.TextOverlaySelection
                && TextColorSelection == compareto.TextColorSelection
                && TimerOverlaySelection == compareto.TimerOverlaySelection
                && TimerColorSelection == compareto.TimerColorSelection
                && TextOverlay == compareto.TextOverlay
                && TextColorFont == compareto.TextColorFont
                && TimerOverlay == compareto.TimerOverlay
                && TimerColorFont == compareto.TimerColorFont
                && TimerColorBar == compareto.TimerColorBar
                && TextOverlayCategory == compareto.TextOverlayCategory
                && TextOverlayThis == compareto.TextOverlayThis
                && TextColorCategory == compareto.TextColorCategory
                && TextColorCharacter == compareto.TextColorCharacter
                && TextColorThis == compareto.TextColorThis
                && TimerColorCategory == compareto.TimerColorCategory
                && TimerOverlayThis == compareto.TimerOverlayThis
                && TimerColorCategory == compareto.TimerColorCategory
                && TimerColorCharacter == compareto.TimerColorCharacter
                && TimerColorThis == compareto.TimerColorThis
                && ProfileId == compareto.ProfileId
                && CategoryId == compareto.CategoryId
                )
            {
                rval = true;
            }
            return rval;
        }
    }
}
