namespace EQAudioTriggers.Models
{
    public class ActivatedTrigger
    {
        private string _character;
        private string _trigger;
        private string _matchedText;
        private string _logTime;
        private string _matchTime;

        public ActivatedTrigger()
        {
            _character = "";
            _trigger = "";
            _matchedText = "";
            _logTime = "";
            _matchTime = "";
        }

        public string Character { get; set; }
        public string Trigger { get; set; }
        public string MatchedText { get; set; }
        public string LogTime { get; set; }
        public string MatchTime { get; set; }

    }
}
