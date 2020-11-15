using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class GINAGroup
    {
        private string _name;
        private string _comments;
        private Boolean _selfcommented;
        private string _groupid;
        private Boolean _enablebydefault;
        private ObservableCollection<GINATrigger> _triggers;

        public GINAGroup()
        {
            Name = "";
            Comments = "";
            SelfCommented = false;
            GroupId = "";
            EnableByDefault = false;
            Triggers = new ObservableCollection<GINATrigger>();
        }
        public string Name { get; set; }
        public string Comments { get; set; }
        public Boolean SelfCommented { get; set; }
        public string GroupId { get; set; }
        public Boolean EnableByDefault { get; set; }
        public ObservableCollection<GINATrigger> Triggers { get; set; }
    }
}
