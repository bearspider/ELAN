using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit.Core.Converters;

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

        public OverlayText()
        {
            _name = "default";
            _font = "Segoe UI";
            _fontcolor = "Black";
            _size = 20;
            _delay = 10;
            _bg = "Transparent";
            _faded = "Transparent";
            _windowHeight = 450;
            _windowWidth = 800;
            _windowX = 0;
            _windowY = 0;
        }
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
