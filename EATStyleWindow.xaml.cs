using Syncfusion.Windows.Tools.Controls;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Syncfusion.SfSkinManager;
using System.Runtime.Remoting;
using EQAudioTriggers.Models;
using System.Collections.ObjectModel;
using Syncfusion.UI.Xaml.TreeView;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Windows.Forms.PropertyGridInternal;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System.Windows.Threading;
using System.Globalization;
using EQAudioTriggers.Views;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO.Compression;
using System.Xml;
using LiteDB;

namespace EQAudioTriggers
{
    public class GlobalVariables
    {
        public static string workingdirectory = @"F:\\EAT";
        public static string foldericon = "\\Images\\Oxygen-Icons.org-Oxygen-Actions-document-open-folder.ico";
        public static string triggericon = "\\Images\\Oxygen-Icons.org-Oxygen-Actions-irc-voice.ico";
        public static Regex eqRegex = new Regex(@"\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\](?<stringToMatch>.*)", RegexOptions.Compiled);
        public static Regex shareRegex = new Regex(@".*?\{HEAP:(?<GUID>.*?)\}", RegexOptions.Compiled);
        public static Regex eqspellRegex = new Regex(@"(\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\])\s((?<character>\w+)\sbegin\s(casting|singing)\s(?<spellname>.*)\.)|(\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\])\s(?<character>\w+)\s(begins\sto\s(cast|sing)\s.*\<(?<spellname>.*)\>)", RegexOptions.Compiled);
        public static ConnectionString defaultDB = new ConnectionString($@"filename=""{workingdirectory}\\eqtriggers.db""; Connection = shared; Upgrade = true");
        public static string dbpath = $"{workingdirectory}\\eqtriggers.db";
        public static string backupDB = $"{workingdirectory}\\BackupDB";
    }

