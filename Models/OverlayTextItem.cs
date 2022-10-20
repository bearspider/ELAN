using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class OverlayTextItem
    {
        private string _text;
        private string _fontcolor;
        public string Text { get; set; }
        public string FontColor { get; set; }

        public OverlayTextItem()
        {
            _text = "";
            _fontcolor = "";
        }


    }
}
