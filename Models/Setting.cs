using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Setting()
        {
            Id = 0;
            Name = "";
            Value = "";
        }
    }
}