    public static class Utilities
    {
        public static string IdGenerator()
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(9)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();
            return id;
        }
    }

    #region Converters
    public class TreeHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (string)value;
            return type == "group" ? 50 : 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitoringColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var monitored = (Boolean)value;
            if (monitored)
            {
                return Brushes.LimeGreen;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CheckboxColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var monitored = (bool?)value;
            if (monitored == true)
            {
                return Brushes.LimeGreen;
            }
            else if(monitored == null)
            {
                return Brushes.Fuchsia;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitoringContextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(Boolean)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MonitoringImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var monitored = (Boolean)value;
            if (monitored)
            {
                return "Images/Google-Noto-Emoji-Objects-62963-crossed-swords.ico";
            }
            else
            {
                return "Images/Icojam-Blue-Bits-Shield.ico";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FontAttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (string)value;
            return type == "group" ? FontWeights.Bold : FontWeights.Regular;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (string)value;
            return type == "group" ? 14 : 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CheckBoxActiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                TreeViewNode tvn = (TreeViewNode)value;
                if (((TriggerManager)tvn.Content).NodeType == "trigger")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Random junk passing in converter");
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    /// <summary>
    /// Interaction logic for EATStyleWindow.xaml
    /// </summary>
    public partial class EATStyleWindow : RibbonWindow
    {
        #region Properties

        //Look into zone specific triggers
        //parse out the "You have entered {The Plane of Knowledge}"

        private int _totallinecount = 0;
        private ObservableCollection<CharacterCollection> _characters = new ObservableCollection<CharacterCollection>();
        private ActivatedTriggerCollection _activatedtriggers = new ActivatedTriggerCollection();
        private ActiveTriggerCollection _activetriggers = new ActiveTriggerCollection();
        private ObservableCollection<TriggerManager> _triggermanager = new ObservableCollection<TriggerManager>();
        private ObservableCollection<TriggerManager> _mergemanager = new ObservableCollection<TriggerManager>();
        private static StringCollection _log = new StringCollection();
        private readonly SynchronizationContext syncontext;
        private ObservableCollection<EQTrigger> _triggermasterlist = new ObservableCollection<EQTrigger>();
        private string _selectedcharacter = "";
        private ObservableCollection<Setting> programsettings = new ObservableCollection<Setting>();

        //System Tray Icons
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;

        #endregion
        public EATStyleWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadBase();
            DataContext = this;
            syncontext = SynchronizationContext.Current;
            //po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            LoadSettings();
            LoadCharacters();
            LoadTriggers();
            ActivateLog();
            if (_characters.Count > 0)
            {
                foreach (CharacterCollection profile in _characters)
                {
                    if (profile.CharacterProfile.Monitor)
                    {
                        StartMonitor(profile.CharacterProfile);
                    }
                }
            }

            //set datacontext for status bar
            txtblockStatus.DataContext = _totallinecount;

            //adding this lame code so when the first item auto selects the checkboxes render
            _listviewCharacters.SelectedItem = null;
        }
        private void InitializeDatabase()
        {
            //Ensure the collections exist in the database
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                db.GetCollection<Setting>("settings");
                db.GetCollection<EQTrigger>("triggers");
                db.GetCollection<Character>("characters");
                db.GetCollection<TriggerGroupProperty>("groups");
                //db.GetCollection<Category>("categories");
            }
        }
        private void LoadBase()
        {
            //Setup the system tray
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/EQAudioTriggers;component/Images/Tonev-Windows-7-Windows-7-headphone.ico")).Stream;
            MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
            MyNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);

            //Check if database exists, if it does, make a backup.
            //Else, this is a new instance.  Set the log maintenance start as today.
            if (File.Exists(GlobalVariables.dbpath))
            {
                if (!Directory.Exists(GlobalVariables.backupDB))
                {
                    Directory.CreateDirectory(GlobalVariables.backupDB);
                }
                File.Copy(GlobalVariables.dbpath, (GlobalVariables.backupDB + @"\eqtriggers.db"), true);
            }
            else
            {
                Properties.Settings.Default.LastLogMaintenance = DateTime.Now;
                Properties.Settings.Default.Save();
            }
        }
        private void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            //if ((bool)checkboxMinimize.IsChecked)
            {
                if (this.WindowState == WindowState.Minimized)
                {
                    this.ShowInTaskbar = false;
                    MyNotifyIcon.BalloonTipTitle = "Minimize Sucessful";
                    MyNotifyIcon.BalloonTipText = "Minimized the app ";
                    MyNotifyIcon.ShowBalloonTip(400);
                    MyNotifyIcon.Visible = true;
                }
                else if (this.WindowState == WindowState.Normal)
                {
                    MyNotifyIcon.Visible = false;
                    this.ShowInTaskbar = true;
                }
            }
        }
        private void Filewatcher_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException().GetType().ToString());
        }
        private async void StartMonitor(Character character)
        {
            #region threading
            await Task.Run(() =>
            {
                Console.WriteLine($"Monitoring {character.Name}");
                using (FileStream filestream = File.Open(character.LogFile, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    character.LastLogPosition = filestream.Seek(0, SeekOrigin.End);
                    FileSystemWatcher filewatcher = new FileSystemWatcher();
                    filewatcher.Path = System.IO.Path.GetDirectoryName(character.LogFile);
                    filewatcher.Filter = System.IO.Path.GetFileName(character.LogFile);
                    filewatcher.EnableRaisingEvents = true;
                    filewatcher.Error += Filewatcher_Error;
                    filewatcher.InternalBufferSize = 65536;

                    using (StreamReader streamReader = new StreamReader(filestream))
                    {

                        while (character.Monitoring)
                        {
                            WaitForChangedResult cr = filewatcher.WaitForChanged(WatcherChangeTypes.Changed, -1);
                            Console.WriteLine($"{cr.TimedOut}");
                            String capturedLine = streamReader.ReadToEnd();
                            if (capturedLine != null)
                            {
                                string[] capturedlines = capturedLine.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                                Console.WriteLine($"Total Lines: {capturedlines.Count<string>()}");
                                UpdateLineCount(capturedlines.Count<String>());
                                Boolean triggered = false;
                                if (capturedLine.Contains("{ELAN:"))
                                { }
                                else
                                {
                                    Parallel.ForEach(_activetriggers.Collection, (EQTrigger doc, ParallelLoopState state) =>
                                    {
                                        //Do regex match if enabled otherwise string.contains
                                        //capturedline is a whole block of text.  If we match something inside that block, then compare each single line to find it.
                                        Boolean foundmatch = false;
                                        if (doc.UseRegex)
                                        {
                                            if (doc.FastCheck)
                                            {
                                                //if (capturedLine.ToUpper().Contains(doc.Digest.ToUpper()))
                                                //{
                                                //    foundmatch = true;
                                                //}
                                            }
                                            else
                                            {
                                                foundmatch = (Regex.Match(capturedLine, doc.SearchText, RegexOptions.IgnoreCase)).Success;
                                            }
                                        }
                                        else
                                        {
                                            String ucaselog = capturedLine.ToUpper();
                                            String ucasetrigger = doc.SearchText.ToUpper();
                                            foundmatch = ucaselog.Contains(ucasetrigger);
                                        }
                                        if (doc.EndEarlyTriggers.Count > 0)
                                        {
                                            Boolean endearly = false;
                                            foreach (EndEarlyTrigger earlyend in doc.EndEarlyTriggers)
                                            {
                                                if (earlyend.UseRegex)
                                                {
                                                    endearly = (Regex.Match(capturedLine, Regex.Escape(earlyend.SearchText), RegexOptions.IgnoreCase)).Success;
                                                }
                                                else
                                                {
                                                    String ucaselog = capturedLine.ToUpper();
                                                    String ucasetrigger = earlyend.SearchText.ToUpper();
                                                    endearly = ucaselog.Contains(ucasetrigger);
                                                }
                                                //TO DO: Probably implement extra stuff on a early end trigger
                                                if (endearly)
                                                {
                                                    //ClearTimer(doc);
                                                }
                                            }
                                        }
                                        if (foundmatch)
                                        {
                                            triggered = true;
                                            Match logmatch = GlobalVariables.eqRegex.Match(capturedLine);
                                            ActivatedTrigger activatedTrigger = new ActivatedTrigger
                                            {
                                                Character = character.Profile,
                                                MatchedText = capturedLine.TrimEnd('\r', '\n'),
                                                MatchTime = DateTime.Now.ToString(),
                                                Trigger = doc.Name,
                                                LogTime = logmatch.Groups["eqtime"].Value.ToString()
                                            };
                                            _activatedtriggers.Collection.Add(activatedTrigger);

                                            Stopwatch firetrigger = new Stopwatch();
                                            firetrigger.Start();
                                            //FireTrigger(doc, character, newstring);
                                            firetrigger.Stop();

                                            //Global Option for stopping on a first match
                                            //if (stopfirstmatch)
                                            //{
                                            //    state.Break();
                                            //}
                                        }
                                    });
                                }
                            }
                        }
                        Console.WriteLine("Stopped Monitoring");
                    }
                }
            });
            #endregion
        }
        private void LoadSettings()
        {
            //Load settings.json
            //Check if EQAudioTriggers folder exists, if not create.
            bool mainPath = Directory.Exists(GlobalVariables.workingdirectory);
            if (!mainPath)
            {
                Directory.CreateDirectory(GlobalVariables.workingdirectory);
            }

            //Load settings from DB
            //Load settings
            Boolean newdb = false;
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<Setting> settings = db.GetCollection<Setting>("settings");
                if (settings.Count() == 0)
                {
                    newdb = true;
                }
                else
                {
                    foreach (Setting programsetting in settings.FindAll())
                    {
                        programsettings.Add(programsetting);
                    }
                    //LoadSettingsTab();
                }
            }
            if (newdb)
            {
                //populate default settings
                DefaultSettings();
            }


        }
        private void LoadSettingsTab()
        {
            //textboxArchiveFolder.Text = logmaintenance.ArchiveFolder = programsettings.Single<Setting>(i => i.Name == "LogArchiveFolder").Value;
            //logmaintenance.ArchiveFolder = programsettings.Single<Setting>(i => i.Name == "LogArchiveFolder").Value;
            //comboboxArchiveSchedule.Text = logmaintenance.ArchiveSchedule = programsettings.Single<Setting>(i => i.Name == "ArchiveSchedule").Value;
            //logmaintenance.AutoDelete = programsettings.Single<Setting>(i => i.Name == "AutoDelete").Value;
            //logmaintenance.CompressArchive = programsettings.Single<Setting>(i => i.Name == "CompressArchive").Value;
            //logmaintenance.ArchiveDays = Convert.ToInt32(programsettings.Single<Setting>(i => i.Name == "DeleteArchives").Value);
            //logmaintenance.LastArchive = Properties.Settings.Default.LastLogMaintenance;
            //checkboxDarkmode.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "DarkMode").Value);
            //checkboxDeleteArchive.IsChecked = Convert.ToBoolean(logmaintenance.AutoDelete);
            //logmaintenance.AutoArchive = programsettings.Single<Setting>(i => i.Name == "AutoArchive").Value;
            //checkboxAutoArchive.IsChecked = Convert.ToBoolean(logmaintenance.AutoArchive);
            //checkboxCompress.IsChecked = Convert.ToBoolean(logmaintenance.CompressArchive);
            //textboxArchiveDays.Text = logmaintenance.ArchiveDays.ToString();
            //textboxShareURI.Text = programsettings.Single<Setting>(i => i.Name == "ShareServiceURI").Value;
            //textboxSelfReference.Text = programsettings.Single<Setting>(i => i.Name == "Reference").Value;
            //checkboxShareDebug.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "EnableDebug").Value);
            //checkboxEnableSharing.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "SharingEnabled").Value);
            //int shareindex = Convert.ToInt32(programsettings.Single<Setting>(i => i.Name == "AcceptInvitationsFrom").Value);
            //switch (shareindex)
            //{
            //    case 0:
            //        radioShareNobody.IsChecked = true;
            //        break;
            //    case 1:
            //        radioShareTrusted.IsChecked = true;
            //        break;
            //    case 2:
            //        radioShareAnybody.IsChecked = true;
            //        break;
            //    default:
            //        radioShareNobody.IsChecked = true;
            //        break;
            //}
            //int mergeindex = Convert.ToInt32(programsettings.Single<Setting>(i => i.Name == "MergeFrom").Value);
            //switch (mergeindex)
            //{
            //    case 0:
            //        radioMergeNobody.IsChecked = true;
            //        break;
            //    case 1:
            //        radioMergeTrusted.IsChecked = true;
            //        break;
            //    case 2:
            //        radioMergeAnybody.IsChecked = true;
            //        break;
            //    default:
            //        radioMergeNobody.IsChecked = true;
            //        break;
            //}
            //String senders = programsettings.Single<Setting>(i => i.Name == "TrustedSenderList").Value;
            //if (!string.IsNullOrEmpty(senders))
            //{
            //    string[] senderarray = senders.Split(',');
            //    foreach (string sender in senderarray)
            //    {
            //        trustedsenders.Add(sender);
            //    }
            //}
            //listviewSenderList.ItemsSource = trustedsenders;
            //checkboxSoundEnable.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "EnableSound").Value);
            //sliderMasterVol.Value = Convert.ToInt32(programsettings.Single<Setting>(i => i.Name == "MasterVolume").Value);
            //checkboxEnableText.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "EnableText").Value);
            //checkboxEnableTimer.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "EnableTimers").Value);
            //checkboxStopTrigger.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "StopTriggerSearch").Value);
            //checkboxMinimize.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "Minimize").Value);
            //checkboxMatchLog.IsChecked = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "DisplayMatchLog").Value);
            //checkboxLogMatches.IsChecked = logmatchestofile = Convert.ToBoolean(programsettings.Single<Setting>(i => i.Name == "LogMatchesToFile").Value);
            //textboxLogMatches.Text = logmatchlocation = programsettings.Single<Setting>(i => i.Name == "LogMatchFilename").Value;
            //textboxClipboard.Text = programsettings.Single<Setting>(i => i.Name == "Clipboard").Value;
            //textboxEQFolder.Text = programsettings.Single<Setting>(i => i.Name == "EQFolder").Value;
            //textboxMediaFolder.Text = programsettings.Single<Setting>(i => i.Name == "ImportedMediaFolder").Value;
            //textboxDataFolder.Text = programsettings.Single<Setting>(i => i.Name == "DataFolder").Value;
            //textboxMaxEntries.Text = programsettings.Single<Setting>(i => i.Name == "MaxLogEntry").Value;
        }
        private void LoadCharacters()
        {
            using (var db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<Character> characters = db.GetCollection<Character>("characters");
                foreach(Character character in characters.FindAll())
                {
                    //check to see if some how monitor is enabled but monitoring disabled
                    if (character.Monitor)
                    {
                        character.Monitoring = true;
                    }
                    CharacterCollection charcollection = new CharacterCollection
                    {
                        Name = character.Name,
                        CharacterProfile = character
                    };
                    _characters.Add(charcollection);
                }
            }
            CollectionViewSource charactervs = new CollectionViewSource();
            SortDescription desc = new SortDescription("Name", ListSortDirection.Ascending);
            charactervs.IsLiveFilteringRequested = true;
            charactervs.SortDescriptions.Add(desc);
            charactervs.Source = _characters;
            _listviewCharacters.ItemsSource = charactervs.View;
        }
        private void ActivateLog()
        {
            activatedDatagrid.ItemsSource = _activatedtriggers.Collection;
        }
        private void LoadTriggers()
        {
            //Load Triggers into Collection            
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<EQTrigger> triggers = db.GetCollection<EQTrigger>("triggers");
                ILiteCollection<TriggerGroupProperty> triggergroups = db.GetCollection<TriggerGroupProperty>("groups");
                foreach(TriggerGroupProperty group in triggergroups.Find(x => x.ParentId == null))
                {
                    TriggerManager newtrigger = new TriggerManager
                    {
                        Name = group.Name,
                        NodeType = "group",
                        Icon = GlobalVariables.foldericon,
                        IsRootNode = true,
                        ParentNode = null,
                        IsActive = false,
                        TriggerGroup = group
                    };
                    if(group.SubGroups.Count > 0)
                    {
                        LoadSubGroup(newtrigger, db);
                    }
                    if(group.Triggers.Count > 0)
                    {
                        foreach(string triggerid in group.Triggers)
                        {
                            EQTrigger eqtrigger = triggers.FindOne(x => x.Id == triggerid);
                            TriggerManager newmanager = new TriggerManager
                            {
                                Name = newtrigger.Name,
                                NodeType = "trigger",
                                Icon = GlobalVariables.triggericon,
                                Trigger = eqtrigger,
                                ParentNode = newtrigger,
                                IsActive = false
                            };
                            _triggermasterlist.Add(eqtrigger);
                        }
                    }                    
                    _triggermanager.Add(newtrigger);
                    treeview.LoadOnDemandCommand = newtrigger.TreeViewOnDemandCommand;
                }
            }
            //Assign trigger collection to tree view
            treeview.ItemsSource = _triggermanager;
        }
        private void LoadSubGroup(TriggerManager triggermanager, LiteDatabase db)
        {
            ILiteCollection<EQTrigger> triggers = db.GetCollection<EQTrigger>("triggers");
            ILiteCollection<TriggerGroupProperty> triggergroups = db.GetCollection<TriggerGroupProperty>("groups");
            foreach (string groupid in triggermanager.TriggerGroup.SubGroups)
            {
                TriggerGroupProperty newgroup = triggergroups.FindOne(x => x.Id == groupid);
                TriggerManager newtrigger = new TriggerManager
                {
                    Name = newgroup.Name,
                    NodeType = "group",
                    Icon = GlobalVariables.foldericon,
                    IsRootNode = false,
                    ParentNode = triggermanager,
                    IsActive = false,
                    TriggerGroup = newgroup
                };
                if (newgroup.SubGroups.Count > 0)
                {
                    LoadSubGroup(newtrigger,db);
                }
                if (newgroup.Triggers.Count > 0)
                {
                    foreach (string triggerid in newgroup.Triggers)
                    {
                        EQTrigger eqtrigger = triggers.FindOne(x => x.Id == triggerid);
                        TriggerManager newmanager = new TriggerManager
                        {
                            Name = eqtrigger.Name,
                            NodeType = "trigger",
                            Icon = GlobalVariables.triggericon,
                            Trigger = eqtrigger,
                            ParentNode = newtrigger,
                            IsActive = false
                        };
                        _triggermasterlist.Add(eqtrigger);
                        newtrigger.SubGroups.Add(newmanager);
                    }
                }
                triggermanager.SubGroups.Add(newtrigger);
            }
        }
        private void PurgeFromTriggers(string name)
        {
            foreach (EQTrigger trigger in _triggermasterlist)
            {
                if (trigger.ActiveCharacters.Contains(name))
                {
                    trigger.ActiveCharacters.Remove(name);
                }
            }
        }
        private void CreateCharacter()
        {
            CharacterEdit chareditor = new CharacterEdit();
            Boolean rval = (bool)chareditor.ShowDialog();
            if (chareditor.ReturnChar != null)
            {
                using(var db = new LiteDatabase(GlobalVariables.defaultDB))
                {
                    ILiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Insert(chareditor.ReturnChar);
                }
                _characters.Add(new CharacterCollection { Name = chareditor.ReturnChar.Name, CharacterProfile = chareditor.ReturnChar });
            }
        }
        private void DefaultSettings()
        {
            Setting mastervolume = new Setting
            {
                Name = "MasterVolume",
                Value = "100"
            };
            Setting update = new Setting
            {
                Name = "ApplicationUpdate",
                Value = "true"
            };
            Setting enablesound = new Setting
            {
                Name = "EnableSound",
                Value = "true"
            };
            Setting enabletext = new Setting
            {
                Name = "EnableText",
                Value = "true"
            };
            Setting enabletimers = new Setting
            {
                Name = "EnableTimers",
                Value = "true"
            };
            Setting minimize = new Setting
            {
                Name = "Minimize",
                Value = "false"
            };
            Setting stoptrigger = new Setting
            {
                Name = "StopTriggerSearch",
                Value = "false"
            };
            Setting displaymatchlog = new Setting
            {
                Name = "DisplayMatchLog",
                Value = "true"
            };
            Setting maxlogentry = new Setting
            {
                Name = "MaxLogEntry",
                Value = "100"
            };
            Setting logmatchtofile = new Setting
            {
                Name = "LogMatchesToFile",
                Value = "false"
            };
            Setting logmatchfilename = new Setting
            {
                Name = "LogMatchFilename",
                Value = ""
            };
            Setting clipboard = new Setting
            {
                Name = "Clipboard",
                Value = "{C}"
            };
            Setting eqfolder = new Setting
            {
                Name = "EQFolder",
                Value = @"C:\EQ"
            };
            Setting importedmedia = new Setting
            {
                Name = "ImportedMediaFolder",
                Value = $"{GlobalVariables.workingdirectory}\\ImportedMedia"
            };
            Setting datafolder = new Setting
            {
                Name = "DataFolder",
                Value = GlobalVariables.workingdirectory
            };
            Setting enablesharing = new Setting
            {
                Name = "SharingEnabled",
                Value = "true"
            };
            Setting enableincoming = new Setting
            {
                Name = "EnableIncomingTriggers",
                Value = "true"
            };
            Setting acceptfrom = new Setting
            {
                Name = "AcceptInvitationsFrom",
                Value = "2"
            };
            Setting mergefrom = new Setting
            {
                Name = "MergeFrom",
                Value = "2"
            };
            Setting senderlist = new Setting
            {
                Name = "TrustedSenderList",
                Value = ""
            };
            Setting logarchive = new Setting
            {
                Name = "LogArchiveFolder",
                Value = $"{GlobalVariables.workingdirectory}\\Archive"
            };
            Setting autoarchive = new Setting
            {
                Name = "AutoArchive",
                Value = "true"
            };
            Setting compress = new Setting
            {
                Name = "CompressArchive",
                Value = "true"
            };
            Setting archivemethod = new Setting
            {
                Name = "ArchiveMethod",
                Value = "Size Threshold"
            };
            Setting logsize = new Setting
            {
                Name = "LogSize",
                Value = "50"
            };
            Setting autodelete = new Setting
            {
                Name = "AutoDelete",
                Value = "true"
            };
            Setting deletearchive = new Setting
            {
                Name = "DeleteArchives",
                Value = "90"
            };
            Setting shareuri = new Setting
            {
                Name = "ShareServiceURI",
                Value = @"http:\\shareservice.com"
            };
            Setting reference = new Setting
            {
                Name = "Reference",
                Value = "You"
            };
            Setting enabledebug = new Setting
            {
                Name = "EnableDebug",
                Value = "false"
            };
            Setting archiveschedule = new Setting
            {
                Name = "ArchiveSchedule",
                Value = ""
            };
            Setting darkmode = new Setting
            {
                Name = "DarkMode",
                Value = "false"
            };
            using (var db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<Setting> settings = db.GetCollection<Setting>("settings");
                settings.Insert(mastervolume);
                settings.Insert(update);
                settings.Insert(enablesound);
                settings.Insert(enabletext);
                settings.Insert(enabletimers);
                settings.Insert(minimize);
                settings.Insert(stoptrigger);
                settings.Insert(displaymatchlog);
                settings.Insert(maxlogentry);
                settings.Insert(logmatchtofile);
                settings.Insert(logmatchfilename);
                settings.Insert(clipboard);
                settings.Insert(eqfolder);
                settings.Insert(importedmedia);
                settings.Insert(datafolder);
                settings.Insert(enablesharing);
                settings.Insert(enableincoming);
                settings.Insert(acceptfrom);
                settings.Insert(mergefrom);
                settings.Insert(senderlist);
                settings.Insert(logarchive);
                settings.Insert(autoarchive);
                settings.Insert(compress);
                settings.Insert(archivemethod);
                settings.Insert(logsize);
                settings.Insert(autodelete);
                settings.Insert(deletearchive);
                settings.Insert(shareuri);
                settings.Insert(reference);
                settings.Insert(enabledebug);
                settings.Insert(archiveschedule);
                settings.Insert(darkmode);
            }
        }

        #region Form Functions
        private void UpdateLineCount(int value)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            syncontext.Post(new SendOrPostCallback(o =>
            {
                _totallinecount += (int)o;
                txtblockStatus.DataContext = _totallinecount;
            }), value);
            stopwatch.Stop();
            Console.WriteLine($"Line Update Took: {stopwatch.Elapsed.TotalSeconds} Seconds");
        }
        #endregion

        #region DockingManager Actions
        private void dockingmanager_CloseAllTabs(object sender, CloseTabEventArgs e)
        {

        }
        private void dockingmanager_CloseOtherTabs(object sender, CloseTabEventArgs e)
        {

        }
        private void dockingmanager_WindowMoving(object sender, WindowMovingEventArgs e)
        {

        }
        private void dockingmanager_DocumentClosing(object sender, CancelingRoutedEventArgs e)
        {

        }
        private void dockingmanager_IsSelectedDocument(FrameworkElement sender, IsSelectedChangedEventArgs e)
        {

        }
        private void mergedockingmanager_TabClosed(object sender, CloseTabEventArgs e)
        {
            mergedockingmanager.Visibility = Visibility.Hidden;
            triggerdockmanager.SetValue(Grid.ColumnSpanProperty, 2);
        }
        private void mergetreeview_ItemDragStarting(object sender, TreeViewItemDragStartingEventArgs e)
        {
            Console.WriteLine("Merge Tree Item Drag Starting");
        }
        private void mergetreeview_ItemDropping(object sender, TreeViewItemDroppingEventArgs e)
        {
            Console.WriteLine("Merge Tree Item Dropping");
        }
        private void mergetreeview_ItemDropped(object sender, TreeViewItemDroppedEventArgs e)
        {
            Console.WriteLine("Merge Tree Item Dropped");
        }
        private void treeview_ItemDragStarted(object sender, TreeViewItemDragStartedEventArgs e)
        {
            Console.WriteLine("TreeView Drag Started");
        }
        #endregion

        #region Ribbon
        private void _ribbon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BackStageExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            CreateCharacter();
        }
        private void editUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Character selected = ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile;
                selected.EditCharacter();
                using (var db = new LiteDatabase(GlobalVariables.defaultDB))
                {
                    ILiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Update(selected);
                }
            }
            catch(Exception ex)
            {
                MessageBoxResult mbox = Xceed.Wpf.Toolkit.MessageBox.Show("Character Not Selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void removeUser_Click(object sender, RoutedEventArgs e)
        {
            CharacterCollection selected = (CharacterCollection)_listviewCharacters.SelectedItem;
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<Character> characters = db.GetCollection<Character>("characters");
                characters.Delete(selected.CharacterProfile.Id);
            }
            _characters.Remove(selected);
            PurgeFromTriggers(selected.CharacterProfile.Id);
        }
        #endregion

        #region TreeView
        private void SfTreeView_QueryNodeSize(object sender, QueryNodeSizeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                //Returns specified item height for items.
                e.Height = 25;
                e.Handled = true;
            }
            else
            {
                // Returns item height based on the content loaded.
                e.Handled = true;
            }
        }
        private void treeview_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            TriggerManager selecteditem = (sender as SfTreeView).CurrentItem as TriggerManager;
            string treename = (sender as SfTreeView).Name;

            if (!treename.Contains("mergetreeview"))
            {
                if (selecteditem.NodeType.Equals("group"))
                {
                    addTriggerGroup.IsEnabled = true;
                    menuaddtoselectedgroup.IsEnabled = true;
                    editTriggerGroup.IsEnabled = true;
                    removeTriggerGroup.IsEnabled = true;
                    addTrigger.IsEnabled = true;
                    editTrigger.IsEnabled = false;
                    removeTrigger.IsEnabled = false;

                }
                if (selecteditem.NodeType.Equals("trigger"))
                {
                    menuaddtoselectedgroup.IsEnabled = false;
                    editTriggerGroup.IsEnabled = false;
                    removeTriggerGroup.IsEnabled = false;
                    addTrigger.IsEnabled = false;
                    editTrigger.IsEnabled = true;
                    removeTrigger.IsEnabled = true;
                }
            }
            else
            {
                addTriggerGroup.IsEnabled = false;
                menuaddtoselectedgroup.IsEnabled = false;
                editTriggerGroup.IsEnabled = false;
                removeTriggerGroup.IsEnabled = false;
                addTrigger.IsEnabled = false;
                editTrigger.IsEnabled = false;
                removeTrigger.IsEnabled = false;
            }
        }
        private void treeview_ItemDropped(object sender, TreeViewItemDroppedEventArgs e)
        {
            Console.WriteLine("TreeView Item Dropped");
            //Folder where our item is going
            if (e.TargetNode != null)
            {                
                TriggerManager desttm = (TriggerManager)e.TargetNode.Content;
                MessageBoxResult confirmdrop = Xceed.Wpf.Toolkit.MessageBox.Show($"Import Triggers on Group: {desttm.Name}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmdrop == MessageBoxResult.Yes)
                {
                    //The item we're draggin
                    foreach (TreeViewNode node in e.DraggingNodes)
                    {
                        TriggerManager droppednode = (TriggerManager)node.Content;
                        if (e.TargetNode.ParentNode == null && droppednode.NodeType == "trigger")
                        {
                            MessageBoxResult mbox = Xceed.Wpf.Toolkit.MessageBox.Show("Can't Move Trigger to Root", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (droppednode.NodeType == "trigger")
                            {
                                //Add the trigger ID to the parent node Triggers
                                //write the parent node to the DB
                                desttm.TriggerGroup.Triggers.Add(droppednode.Trigger.Id);
                                WriteGroupToDB(desttm.TriggerGroup);
                                //Add the Group Id to the trigger
                                //write the trigger to the DB
                                droppednode.Trigger.GroupId = desttm.TriggerGroup.Id;
                                WriteTriggerToDB(droppednode.Trigger);
                            }
                            else if (droppednode.NodeType == "group")
                            {
                                //add the parent id to the dropped node, then import it
                                droppednode.TriggerGroup.ParentId = desttm.TriggerGroup.Id;
                                WriteGroupToDB(droppednode.TriggerGroup);

                                //add the dropped node id to the parent subgroup
                                //write the parent group to the db
                                desttm.TriggerGroup.SubGroups.Add(droppednode.TriggerGroup.Id);
                                WriteGroupToDB(desttm.TriggerGroup);

                                //Call a recursive function that goes through the groups and subgroups to pull everything over
                                //we'll check if somebody imports an empty group, people do dumb stuff
                                if (droppednode.SubGroups.Count > 0)
                                { ImportTriggerGroup(droppednode); }
                            }
                        }
                    }
                }
            }
        }
        private void treeview_ItemDropping(object sender, TreeViewItemDroppingEventArgs e)
        {
            //Restrict the dropping on certain nodes
            if (e.TargetNode != null )
            {
                try
                {
                    TriggerManager node = e.TargetNode.Content as TriggerManager;
                    ObservableCollection<TreeViewNode> dragsource = e.DraggingNodes;
                    if(dragsource != null && dragsource.Count > 0)
                    {
                        TriggerManager droplocation = (e.TargetNode).Content as TriggerManager;
                        if(droplocation.NodeType == "group")
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Item Dropping in TreeView Failure");
                }
            }
            else
            {
                e.Handled = true;
            }
        }
        private void treeview_ItemDragOver(object sender, TreeViewItemDragOverEventArgs e)
        {
            Console.WriteLine("TreeView item Drag Over");
            if (e.TargetNode != null)
            {
                TriggerManager node = e.TargetNode.Content as TriggerManager;
                if (node.SubGroups.Count() > 0)
                {
                    treeview.ExpandNode(e.TargetNode);
                }
            }
        }
        private void SetCheckedItems(TriggerManager tm, Boolean enable)
        {
            if (tm.NodeType == "trigger")
            {
                if (enable) { EnableTrigger(tm); }
                else { DisableTrigger(tm); }
                tm.IsActive = enable;
            }
            if (tm.NodeType == "group")
            {
                tm.IsActive = enable;
            }
            if (tm.SubGroups.Count > 0)
            {
                foreach (TriggerManager sub in tm.SubGroups)
                {
                    SetCheckedItems(sub, enable);
                }
            }

        }
        private bool? SetCheckedItems(TriggerManager tm)
        {
            bool? updateparent = false;
            Boolean partialcheck = false;
            List<bool?> childrenchecked = new List<bool?>();
            //Call recursively until we get to the bottom of the tree
            if (tm.SubGroups.Count > 0)
            {
                //go through each sub group, the return value is if the sub group is active or not
                foreach (TriggerManager sub in tm.SubGroups)
                {
                    childrenchecked.Add(SetCheckedItems(sub));
                }
            }
            //if the node is a trigger and has the selected character as active, then mark the node active and return true so the parent knows we have an active trigger
            if (tm.NodeType == "trigger")
            {
                if (tm.Trigger.ActiveCharacters.Contains(_selectedcharacter))
                {
                    tm.IsActive = true;
                    updateparent = true;
                }
                else
                {
                    tm.IsActive = false;
                    updateparent = false;
                }
            }
            //if this is a node type "group" and all of the childrenchecked = true, then add this item to the checked list
            if (tm.NodeType == "group")
            {
                //count how many branches
                int totalnodes = tm.SubGroups.Count;
                //keep track of how many branches are active
                int checkednodes = 0;

                //If a group doesn't have any children, then mark it inactive, nothing to see here.
                if (totalnodes == 0)
                {
                    updateparent = false;
                    tm.IsActive = false;
                }
                else
                {
                    //go through the list of setcheckeditems on the subgroups
                    //   if the item is true, then add a checkednode to the list
                    //   if one of the items is null, then we have a partial check and that will float to the top
                    foreach (bool? children in childrenchecked)
                    {
                        if (children == true)
                        {
                            checkednodes++;
                        }
                        if (children == null)
                        {
                            partialcheck = true;
                        }
                    }
                    //if we got a partial check, mark the node null and move on.
                    if (partialcheck)
                    {
                        tm.IsActive = null;
                        updateparent = null;
                    }
                    //All of the sub groups/triggers are active mark the group with a full check
                    else if (checkednodes == totalnodes && totalnodes > 0)
                    {
                        updateparent = true;
                        tm.IsActive = true;
                    }
                    else if (totalnodes > 0 && checkednodes == 0)
                    {
                        updateparent = false;
                        tm.IsActive = false;
                    }
                    else if (totalnodes > 0 && checkednodes < totalnodes && checkednodes != 0)
                    {
                        updateparent = null;
                        tm.IsActive = null;
                    }

                }
            }

            return updateparent;
        }
        private void UpdateCheckedItems()
        {
            //Update Checked Items collection for treeview
            foreach (TriggerManager tm in _triggermanager)
            {
                tm.IsActive = SetCheckedItems(tm);
            }
        }
        #endregion

        #region TriggerGroup Clicks
        private void AddTopLevel_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            //If no group is selected or a root node is selected.
            //Top level group is another root node
            //Else if we have a trigger selected, we can add a group to the trigger parent node.
            //otherwise the selecteditem is the parent group and we need to add a group to it.
            if (selecteditem == null || selecteditem.IsRootNode)
            {
                TriggerManager newtrigger = new TriggerManager { NodeType = "group", Icon = GlobalVariables.foldericon, IsRootNode = true };
                newtrigger.CreateRootTriggerGroup();
                _triggermanager.Add(newtrigger);
                //resort collection
                _triggermanager = new ObservableCollection<TriggerManager>(_triggermanager.OrderBy(i => i.Name));
                treeview.ItemsSource = _triggermanager;
            }
            else if (selecteditem.NodeType.Equals("trigger"))
            {
                selecteditem.ParentNode.AddTriggerGroup();
            }
            else
            {
                selecteditem.AddTriggerGroup();
            }
        }
        private void AddToSelected_Click(object sender, RoutedEventArgs e)
        {
            ((TriggerManager)treeview.SelectedItem).AddTriggerGroup();
        }
        private void editTriggerGroup_Click(object sender, RoutedEventArgs e)
        {
            ((TriggerManager)treeview.SelectedItem).EditTriggerGroup();
        }
        private void removeTriggerGroup_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            selecteditem.RemoveTriggerGroup();
            if (selecteditem.IsRootNode)
            {
                _triggermanager.Remove(selecteditem);
            }
        }
        #endregion

        #region Trigger Clicks
        private void addTrigger_Click(object sender, RoutedEventArgs e)
        {
            EQTrigger newtrigger = ((TriggerManager)treeview.SelectedItem).AddTrigger(_characters);
            if (newtrigger != null)
            {
                _triggermasterlist.Add(newtrigger);
            }
        }
        private void editTrigger_Click(object sender, RoutedEventArgs e)
        {
            ((TriggerManager)treeview.SelectedItem).EditTrigger(_characters);
        }
        private void removeTrigger_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            selecteditem.RemoveTrigger();
            _triggermasterlist.Remove(selecteditem.Trigger);
        }
        #endregion

        #region ListView
        private void _listviewCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)e.Source).SelectedItem == null)
            {
                if (_listviewCharacters.Items.Count > 0)
                {
                    _listviewCharacters.SelectedIndex = 0;
                }
            }
            else
            {
                txtblockProfile.Text = ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Profile;
                _selectedcharacter = ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Id;
                Console.WriteLine($"Changed Character: {_selectedcharacter}");
            }
            UpdateCheckedItems();
        }
        private void WalkTreeCheckedItems(TreeViewNode tvn)
        {
            TriggerManager tm = (TriggerManager)tvn.Content;
            if (tm.NodeType == "trigger")
            {
                if (tm.Trigger.ActiveCharacters.Contains(((CharacterCollection)_listviewCharacters.SelectedItem).Name))
                {
                    Console.WriteLine($"{((CharacterCollection)_listviewCharacters.SelectedItem).Name} Adding to checked node {tm.Trigger.Name}");
                    treeview.CheckedItems.Add(tvn);
                }
            }
            if (tvn.ChildNodes.Count > 0)
            {
                foreach (TreeViewNode newtvn in tvn.ChildNodes)
                {
                    WalkTreeCheckedItems(newtvn);
                }
            }
        }
        private void _listviewCharacters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Boolean monitorstatus = ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Monitoring;
            if (monitorstatus)
            {
                ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Monitoring = false;
            }
            else
            {
                ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Monitoring = true;
                StartMonitor(((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile);
            }
        }
        private void MenuItemCharEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Character selected = ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile;
                selected.EditCharacter();
                using (var db = new LiteDatabase(GlobalVariables.defaultDB))
                {
                    ILiteCollection<Character> characters = db.GetCollection<Character>("characters");
                    characters.Update(selected);
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult mbox = Xceed.Wpf.Toolkit.MessageBox.Show("Character Not Selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MenuItemCharDelete_Click(object sender, RoutedEventArgs e)
        {
            CharacterCollection selected = (CharacterCollection)_listviewCharacters.SelectedItem;
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<Character> characters = db.GetCollection<Character>("characters");
                characters.Delete(selected.CharacterProfile.Id);
            }
            _characters.Remove(selected);
            PurgeFromTriggers(selected.CharacterProfile.Id);
        }
        private void MenuItemStartMonitor_Click(object sender, RoutedEventArgs e)
        {
            Character selected = ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile;
            selected.Monitoring = true;
            StartMonitor(selected);
        }
        private void MenuItemStopMonitor_Click(object sender, RoutedEventArgs e)
        {
            Character selected = ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile;
            selected.Monitoring = false;
        }
        #endregion

        private void _treeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            TriggerManager target = (((e.Source as CheckBox).DataContext as TreeViewNode).Content as TriggerManager);
            //if Trigger, activate it for the selected character
            if (target.NodeType == "trigger")
            {
                EnableTrigger(target);
            }
            //if Group, iterate through tree and activate each trigger on the way down for the selected character
            if (target.NodeType == "group")
            {
                SetCheckedItems(target, true);
            }
            //walk back up the tree and do parent checks
            target.ClimbTree(true);
            //If the trigger doesn't have any characters subscribing to it, then remove it from active triggers
            _activetriggers.Refactor(_triggermasterlist);
        }
        private void _treeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

            TriggerManager target = (((e.Source as CheckBox).DataContext as TreeViewNode).Content as TriggerManager);
            //if Trigger, deactivate it for the selected character
            if (target.NodeType == "trigger")
            {
                DisableTrigger(target);
            }
            //if Group, iterate through tree and deactivate each trigger on the way down for the selected character
            if (target.NodeType == "group")
            {
                SetCheckedItems(target, false);
            }
            //walk back up the tree and do parent unchecks
            target.ClimbTree(false);
            //If the trigger doesn't have any characters subscribing to it, then remove it from active triggers
            _activetriggers.Refactor(_triggermasterlist);
        }

        #region Trigger
        private void DisableTrigger(TriggerManager tm)
        {
            string character = ((CharacterCollection)(_listviewCharacters.SelectedItem)).CharacterProfile.Id;
            Console.WriteLine($"Unchecked Trigger{tm.Name}");

            //check if character is already in the list
            //string found = tm.Trigger.ActiveCharacters.Single<string>(x => x.Contains(character));
            bool found = tm.Trigger.ActiveCharacters.Contains(character);
            if (found)
            {
                //Remove trigger, which also updates DB
                tm.Trigger.RemoveCharacter(character);
            }
        }
        private void EnableTrigger(TriggerManager tm)
        {
            Console.WriteLine("Calling Node Checked");
            string character = ((CharacterCollection)(_listviewCharacters.SelectedItem)).CharacterProfile.Id;
            Console.WriteLine($"Checked Trigger{tm.Name}");
            bool found = tm.Trigger.ActiveCharacters.Contains(character);
            if(!found)
            {
                tm.Trigger.AddCharacter(character);
            }
        }
        private void WriteGroupToDB(TriggerGroupProperty tg)
        {
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<TriggerGroupProperty> triggergroups = db.GetCollection<TriggerGroupProperty>("groups");
                //See if the group exists, if so update it
                TriggerGroupProperty test = triggergroups.FindOne(x => x.Id == tg.Id);
                if (test != null)
                {
                    triggergroups.Update(tg);
                }
                else
                {
                    triggergroups.Insert(tg);
                }
            }
        }
        private void DeleteGroupFromDB(TriggerGroupProperty tg)
        {
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<TriggerGroupProperty> triggergroups = db.GetCollection<TriggerGroupProperty>("groups");
                triggergroups.Delete(tg.Id);
            }
        }
        private void WriteTriggerToDB(EQTrigger trigger)
        {
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<EQTrigger> triggers = db.GetCollection<EQTrigger>("triggers");
                //See if the group exists, if so update it
                EQTrigger test = triggers.FindOne(x => x.Id == trigger.Id);
                if (test != null)
                {
                    triggers.Update(trigger);
                }
                else
                {
                    triggers.Insert(trigger);
                }
            }
        }
        private void DeleteTriggerFromDB(EQTrigger trigger)
        {
            using (LiteDatabase db = new LiteDatabase(GlobalVariables.defaultDB))
            {
                ILiteCollection<EQTrigger> triggers = db.GetCollection<EQTrigger>("triggers");
                triggers.Delete(trigger.Id);
            }
        }
        #endregion

        #region Import/Export Triggers
        private void ImportFromGina_Click(object sender, RoutedEventArgs e)
        {
            //clear the merge tree in case we're loading another set
            _mergemanager.Clear();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "GINA Trigger Package|*.gtp";
            if (fileDialog.ShowDialog() == true)
            {
                using (ZipArchive archive = ZipFile.OpenRead(fileDialog.FileName))
                {
                    //Load the xml
                    if (archive.Entries.Count > 0)
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name == "ShareData.xml")
                            {
                                ZipArchiveEntry triggersxml = entry;
                                using (StreamReader streamtriggers = new StreamReader(triggersxml.Open()))
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(streamtriggers.ReadToEnd());
                                    string json = JsonConvert.SerializeXmlNode(doc);
                                    JToken jsontoken = JObject.Parse(json);
                                    ParseGina(jsontoken.SelectToken("SharedData"));
                                }
                            }
                            if (entry.Name.Contains("wav"))
                            {
                                //Check if EQAudioTriggers folder exists, if not create.
                                bool mainPath = Directory.Exists($"{GlobalVariables.workingdirectory}\\ImportedSounds");
                                if (!mainPath)
                                {
                                    Directory.CreateDirectory($"{GlobalVariables.workingdirectory}\\ImportedSounds");
                                }
                                //export file to export sound dir
                                String extracttofile = $"{GlobalVariables.workingdirectory}\\ImportedSounds\\{entry.Name}";
                                entry.ExtractToFile(extracttofile);
                            }
                        }
                    }
                }
            }
            if(_mergemanager.Count > 0)
            {
                mergetreeview.ItemsSource = _mergemanager;
                triggerdockmanager.SetValue(Grid.ColumnSpanProperty, 1);
                mergedockingmanager.Visibility = Visibility.Visible;
            }
        }
        private TriggerManager CreateJTokenGroup(JToken newtoken, TriggerManager parentnode)
        {
            TriggerGroupProperty newgroup = new TriggerGroupProperty
            {
                Name = newtoken["Name"].ToString(),
                Comments = newtoken["Comments"].ToString(),
                Id = Utilities.IdGenerator(),
                DefaultEnabled = newtoken["EnableByDefault"].ToObject<Boolean>()
            };
            TriggerManager tm = new TriggerManager
            {
                Name = newgroup.Name,
                NodeType = "group",
                IsActive = false,
                IsRootNode = true,
                ParentNode = parentnode,
                Icon = GlobalVariables.foldericon,
                TriggerGroup = newgroup
                //Add File Information
            };
            return tm;
        }
        private TriggerManager CreateJTokenTrigger(JToken newtoken, TriggerManager parentnode)
        {
            EQTrigger newtrigger = new EQTrigger
            {
                Name = newtoken["Name"].ToString(),
                SearchText = newtoken["TriggerText"].ToString(),
                Comments = newtoken["Comments"].ToString(),
                UseRegex = newtoken["EnableRegex"].ToObject<Boolean>(),
                UseBasicText = newtoken["UseText"].ToObject<Boolean>(),
                BasicText = newtoken["DisplayText"].ToString(),
                UseClipboardText = newtoken["CopyToClipboard"].ToObject<Boolean>(),
                BasicClipboardText = newtoken["ClipboardText"].ToString(),
                RadioBasicTTS = newtoken["UseTextToVoice"].ToObject<Boolean>(),
                UseBasicInterrupt = newtoken["InterruptSpeech"].ToObject<Boolean>(),
                BasicTTS = newtoken["TextToVoiceText"].ToString(),
                RadioBasicPlay = newtoken["PlayMediaFile"].ToObject<Boolean>(),
                TimerType = newtoken["TimerType"].ToString(),
                TimerName = newtoken["TimerName"].ToString(),
                RestartOnTimerId = newtoken["RestartBasedOnTimerName"].ToObject<Boolean>(),
                EndingNotify = newtoken["UseTimerEnding"].ToObject<Boolean>(),
                EndedNotify = newtoken["UseTimerEnded"].ToObject<Boolean>(),
                CounterReset = newtoken["UseCounterResetTimer"].ToObject<Boolean>(),
                Category = newtoken["Category"].ToString(),
                FastCheck = newtoken["UseFastCheck"].ToObject<Boolean>()
            };
            TriggerManager tm = new TriggerManager
            {
                Name = newtrigger.Name,
                NodeType = "trigger",
                Icon = GlobalVariables.triggericon,
                Trigger = newtrigger,
                ParentNode = parentnode,
                IsActive = false
            };

            //Add End Early Text
            if (newtoken["TimerEarlyEnders"].Count() > 0)
            {
                if (newtoken["TimerEarlyEnders"]["EarlyEnder"].GetType().ToString() == "Newtonsoft.Json.Linq.JObject")
                {
                    EndEarlyTrigger endEarlyTrigger = new EndEarlyTrigger
                    {
                        SearchText = (String)newtoken["TimerEarlyEnders"]["EarlyEnder"]["EarlyEndText"],
                        UseRegex = (Boolean)newtoken["TimerEarlyEnders"]["EarlyEnder"]["EnableRegex"]
                    };
                    newtrigger.EndEarlyTriggers.Add(endEarlyTrigger);
                }
                else
                {
                    JArray earlyenders = (JArray)newtoken["TimerEarlyEnders"]["EarlyEnder"];
                    if ((earlyenders.Children()).Count() > 0)
                    {
                        foreach (JToken childtoken in earlyenders.Children())
                        {
                            EndEarlyTrigger endEarlyTrigger = new EndEarlyTrigger
                            {
                                SearchText = (String)childtoken["EarlyEndText"],
                                UseRegex = (Boolean)childtoken["EnableRegex"]
                            };
                            newtrigger.EndEarlyTriggers.Add(endEarlyTrigger);
                        }
                    }
                }
            }
            //Generate durations
            /*            TimerDuration = "";
                TimerVisibleDuration = "";
                TimerStartBehavior = "";
                TimerEndingTime = "";
            CounterResetDuration = "";
            */
            return tm;
        }
        private void ParseGina(JToken jsontoken)
        {
            if ((jsontoken["TriggerGroups"]["TriggerGroup"]).GetType().ToString() == "Newtonsoft.Json.Linq.JArray")
            {
                foreach (JToken grouptoken in ((JArray)(jsontoken["TriggerGroups"]["TriggerGroup"])).Children())
                {
                    TriggerManager tm = CreateJTokenGroup(grouptoken, null);
                    GetTriggerGroups(grouptoken, tm);
                    _mergemanager.Add(tm);
                }
            }
            else
            {
                TriggerManager tm = CreateJTokenGroup(jsontoken.SelectToken("TriggerGroups.TriggerGroup"), null);
                GetTriggerGroups(jsontoken.SelectToken("TriggerGroups.TriggerGroup"), tm);
                _mergemanager.Add(tm);
            }
        }
        private void GetTriggerGroups(JToken jsontoken, TriggerManager parentnode)
        {
            foreach (JToken token in jsontoken.Children())
            {
                switch (((JProperty)token).Name)
                {
                    case "TriggerGroups":
                        if ((jsontoken["TriggerGroups"]["TriggerGroup"]).GetType().ToString() == "Newtonsoft.Json.Linq.JArray")
                        {
                            foreach (JToken newtoken in ((JArray)(jsontoken["TriggerGroups"]["TriggerGroup"])).Children())
                            {
                                TriggerManager tm = CreateJTokenGroup(newtoken, parentnode);
                                parentnode.SubGroups.Add(tm);
                                GetTriggerGroups(newtoken, tm);
                            }
                        }
                        else
                        {
                            if ((jsontoken["TriggerGroups"]["TriggerGroup"]).GetType().ToString() == "Newtonsoft.Json.Linq.JObject")
                            {
                                JToken newtoken = jsontoken["TriggerGroups"]["TriggerGroup"];
                                TriggerManager tm = CreateJTokenGroup(newtoken, parentnode);
                                parentnode.SubGroups.Add(tm);
                                GetTriggerGroups(newtoken, tm);
                            }
                        }
                        break;
                    case "Triggers":
                        if ((jsontoken["Triggers"]["Trigger"]).GetType().ToString() == "Newtonsoft.Json.Linq.JArray")
                        {
                            foreach (JToken newtoken in ((JArray)(jsontoken["Triggers"]["Trigger"])).Children())
                            {
                                TriggerManager tm = CreateJTokenTrigger(newtoken, parentnode);
                                parentnode.SubGroups.Add(tm);
                                GetTrigger(newtoken, tm);
                            }
                        }
                        else
                        {
                            if ((jsontoken["Triggers"]["Trigger"]).GetType().ToString() == "Newtonsoft.Json.Linq.JObject")
                            {
                                JToken newtoken = jsontoken["Triggers"]["Trigger"];
                                TriggerManager tm = CreateJTokenTrigger(newtoken, parentnode);
                                parentnode.SubGroups.Add(tm);
                                GetTrigger(newtoken, tm);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private string GetTrigger(JToken jsontoken, TriggerManager parentnode)
        {
            string rval = Utilities.IdGenerator();
            Dictionary<String, String> timerconversion = new Dictionary<string, string>();
            timerconversion.Add("NoTimer", "No Timer");
            timerconversion.Add("Timer", "Timer(Count Down)");
            timerconversion.Add("Stopwatch", "Timer(Count Up)");
            timerconversion.Add("RepeatingTimer", "Repeating Timer");
            EQTrigger newtrigger = new EQTrigger
            {
                Id = rval,
                Name = jsontoken["Name"].ToString(),
                SearchText = (String)jsontoken["TriggerText"],
                Comments = (String)jsontoken["Comments"],
                UseRegex = (bool)jsontoken["EnableRegex"],
                FastCheck = false,
                Category = "",
                BasicText = (String)jsontoken["DisplayText"],
                BasicClipboardText = (String)jsontoken["ClipboardText"],
                TimerType = timerconversion[(String)jsontoken["TimerType"]],
                TimerName = (String)jsontoken["TimerName"],
                // = (int)jsontoken["TimerDuration"],
                //TriggeredAgain = 2,
                //TimerEndingDuration = 0,
                EndingText = "",
                EndingClipboard = "",
                //TimerEnding = (bool)jsontoken["UseTimerEnding"],
                EndedClipboard = "",
                EndedDisplayText = "",
                //TimerEnded = (bool)jsontoken["UseTimerEnded"],
                //ResetCounter = (bool)jsontoken["UseCounterResetTimer"],
                //ResetCounterDuration = (int)jsontoken["CounterResetDuration"],
            };
            //protect from stupid people, if using regex always use fast check
            if (newtrigger.UseRegex)
            {
                newtrigger.FastCheck = true;
            }
            //Set Timer Behavior
            switch ((String)jsontoken["TimerStartBehavior"])
            {
                case "DoNothing":
                    newtrigger.TimerTriggered = "Do Nothing";
                    break;
                case "StartNewTimer":
                    newtrigger.TimerTriggered = "Start a new timer";
                    break;
                default:
                    break;
            }
            //Set Audio Settings
            newtrigger.UseBasicInterrupt = (bool)jsontoken["InterruptSpeech"];
            if ((bool)jsontoken["UseTextToVoice"])
            {
                //newtrigger.BasicTTS = "tts";
                newtrigger.BasicTTS = (String)jsontoken["TextToVoiceText"];
            }
            if ((bool)jsontoken["PlayMediaFile"])
            {
                newtrigger.BasicPlayFile = "file";
                //set audio file
            }

            //Add End Early Text
            if (jsontoken["TimerEarlyEnders"].Count() > 0)
            {
                if (jsontoken["TimerEarlyEnders"]["EarlyEnder"].GetType().ToString() == "Newtonsoft.Json.Linq.JObject")
                {
                    EndEarlyTrigger endEarlyTrigger = new EndEarlyTrigger
                    {
                        SearchText = (String)jsontoken["TimerEarlyEnders"]["EarlyEnder"]["EarlyEndText"],
                        UseRegex = (Boolean)jsontoken["TimerEarlyEnders"]["EarlyEnder"]["EnableRegex"]
                    };
                    newtrigger.EndEarlyTriggers.Add(endEarlyTrigger);
                }
                else
                {
                    JArray earlyenders = (JArray)jsontoken["TimerEarlyEnders"]["EarlyEnder"];
                    if ((earlyenders.Children()).Count() > 0)
                    {
                        foreach (JToken newtoken in earlyenders.Children())
                        {
                            EndEarlyTrigger endEarlyTrigger = new EndEarlyTrigger
                            {
                                SearchText = (String)newtoken["EarlyEndText"],
                                UseRegex = (Boolean)newtoken["EnableRegex"]
                            };
                            newtrigger.EndEarlyTriggers.Add(endEarlyTrigger);
                        }
                    }
                }
            }
            //Add Timer Ending Trigger
            if (jsontoken["TimerEndingTrigger"] != null)
            {
                if (jsontoken["TimerEndingTrigger"].Count() > 0)
                {
                    //newtrigger.basi = (bool)jsontoken["TimerEndingTrigger"]["UseText"];
                    newtrigger.EndedDisplayText = (String)jsontoken["TimerEndingTrigger"]["DisplayText"];
                    //newtrigger.TimerEndingDuration = (int)jsontoken["TimerEndingTime"];
                    if ((bool)jsontoken["TimerEndingTrigger"]["UseTextToVoice"])
                    {
                        newtrigger.RadioEndingTTS = true;
                        newtrigger.EndingTTS = (String)jsontoken["TimerEndingTrigger"]["TextToVoiceText"];
                        newtrigger.EndingInterrupt = (bool)jsontoken["TimerEndingTrigger"]["InterruptSpeech"];
                    }
                    if ((bool)jsontoken["TimerEndingTrigger"]["PlayMediaFile"])
                    {
                        newtrigger.RadioEndingSound = true;
                        newtrigger.EndingSoundFile = "file";
                        newtrigger.EndingInterrupt = (bool)jsontoken["TimerEndingTrigger"]["InterruptSpeech"];
                    }
                }
            }
            if (jsontoken["TimerEndedTrigger"] != null)
            {
                //Add Timer Ended Trigger
                if (jsontoken["TimerEndedTrigger"].Count() > 0)
                {
                    //newtrigger.TimerEnded = (bool)jsontoken["TimerEndedTrigger"]["UseText"];
                    newtrigger.EndedDisplayText = (String)jsontoken["TimerEndedTrigger"]["DisplayText"];
                    if ((bool)jsontoken["TimerEndedTrigger"]["UseTextToVoice"])
                    {
                        newtrigger.RadioEndedTTS = true;
                        newtrigger.EndedTTS = (String)jsontoken["TimerEndedTrigger"]["TextToVoiceText"];
                        newtrigger.EndedInterrupt = (bool)jsontoken["TimerEndedTrigger"]["InterruptSpeech"];
                    }
                    if ((bool)jsontoken["TimerEndedTrigger"]["PlayMediaFile"])
                    {
                        newtrigger.RadioEndedSound = true;
                        newtrigger.EndedInterrupt = (bool)jsontoken["TimerEndedTrigger"]["InterruptSpeech"];
                        //add media file
                    }
                }
            }
            return rval;
        }
        private void ImportTriggerGroup(TriggerManager tm)
        {
            if(tm.SubGroups.Count > 0)
            {
                foreach(TriggerManager subgroup in tm.SubGroups)
                {
                    if(subgroup.NodeType == "trigger")
                    {
                        //Ensure the child and parent information is correct
                        //Add the trigger ID to the parent node Triggers
                        //write the parent node to the DB
                        tm.TriggerGroup.Triggers.Add(subgroup.Trigger.Id);
                        WriteGroupToDB(tm.TriggerGroup);
                        //Add the Group Id to the trigger
                        //write the trigger to the DB
                        subgroup.Trigger.GroupId = tm.TriggerGroup.Id;
                        WriteTriggerToDB(subgroup.Trigger);
                    }
                    if(subgroup.NodeType == "group")
                    {
                        //Ensure child and parent information is correct
                        //add the parent id to the dropped node, then import it
                        subgroup.TriggerGroup.ParentId = tm.TriggerGroup.Id;
                        WriteGroupToDB(subgroup.TriggerGroup);

                        //add the dropped node id to the parent subgroup
                        //write the parent group to the db
                        tm.TriggerGroup.SubGroups.Add(subgroup.TriggerGroup.Id);
                        WriteGroupToDB(tm.TriggerGroup);

                        //If we have more subgroups, keep processing
                        if (subgroup.SubGroups.Count > 0)
                        { ImportTriggerGroup(subgroup); }                        
                    }
                }
            }
        }
        #endregion

        private void buttonGeneralSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonEQFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonMediaFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDataFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAddSender_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonRemoveSender_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonSaveSharing_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}