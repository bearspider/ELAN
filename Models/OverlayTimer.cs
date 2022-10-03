using EQAudioTriggers.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace EQAudioTriggers.Models
{
    public class OverlayTimer : INotifyPropertyChanged
    {
        public OverlayTimer()
        {
            Name = "default";
            Font = "Segoe UI";
            Size = 20;
            BG = "GhostWhite";
            Faded = "Transparent";
            Showtimer = true;
            Emptycolor = "Gray";
            Standardize = false;
            Group = false;
            Sortby = "Time Remaining";
            WindowHeight = 450;
            WindowWidth = 800;
            WindowX = 0;
            WindowY = 0;
            FontColor = "Black";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private int _id;
        private string _name;
        private string _font;
        private int _size;
        private string _bg;
        private string _faded;
        private Boolean _showtimer;
        private string _emptycolor;
        private Boolean _standardize;
        private Boolean _group;
        private string _sortby;
        private double _windowheight;
        private double _windowwidth;
        private double _windowX;
        private double _windowY;
        private string _fontcolor;

        public int Id { get { return _id; } set { _id = value; NotifyPropertyChanged("Id"); } }
        public String Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public String Font { get { return _font; } set { _font = value; NotifyPropertyChanged("Font"); } }
        public int Size { get { return _size; } set { _size = value; NotifyPropertyChanged("Size"); } }
        public String BG { get { return _bg; } set { _bg = value; NotifyPropertyChanged("BG"); } }
        public String Faded { get { return _faded; } set { _faded = value; NotifyPropertyChanged("Faded"); } }
        public Boolean Showtimer { get { return _showtimer; } set { _showtimer = value; NotifyPropertyChanged("ShowTimer"); } }
        public String Emptycolor { get { return _emptycolor; } set { _emptycolor = value; NotifyPropertyChanged("EmptyColor"); } }
        public Boolean Standardize { get { return _standardize; } set { _standardize = value; NotifyPropertyChanged("Standardize"); } }
        public Boolean Group { get { return _group; } set { _group = value; NotifyPropertyChanged("Group"); } }
        public String Sortby { get { return _sortby; } set { _sortby = value; NotifyPropertyChanged("Sortby"); } }
        public double WindowHeight { get { return _windowheight; } set { _windowheight = value; NotifyPropertyChanged("WindowHeight"); } }
        public double WindowWidth { get { return _windowwidth; } set { _windowwidth = value; NotifyPropertyChanged("WindowWidth"); } }
        public double WindowX { get { return _windowX; } set { _windowX = value; NotifyPropertyChanged("WindowX"); } }
        public double WindowY { get { return _windowY; } set { _windowY = value; NotifyPropertyChanged("WindowY"); } }
        public string FontColor { get { return _fontcolor; } set { _fontcolor = value; NotifyPropertyChanged("FontColor"); } }
        public bool Equals(OverlayTimer compareto)
        {
            bool rval = false;
            if (
                Id == compareto.Id
                && Name == compareto.Name
                && Font == compareto.Font
                && Size == compareto.Size
                && BG == compareto.BG
                && Faded == compareto.Faded
                && Showtimer == compareto.Showtimer
                && Emptycolor == compareto.Emptycolor
                && Standardize == compareto.Standardize
                && Group == compareto.Group
                && Sortby == compareto.Sortby
                && WindowHeight == compareto.WindowHeight
                && WindowWidth == compareto.WindowWidth
                && WindowX == compareto.WindowX
                && WindowY == compareto.WindowY
                && FontColor == compareto.FontColor
                )
            {
                rval = true;
            }
            return rval;
        }
    }
}
