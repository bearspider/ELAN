using System;
using System.ComponentModel;

namespace EQAudioTriggers.Models
{
    public class OverlayText : INotifyPropertyChanged
    {
        private string _name;
        private string _font;
        private string _fontcolor;
        private int _size;
        private int _delay;
        private string _bg;
        private string _faded;
        private double _windowHeight;
        private double _windowWidth;
        private double _windowX;
        private double _windowY;
        private string _id;
        private Boolean _isoverride;
        private string _overrideid;

        public OverlayText()
        {
            Name = "default";
            Font = "Segoe UI";
            FontColor = "Black";
            Size = 20;
            Delay = 10;
            BG = "Transparent";
            Faded = "Transparent";
            WindowHeight = 450;
            WindowWidth = 800;
            WindowX = 0;
            WindowY = 0;
            Id = Guid.NewGuid().ToString();
            IsOverride = false;
            OverrideId = "";
        }
        public string Id { get { return _id; } set { _id = value; NotifyPropertyChanged("Id"); } }
        public Boolean IsOverride { get { return _isoverride; } set { _isoverride = value; NotifyPropertyChanged("IsOverride"); } }
        public string OverrideId { get { return _overrideid; } set { _overrideid = value; NotifyPropertyChanged("OverrideId"); } }
        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public string Font { get { return _font; } set { _font = value; NotifyPropertyChanged("Name"); } }
        public string FontColor { get { return _fontcolor; } set { _fontcolor = value; NotifyPropertyChanged("Name"); } }
        public int Size { get { return _size; } set { _size = value; NotifyPropertyChanged("Name"); } }
        public int Delay { get { return _delay; } set { _delay = value; NotifyPropertyChanged("Name"); } }
        public string BG { get { return _bg; } set { _bg = value; NotifyPropertyChanged("Name"); } }
        public string Faded { get { return _faded; } set { _faded = value; NotifyPropertyChanged("Name"); } }
        public double WindowHeight { get { return _windowHeight; } set { _windowHeight = value; NotifyPropertyChanged("Name"); } }
        public double WindowWidth { get { return _windowWidth; } set { _windowWidth = value; NotifyPropertyChanged("Name"); } }
        public double WindowX { get { return _windowX; } set { _windowX = value; NotifyPropertyChanged("Name"); } }
        public double WindowY { get { return _windowY; } set { _windowY = value; NotifyPropertyChanged("Name"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
