using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class OverlayTimerItem
    {
        private string _text;
        private string _fontcolor;
        private string _barcolor;
        public string Text { get; set; }
        public string FontColor { get; set; }
        public string BarColor { get; set; }

        public OverlayTimerItem()
        {
            _text = "";
            _fontcolor = "";
            _barcolor = "";
        }
    }
}
