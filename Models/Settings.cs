using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class Settings : INotifyPropertyChanged
    {
        private string _mastervolume;
        private string _applicationupdate;
        private string _enablesound;
        private string _enabletext;
        private string _enabletimers;
        private string _minimize;
        private string _stoptriggersearch;
        private string _displaymatchlog;
        private string _maxlogentry;
        private string _logmatchestofile;
        private string _logmatchfilename;
        private string _clipboard;
        private string _eqfolder;
        private string _importedmediafolder;
        private string _datafolder;
        private string _sharingenabled;
        private string _enableincomingtriggers;
        private string _acceptinvitationsfrom;
        private string _mergefrom;
        private ObservableCollection<string> _trustedsenderlist;
        private string _logarchivefolder;
        private string _autoarchive;
        private string _compressarchive;
        private string _archivemethod;
        private string _logsize;
        private string _autodelete;
        private string _deletearchives;
        private string _shareserviceuri;
        private string _reference;
        private string _enabledebug;
        private string _archiveschedule;
        private string _darkmode;
        private string _corepercentage;
        private List<string> _archivemethodlist;
        private List<string> _archiveschedulelist;

        public event PropertyChangedEventHandler PropertyChanged;

        public Settings()
        {
            _mastervolume = "60";
            _applicationupdate = "true";
            _enablesound = "true";
            _enabletext = "true";
            _enabletimers = "true";
            _minimize = "false";
            _stoptriggersearch = "false";
            _displaymatchlog = "true";
            _maxlogentry = "100";
            _logmatchestofile = "false";
            _logmatchfilename = "";
            _clipboard = "{C}";
            _eqfolder = "C:\\EQ";
            _importedmediafolder = "C:\\EAT\\ImportedMedia";
            _datafolder = "C:\\EAT";
            _sharingenabled = "true";
            _enableincomingtriggers = "true";
            _acceptinvitationsfrom = "2";
            _mergefrom = "2";
            _logarchivefolder = "C:\\EAT\\Archive";
            _autoarchive = "true";
            _compressarchive = "true";
            _archivemethod = "Size Threshold";
            _logsize = "50";
            _autodelete = "true";
            _deletearchives = "90";
            _shareserviceuri = "http:\\shareservice.com";
            _reference = "You";
            _enabledebug = "false";
            _archiveschedule = "Weekly (Friday)";
            _darkmode = "false";
            _corepercentage = "80";
            _trustedsenderlist = new ObservableCollection<string>();
            _archivemethodlist = new List<string>();
            _archiveschedulelist = new List<string>();


        }
        public void DefaultSettings()
        {
            _archivemethodlist.Add("Scheduled");
            _archivemethodlist.Add("Size Threshold");
            _archiveschedulelist.Add("Weekly (Monday)");
            _archiveschedulelist.Add("Weekly (Tuesday)");
            _archiveschedulelist.Add("Weekly (Wednesday)");
            _archiveschedulelist.Add("Weekly (Thursday)");
            _archiveschedulelist.Add("Weekly (Friday)");
            _archiveschedulelist.Add("Weekly (Saturday)");
            _archiveschedulelist.Add("Weekly (Sunday)");
            _archiveschedulelist.Add("Daily");
            _archiveschedulelist.Add("Monthly");
            _archiveschedulelist.Add("Quarterly");
            _archiveschedulelist.Add("Annually");
        }
        public List<String> ArchiveMethodList { get { return _archivemethodlist; } set {_archivemethodlist = value; } }
        public List<String> ArchiveScheduleList { get { return _archiveschedulelist; } set {_archiveschedulelist = value; } }
        public string MasterVolume { get { return _mastervolume; } set { _mastervolume = value; NotifyPropertyChanged("MasterVolume"); } }
        public string ApplicationUpdate { get { return _applicationupdate; } set { _applicationupdate = value; NotifyPropertyChanged("ApplicationUpdate"); } }
        public string EnableSound { get { return _enablesound; } set { _enablesound = value; NotifyPropertyChanged("EnableSound"); } }
        public string EnableText { get { return _enabletext; } set { _enabletext = value; NotifyPropertyChanged("EnableText"); } }
        public string EnableTimers { get { return _enabletimers; } set { _enabletimers = value; NotifyPropertyChanged("EnableTimers"); } }
        public string Minimize { get { return _minimize; } set { _minimize = value; NotifyPropertyChanged("Minimize"); } }
        public string StopTriggerSearch { get { return _stoptriggersearch; } set { _stoptriggersearch = value; NotifyPropertyChanged("StopTriggerSearch"); } }
        public string DisplayMatchLog { get { return _displaymatchlog; } set { _displaymatchlog = value; NotifyPropertyChanged("DisplayMatchLog"); } }
        public string MaxLogEntry { get { return _maxlogentry; } set { _maxlogentry = value; NotifyPropertyChanged("MaxLogEntry"); } }
        public string LogMatchesToFile { get { return _logmatchestofile; } set { _logmatchestofile = value; NotifyPropertyChanged("LogMatchesToFile"); } }
        public string LogMatchFilename { get { return _logmatchfilename; } set { _logmatchfilename = value; NotifyPropertyChanged("LogMatchFilename"); } }
        public string Clipboard { get { return _clipboard; } set { _clipboard = value; NotifyPropertyChanged("Clipboard"); } }
        public string EQFolder { get { return _eqfolder; } set { _eqfolder = value; NotifyPropertyChanged("EQFolder"); } }
        public string ImportedMediaFolder { get { return _importedmediafolder; } set { _importedmediafolder = value; NotifyPropertyChanged("ImportedMediaFolder"); } }
        public string DataFolder { get { return _datafolder; } set { _datafolder = value; NotifyPropertyChanged("DataFolder"); } }
        public string SharingEnabled { get { return _sharingenabled; } set { _sharingenabled = value; NotifyPropertyChanged("SharingEnabled"); } }
        public string EnableIncomingTriggers { get { return _enableincomingtriggers; } set { _enableincomingtriggers = value; NotifyPropertyChanged("EnableIncomingTriggers"); } }
        public string AcceptInvitationsFrom { get { return _acceptinvitationsfrom; } set { _acceptinvitationsfrom = value; NotifyPropertyChanged("AcceptInvitationsFrom"); } }
        public string MergeFrom { get { return _mergefrom; } set { _mergefrom = value; NotifyPropertyChanged("MergeFrom"); } }
        public ObservableCollection<string> TrustedSenderList { get { return _trustedsenderlist; } set { _trustedsenderlist = value; NotifyPropertyChanged("TrustedSenderList"); } }
        public string LogArchiveFolder { get { return _logarchivefolder; } set { _logarchivefolder = value; NotifyPropertyChanged("LogArchiveFolder"); } }
        public string AutoArchive { get { return _autoarchive; } set { _autoarchive = value; NotifyPropertyChanged("AutoArchive"); } }
        public string CompressArchive { get { return _compressarchive; } set { _compressarchive = value; NotifyPropertyChanged("CompressArchive"); } }
        public string ArchiveMethod { get { return _archivemethod; } set { _archivemethod = value; NotifyPropertyChanged("ArchiveMethod"); } }
        public string LogSize { get { return _logsize; } set { _logsize = value; NotifyPropertyChanged("LogSize"); } }
        public string AutoDelete { get { return _autodelete; } set { _autodelete = value; NotifyPropertyChanged("AutoDelete"); } }
        public string DeleteArchives { get { return _deletearchives; } set { _deletearchives = value; NotifyPropertyChanged("DeleteArchives"); } }
        public string ShareServiceURI { get { return _shareserviceuri; } set { _shareserviceuri = value; NotifyPropertyChanged("ShareServiceURI"); } }
        public string Reference { get { return _reference; } set { _reference = value; NotifyPropertyChanged("Reference"); } }
        public string EnableDebug { get { return _enabledebug; } set { _enabledebug = value; NotifyPropertyChanged("EnableDebug"); } }
        public string ArchiveSchedule { get { return _archiveschedule; } set { _archiveschedule = value; NotifyPropertyChanged("ArchiveSchedule"); } }
        public string DarkMode { get { return _darkmode; } set { _darkmode = value; NotifyPropertyChanged("DarkMode"); } }
        public string CorePercentage { get { return _corepercentage; } set { _corepercentage = value; NotifyPropertyChanged("CorePercentage"); } }
        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
