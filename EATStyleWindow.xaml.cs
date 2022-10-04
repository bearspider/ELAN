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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Runtime.InteropServices;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using Microsoft.SqlServer.Server;

namespace EQAudioTriggers
{
    public class GlobalVariables
    {
        public static string workingdirectory = @"C:\\EAT";
        public static string foldericon = "\\Images\\Oxygen-Icons.org-Oxygen-Actions-document-open-folder.ico";
        public static string triggericon = "\\Images\\Oxygen-Icons.org-Oxygen-Actions-irc-voice.ico";
        public static Regex eqRegex = new Regex(@"\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\](?<stringToMatch>.*)", RegexOptions.Compiled);
        public static Regex shareRegex = new Regex(@".*?\{HEAP:(?<GUID>.*?)\}", RegexOptions.Compiled);
        public static Regex eqspellRegex = new Regex(@"(\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\])\s((?<character>\w+)\sbegin\s(casting|singing)\s(?<spellname>.*)\.)|(\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\])\s(?<character>\w+)\s(begins\sto\s(cast|sing)\s.*\<(?<spellname>.*)\>)", RegexOptions.Compiled);
        public static string zoneMatchString = "You have entered";
        public static Regex zoneRegex = new Regex(@"(\[(?<eqtime>\w+\s\w+\s+\d+\s\d+:\d+:\d+\s\d+)\])\s(You\shave\sentered\s)(?<zonename>.*)\.");
        public static string backupDB = $"{workingdirectory}\\Backup";
    }
    public enum ShareInvitation
    {
        Nobody,
        Trusted,
        Anybody
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
    /// <summary>
    /// Convert a Boolean value to icon which will display the whether a category is DEFAULT
    /// </summary>
    /// <returns>When true, an icon will displayed which indicates the DEFAULT category</returns>
    public class DefaultCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String rString = "";
            if ((Boolean)value)
            {
                rString = "Images/Paomedia-Small-N-Flat-Sign-check.ico";
            }
            else
            {
                rString = "";
            }
            return rString;
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
            else if (monitored == null)
            {
                return Brushes.Fuchsia;
            }
            else
            {
                return null;
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

    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            Enum.TryParse((string)value, out ShareInvitation share);
            Boolean rval = share.Equals(parameter);
            return rval;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
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
        //parse out the "You have entered {The Plane of Knowledge}" and ignore "you have entered something where levitation doesn't work

        private int _totallinecount = 0;
        private string _currentzone = "None";
        private int _systemprocs = Environment.ProcessorCount;
        private string _defaultcharacter = "";
        private ObservableCollection<CharacterCollection> _characters = new ObservableCollection<CharacterCollection>();
        private ObservableCollection<string> _monitoringcharacters = new ObservableCollection<string>();
        private ObservableCollection<TriggerGroupProperty> _triggergroups = new ObservableCollection<TriggerGroupProperty>();
        private ActivatedTriggerCollection _activatedtriggers = new ActivatedTriggerCollection();
        private ActiveTriggerCollection _activetriggers = new ActiveTriggerCollection();
        private ObservableCollection<TriggerManager> _triggermanager = new ObservableCollection<TriggerManager>();
        private ObservableCollection<TriggerManager> _mergemanager = new ObservableCollection<TriggerManager>();
        private static StringCollection _log = new StringCollection();
        private readonly SynchronizationContext _syncontext;
        private ObservableCollection<EQTrigger> _triggermasterlist = new ObservableCollection<EQTrigger>();
        private string _selectedcharacter = "";
        private Settings _settings = new Settings();
        private ObservableCollection<EQTrigger> _modifiedtriggers = new ObservableCollection<EQTrigger>();
        private ParallelOptions _po = new ParallelOptions();
        private List<string> _availableThemes = new List<string>();
        private string _selectedtheme = "FluentDark";
        private ObservableCollection<OverlayText> _overlaytext = new ObservableCollection<OverlayText>();
        private ObservableCollection<OverlayTextWindow> _overlaytextwindows = new ObservableCollection<OverlayTextWindow>();
        private ObservableCollection<OverlayTimer> _overlaytimer = new ObservableCollection<OverlayTimer>();
        private ObservableCollection<OverlayTimerWindow> _overlaytimerwindows = new ObservableCollection<OverlayTimerWindow>();
        private ObservableCollection<Category> _categories = new ObservableCollection<Category>();

        //System Tray Icons
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;

        #endregion
        public EATStyleWindow()
        {
            InitializeComponent();
            _syncontext = SynchronizationContext.Current;
            LoadBase();
            LoadSettings();
            LoadThemes();
            LoadCharacters();
            LoadCategories();
            ActivateLog();
            if (_characters.Count > 0)
            {
                foreach (CharacterCollection profile in _characters)
                {
                    if (profile.CharacterProfile.Monitor)
                    {
                        StartMonitor(profile.CharacterProfile);
                    }
                    if (profile.CharacterProfile.Default)
                    {
                        _defaultcharacter = profile.CharacterProfile.Name;
                        _listviewCharacters.SelectedItem = profile;
                    }
                }
            }
            DataContext = this;
            //set datacontext for status bar
            txtblockStatus.DataContext = _totallinecount;
            txtblockZone.DataContext = _currentzone;
            LoadOverlayText();
            LoadOverlayTimer();
        }
        private void LoadCategories()
        {
            if (File.Exists($"{GlobalVariables.workingdirectory}\\categories.json"))
            {
                using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\categories.json"))
                {
                    string json = r.ReadToEnd();
                    _categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
                }
            }
            else
            {
                _categories.Add(new Category());
                WriteCategories();
            }
            categoryTabs.ItemsSource = _categories;
        }
        private void LoadThemes()
        {
            _availableThemes.Add("MaterialDark");
            _availableThemes.Add("MaterialLight");
            _availableThemes.Add("FluentLight");
            _availableThemes.Add("FluentDark");
            _availableThemes.Add("MaterialLightBlue");
            _availableThemes.Add("MaterialDarkBlue");
            _availableThemes.Add("Office2019Colorful");
            _availableThemes.Add("Office2019Black");
            _availableThemes.Add("Office2019DarkGray");
            comboVisualStyle.ItemsSource = _availableThemes;
            comboVisualStyle.DataContext = _settings;
        }
        private void LoadBase()
        {
            //Setup the system tray
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/EQAudioTriggers;component/Images/Tonev-Windows-7-Windows-7-headphone.ico")).Stream;
            MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
            MyNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);

            //Backup all of the json files
            //Else, this is a new instance.  Set the log maintenance start as today.
            if (!Directory.Exists(GlobalVariables.workingdirectory))
            {
                Directory.CreateDirectory(GlobalVariables.workingdirectory);
                Properties.Settings.Default.LastLogMaintenance = DateTime.Now;
                Properties.Settings.Default.Save();
            }
            else
            {
                if (!Directory.Exists(GlobalVariables.backupDB))
                {
                    Directory.CreateDirectory(GlobalVariables.backupDB);
                }
                string[] files = System.IO.Directory.GetFiles(GlobalVariables.workingdirectory, "*.json");
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    File.Copy(file, $"{GlobalVariables.workingdirectory}\\Backup\\{fi.Name}", true);
                }
            }
        }
        private void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (_settings.Minimize == "True")
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
            await System.Threading.Tasks.Task.Run(() =>
            {
                Console.WriteLine($"Monitoring {character.Name}");
                _monitoringcharacters.Add(character.Name);
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
                                //Only do zone monitoring for the selected character
                                if (character.Id == _selectedcharacter)
                                {
                                    if (capturedLine.Contains(GlobalVariables.zoneMatchString))
                                    {
                                        //Find out what zone we're in
                                        Match zone = GlobalVariables.zoneRegex.Match(capturedLine);
                                        string zonename = zone.Groups["zonename"].Value;
                                        //update which zone we're in on statusbar
                                        UpdateZoneName(zonename);
                                    }
                                }
                                if (capturedLine.Contains("{ELAN:"))
                                { }
                                else
                                {
                                    Stopwatch triggersearch = new Stopwatch();
                                    triggersearch.Start();
                                    Parallel.ForEach(_activetriggers.Collection, _po, (EQTrigger doc, ParallelLoopState state) =>
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
                                        if (foundmatch  && doc.ActiveCharacters.Contains(character.Id))
                                        {
                                            triggered = true;
                                            Match logmatch = GlobalVariables.eqRegex.Match(capturedLine);
                                            ActivatedTrigger activatedTrigger = new ActivatedTrigger
                                            {
                                                Character = character.Profile,
                                                MatchedText = capturedLine.Replace("\r","").Replace("\n",""),
                                                MatchTime = DateTime.Now.ToString(),
                                                Trigger = doc.Name,
                                                LogTime = logmatch.Groups["eqtime"].Value.ToString()
                                            };
                                            _activatedtriggers.Collection.Add(activatedTrigger);

                                            //Stopwatch firetrigger = new Stopwatch();
                                            //firetrigger.Start();
                                            //FireTrigger(doc, character, newstring);
                                            //firetrigger.Stop();
                                            if (doc.RadioBasicTTS && _settings.EnableSound == "true")
                                            { character.Speak(doc.BasicTTS); }

                                            //Check if there is a text overlay
                                            if (doc.UseBasicText)
                                            {
                                                ////Get Category Object for this trigger
                                                //Category category = _categories.Where<Category>(x => x.Name == doc.Category).FirstOrDefault();
                                                ////Find which overlay text window to publish on
                                                //OverlayTextWindow otw = _overlaytextwindows.Where<OverlayTextWindow>(x => x.Name == category.TextOverlay).FirstOrDefault();
                                                ////Create new overlay item
                                                //OverlayTextItem oti = new OverlayTextItem(doc, otw);
                                                ////push overlay to text collection
                                                //otw.Items.Add(oti);

                                            }
                                            //Global Option for stopping on a first match
                                            //if (stopfirstmatch)
                                            //{
                                            //    state.Break();
                                            //}
                                        }
                                    });
                                    triggersearch.Stop();
                                    Console.WriteLine($"Took {triggersearch.Elapsed.TotalSeconds} seconds to process triggers.");
                                }
                            }
                        }
                        _monitoringcharacters.Remove(character.Name);
                        Console.WriteLine("Stopped Monitoring");
                    }
                }
            });
            #endregion
        }
        private void LoadSettings()
        {
            //Load settings.json
            if (File.Exists($"{GlobalVariables.workingdirectory}\\settings.json"))
            {
                using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\settings.json"))
                {
                    string json = r.ReadToEnd();
                    _settings = JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            else
            {
                _settings.DefaultSettings();
                WriteSettings();
            }
        }
        private void SetCores(int percentage)
        {
            int totalcores = Convert.ToInt32(Math.Floor(_systemprocs * (percentage / 100.0)));
            if (totalcores < 1)
            {
                totalcores = 1;
            }
            _po.MaxDegreeOfParallelism = totalcores;
        }
        private void LoadCharacters()
        {
            //If this is the initial load, create a default character
            if (!File.Exists($"{GlobalVariables.workingdirectory}\\characters.json"))
            {
                CreateCharacter();
            }
            using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\characters.json"))
            {
                string json = r.ReadToEnd();
                _characters = JsonConvert.DeserializeObject<ObservableCollection<CharacterCollection>>(json);
            }
            LoadTriggers();
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
            //If this is the first time loading, create default files
            if (!File.Exists($"{GlobalVariables.workingdirectory}\\triggers.json"))
            {
                Trigger newtrigger = new Trigger();
                WriteTriggers();
            }
            if (!File.Exists($"{GlobalVariables.workingdirectory}\\triggergroups.json"))
            {
                TriggerGroupProperty newgroup = new TriggerGroupProperty();
                WriteTriggerGroups();
            }
            //Load Triggers into Collection     
            using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\triggers.json"))
            {
                string json = r.ReadToEnd();
                _triggermasterlist = JsonConvert.DeserializeObject<ObservableCollection<EQTrigger>>(json);
            }
            //Load Groups into Collection
            using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\triggergroups.json"))
            {
                string json = r.ReadToEnd();
                _triggergroups = JsonConvert.DeserializeObject<ObservableCollection<TriggerGroupProperty>>(json);
            }
            //initialize active trigger list
            _activetriggers.Refactor(_triggermasterlist);
            //Build Tree
            foreach (TriggerGroupProperty group in _triggergroups.Where(x => String.IsNullOrEmpty(x.ParentId)))
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
                if (group.SubGroups.Count > 0)
                {
                    LoadSubGroup(newtrigger);
                }
                if (group.Triggers.Count > 0)
                {
                    foreach (string triggerid in group.Triggers)
                    {
                        EQTrigger eqtrigger = FindTrigger(triggerid);
                        TriggerManager newmanager = new TriggerManager
                        {
                            Name = eqtrigger.Name,
                            NodeType = "trigger",
                            Icon = GlobalVariables.triggericon,
                            Trigger = eqtrigger,
                            ParentNode = newtrigger,
                            IsActive = false
                        };
                        newtrigger.SubGroups.Add(newmanager);
                    }
                }
                _triggermanager.Add(newtrigger);
                treeview.LoadOnDemandCommand = newtrigger.TreeViewOnDemandCommand;
            }

            //Test out sorting.
            CollectionViewSource groupvs = new CollectionViewSource();
            SortDescription desc = new SortDescription("Name", ListSortDirection.Ascending);
            groupvs.IsLiveFilteringRequested = true;
            groupvs.SortDescriptions.Add(desc);
            groupvs.Source = _triggermanager;
            treeview.ItemsSource = groupvs.View;

            //Assign trigger collection to tree view
            //treeview.ItemsSource = _triggermanager;
        }
        private void LoadSubGroup(TriggerManager triggermanager)
        {
            foreach (string groupid in triggermanager.TriggerGroup.SubGroups)
            {
                TriggerGroupProperty newgroup = FindGroup(groupid);
                if (newgroup != null)
                {
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
                        LoadSubGroup(newtrigger);
                    }
                    if (newgroup.Triggers.Count > 0)
                    {
                        foreach (string triggerid in newgroup.Triggers)
                        {
                            EQTrigger eqtrigger = FindTrigger(triggerid);
                            if (eqtrigger != null)
                            {
                                TriggerManager newmanager = new TriggerManager
                                {
                                    Name = eqtrigger.Name,
                                    NodeType = "trigger",
                                    Icon = GlobalVariables.triggericon,
                                    Trigger = eqtrigger,
                                    ParentNode = newtrigger,
                                    IsActive = false
                                };
                                newtrigger.SubGroups.Add(newmanager);
                            }
                        }
                    }
                    triggermanager.SubGroups.Add(newtrigger);
                }
            }
        }
        private void PurgeFromTriggers(string Id)
        {
            foreach (EQTrigger trigger in _triggermasterlist)
            {
                if (trigger.ActiveCharacters.Contains(Id))
                {
                    trigger.ActiveCharacters.Remove(Id);
                }
            }
            WriteTriggers();
        }
        private void WriteCategories()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\categories.json"))
            {
                file.Write(JsonConvert.SerializeObject(_categories, Newtonsoft.Json.Formatting.Indented));
            }
        }
        private void WriteTriggers()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\triggers.json"))
            {
                file.Write(JsonConvert.SerializeObject(_triggermasterlist, Newtonsoft.Json.Formatting.Indented));
            }
        }
        private void WriteTriggerGroups()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\triggergroups.json"))
            {
                file.Write(JsonConvert.SerializeObject(_triggergroups, Newtonsoft.Json.Formatting.Indented));
            }
        }
        private EQTrigger FindTrigger(string id)
        {
            foreach (EQTrigger eqtrigger in _triggermasterlist)
            {
                if (eqtrigger.Id == id)
                {
                    return eqtrigger;
                }
            }
            return null;
        }
        private TriggerGroupProperty FindGroup(string id)
        {
            foreach (TriggerGroupProperty tgp in _triggergroups)
            {
                if (tgp.Id == id)
                {
                    return tgp;
                }
            }
            return null;
        }
        #region Characters
        private void CreateCharacter()
        {
            CharacterEdit chareditor = new CharacterEdit();
            chareditor.SetTheme(_selectedtheme);
            Boolean rval = (bool)chareditor.ShowDialog();
            if (chareditor.ReturnChar != null)
            {
                _characters.Add(new CharacterCollection { Name = chareditor.ReturnChar.Name, CharacterProfile = chareditor.ReturnChar });
                //If monitor at startup is checked, make sure that Monitoring is also checked
                if (chareditor.ReturnChar.Monitor)
                {
                    chareditor.ReturnChar.Monitoring = true;
                    StartMonitor(chareditor.ReturnChar);
                }
                else
                {
                    chareditor.ReturnChar.Monitoring = false;
                }
                //If we don't have any characters, set this one as the default
                if (_characters.Count == 0)
                {
                    chareditor.ReturnChar.Default = true;
                }
                //We need to make sure that there is only one default character
                if (chareditor.ReturnChar.Default)
                {
                    _defaultcharacter = chareditor.ReturnChar.Name;
                    foreach (CharacterCollection cc in _characters)
                    {
                        if (cc.CharacterProfile.Default && cc.CharacterProfile.Name != _defaultcharacter)
                        {
                            cc.CharacterProfile.Default = false;
                        }
                    }
                }

                WriteCharacters();
            }
        }
        private void EditCharacter(CharacterCollection character)
        {
            try
            {
                CharacterEdit chareditor = new CharacterEdit(character.CharacterProfile);
                chareditor.SetTheme(_selectedtheme);
                Boolean rval = (Boolean)chareditor.ShowDialog();
                //Update the collection name if it changed
                character.Name = character.CharacterProfile.Name;
                //If monitor at startup is checked, make sure that Monitoring is also checked
                if (character.CharacterProfile.Monitor)
                {
                    if (!_monitoringcharacters.Contains(character.CharacterProfile.Name))
                    {
                        StartMonitor(character.CharacterProfile);
                    }
                    character.CharacterProfile.Monitoring = true;
                }
                else
                {
                    character.CharacterProfile.Monitoring = false;
                }
                //We need to make sure that there is only one default character
                if (character.CharacterProfile.Default)
                {
                    _defaultcharacter = character.CharacterProfile.Name;
                    foreach (CharacterCollection cc in _characters)
                    {
                        if (cc.CharacterProfile.Default && cc.CharacterProfile.Name != _defaultcharacter)
                        {
                            cc.CharacterProfile.Default = false;
                        }
                    }
                }
                WriteCharacters();
            }
            catch (Exception ex)
            {
                MessageBoxResult mbox = Xceed.Wpf.Toolkit.MessageBox.Show("Character Not Selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteCharacter(CharacterCollection character)
        {
            //If we're monitoring the character, stop the monitoring before we delete it
            character.CharacterProfile.Monitoring = false;
            _characters.Remove(character);
            WriteCharacters();
            PurgeFromTriggers(character.CharacterProfile.Id);
        }
        private void WriteCharacters()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\characters.json"))
            {
                file.Write(JsonConvert.SerializeObject(_characters, Newtonsoft.Json.Formatting.Indented));
            }
        }
        #endregion

        #region Form Functions
        private void UpdateLineCount(int value)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _syncontext.Post(new SendOrPostCallback(o =>
            {
                _totallinecount += (int)o;
                txtblockStatus.DataContext = _totallinecount;
            }), value);
            stopwatch.Stop();
            Console.WriteLine($"Line Update Took: {stopwatch.Elapsed.TotalSeconds} Seconds");
        }

        private void UpdateZoneName(string zonename)
        {
            _syncontext.Post(new SendOrPostCallback(o =>
            {
                _currentzone = (string)o;
                txtblockZone.DataContext = _currentzone;
            }), zonename);
        }
        private void MainRibbon_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
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
            RibbonTab tab = (RibbonTab)(sender as Ribbon).SelectedItem;
            if(tab.Caption == "Categories")
            {
                //dockCategory.Visibility = Visibility.Visible;
            }            
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
            EditCharacter((CharacterCollection)_listviewCharacters.SelectedItem);
        }
        private void removeUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteCharacter((CharacterCollection)_listviewCharacters.SelectedItem);
        }
        private void sliderMaster_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelMaster.Content = sliderMaster.Value;
            _settings.MasterVolume = Convert.ToString(sliderMaster.Value);
        }
        private void sliderCores_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            labelCores.Content = sliderCores.Value;
            _settings.CorePercentage = Convert.ToString(sliderCores.Value);
            SetCores(Convert.ToInt32(Math.Floor(sliderCores.Value)));
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
                    treeview.ContextMenu = treeview.Resources["groupContext"] as System.Windows.Controls.ContextMenu;
                }
                if (selecteditem.NodeType.Equals("trigger"))
                {
                    menuaddtoselectedgroup.IsEnabled = false;
                    editTriggerGroup.IsEnabled = false;
                    removeTriggerGroup.IsEnabled = false;
                    addTrigger.IsEnabled = false;
                    editTrigger.IsEnabled = true;
                    removeTrigger.IsEnabled = true;
                    treeview.ContextMenu = treeview.Resources["triggerContext"] as System.Windows.Controls.ContextMenu;
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
        private void CheckboxChecked(TriggerManager target)
        {
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
        private void CheckboxUnchecked(TriggerManager target)
        {
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
        private void _treeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsMouseOver)
            {
                TriggerManager target = (((e.Source as CheckBox).DataContext as TreeViewNode).Content as TriggerManager);
                //if Trigger, activate it for the selected character
                CheckboxChecked(target);
                //write the modified triggers
                WriteTriggers();
            }
        }
        private void _treeCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsMouseOver)
            {
                TriggerManager target = (((e.Source as CheckBox).DataContext as TreeViewNode).Content as TriggerManager);
                //if Trigger, deactivate it for the selected character
                CheckboxUnchecked(target);
                //write the modified triggers
                WriteTriggers();
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
                    //If we're dragging/dropping from the same tree handle it differently
                    string sourcetree = (e.DragSource as SfTreeView).Name;
                    string desttree = (sender as SfTreeView).Name;
                    //If we're dragging into the same tree we only need to change the root node to point to it's new parent
                    if (sourcetree == desttree)
                    {
                        Console.WriteLine("Dragging into the same tree");
                        //if a trigger, delete trigger id from old parent node
                        // add trigger id to new parent node and update groupid on trigger
                        // write old parent, new parent, and trigger to db
                        TreeViewNode rootnode = e.DraggingNodes[0];
                        TriggerManager rootmanager = (TriggerManager)rootnode.Content;
                        TriggerManager parentnode = rootmanager.ParentNode;

                        if (rootmanager.NodeType == "trigger")
                        {
                            //Remove the trigger from the old parent node
                            parentnode.TriggerGroup.Triggers.Remove(rootmanager.Trigger.Id);
                            //add the trigger to the Triggers of parent and the id to the trigger group subgroup                            
                            desttm.TriggerGroup.Triggers.Add(rootmanager.Trigger.Id);
                            //add the new parent group id to the trigger group id property
                            rootmanager.Trigger.GroupId = desttm.TriggerGroup.Id;
                            //Change the parent node on the trigger to its new parent
                            rootmanager.ParentNode = desttm;
                        }

                        //if a group, delete id out of parent sub group
                        // add group id to new sub group
                        // write old parent group, new parent group, and this group to db
                        if (rootmanager.NodeType == "group")
                        {
                            desttm.TriggerGroup.SubGroups.Add(rootmanager.TriggerGroup.Id);
                            rootmanager.TriggerGroup.ParentId = desttm.TriggerGroup.Id;
                            rootmanager.ParentNode = desttm;
                            //if this is a root group, we don't need to remove the parent node
                            //we need to remove it from the _triggermanager
                            if (parentnode != null)
                            {
                                parentnode.TriggerGroup.SubGroups.Remove(rootmanager.TriggerGroup.Id);
                            }
                            else
                            {
                                _triggermanager.Remove(parentnode);
                            }
                        }
                    }
                    else
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
                                    //remove ourselves from the merge tree to avoid visual issues
                                    if (droppednode.ParentNode != null)
                                    { droppednode.ParentNode.SubGroups.Remove(droppednode); }

                                    //Add the trigger ID to the parent node Triggers
                                    //write the parent node to the DB
                                    desttm.TriggerGroup.Triggers.Add(droppednode.Trigger.Id);
                                    //Add the Group Id to the trigger
                                    //write the trigger to the DB
                                    droppednode.Trigger.GroupId = desttm.TriggerGroup.Id;
                                    _triggermasterlist.Add(droppednode.Trigger);
                                }
                                else if (droppednode.NodeType == "group")
                                {
                                    //remove ourselves from the old parent tree, it causes display issues
                                    if (droppednode.ParentNode != null)
                                    { droppednode.ParentNode.SubGroups.Remove(droppednode); }
                                    //add the parent id to the dropped node, then import it
                                    droppednode.TriggerGroup.ParentId = desttm.TriggerGroup.Id;

                                    //add the dropped node id to the parent subgroup
                                    //write the parent group to the db
                                    desttm.TriggerGroup.SubGroups.Add(droppednode.TriggerGroup.Id);

                                    _triggergroups.Add(droppednode.TriggerGroup);
                                    //Call a recursive function that goes through the groups and subgroups to pull everything over
                                    //we'll check if somebody imports an empty group, people do dumb stuff
                                    if (droppednode.SubGroups.Count > 0)
                                    {
                                        ImportTriggerGroup(droppednode);
                                    }
                                }
                            }
                        }
                    }
                }
                WriteTriggerGroups();
                WriteTriggers();
            }
        }
        private void treeview_ItemDropping(object sender, TreeViewItemDroppingEventArgs e)
        {
            //Restrict the dropping on certain nodes
            string sourcetree = (e.DragSource as SfTreeView).Name;
            string desttree = (sender as SfTreeView).Name;
            if (e.TargetNode != null)
            {
                try
                {
                    ObservableCollection<TreeViewNode> dragsource = e.DraggingNodes;
                    if (dragsource != null && dragsource.Count > 0)
                    {
                        TriggerManager droplocation = (e.TargetNode).Content as TriggerManager;
                        if (droplocation.NodeType == "group")
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
                else
                {
                    DisableTrigger(tm);
                }
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
                newtrigger.CreateRootTriggerGroup(_selectedtheme);
                _triggermanager.Add(newtrigger);
                //resort collection
                _triggermanager = new ObservableCollection<TriggerManager>(_triggermanager.OrderBy(i => i.Name));
                treeview.ItemsSource = _triggermanager;
                _triggergroups.Add(newtrigger.TriggerGroup);
            }
            else
            {
                TriggerManager newmanager = selecteditem.ParentNode.AddTriggerGroup(_selectedtheme);
                if (newmanager != null)
                {
                    _triggergroups.Add(newmanager.TriggerGroup);
                }
            }
            WriteTriggerGroups();
            WriteTriggers();
        }
        private void AddToSelected_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager newmanager = ((TriggerManager)treeview.SelectedItem).AddTriggerGroup(_selectedtheme);
            if (newmanager != null)
            {
                _triggergroups.Add(newmanager.TriggerGroup);
                WriteTriggerGroups();
                WriteTriggers();
            }
        }
        private void editTriggerGroup_Click(object sender, RoutedEventArgs e)
        {
            EditTriggerGroup(((TriggerManager)treeview.SelectedItem));
        }
        private void removeTriggerGroup_Click(object sender, RoutedEventArgs e)
        {
            RemoveTriggerGroup((TriggerManager)treeview.SelectedItem);
        }
        private void EditTriggerGroup(TriggerManager tm)
        {
            tm.EditTriggerGroup(_selectedtheme);
            WriteTriggerGroups();
        }
        private void RemoveTriggerGroup(TriggerManager tm)
        {
            //if we have sub groups, delete those too
            foreach (TriggerManager subgroup in tm.SubGroups.ToList<TriggerManager>())
            {
                if (subgroup.NodeType == "group")
                {
                    RemoveTriggerGroup(subgroup);
                }
                if (subgroup.NodeType == "trigger")
                {
                    subgroup.RemoveTrigger();
                    _triggermasterlist.Remove(subgroup.Trigger);
                }
            }
            //Once we've cleared out all the children, remove ourself from the parent node.
            if (!tm.IsRootNode)
            {
                tm.ParentNode.SubGroups.Remove(tm);
                tm.ParentNode.TriggerGroup.SubGroups.Remove(tm.TriggerGroup.Id);
                //Update the parent node in the DB
            }
            if (tm.IsRootNode)
            {
                _triggermanager.Remove(tm);
            }
            _triggergroups.Remove(tm.TriggerGroup);
            WriteTriggers();
            WriteTriggerGroups();
        }
        #endregion

        #region Trigger Clicks
        private void addTrigger_Click(object sender, RoutedEventArgs e)
        {
            EQTrigger newtrigger = ((TriggerManager)treeview.SelectedItem).AddTrigger(_characters, _selectedtheme);
            if (newtrigger != null)
            {
                _triggermasterlist.Add(newtrigger);
            }
            WriteTriggers();
            WriteTriggerGroups();
        }
        private void editTrigger_Click(object sender, RoutedEventArgs e)
        {
            ((TriggerManager)treeview.SelectedItem).EditTrigger(_characters, _selectedtheme);
            WriteTriggers();
            WriteTriggerGroups();
        }
        private void removeTrigger_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            selecteditem.RemoveTrigger();
            _triggermasterlist.Remove(selecteditem.Trigger);
            WriteTriggers();
            WriteTriggerGroups();
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
            EditCharacter((CharacterCollection)_listviewCharacters.SelectedItem);
        }
        private void MenuItemCharDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteCharacter((CharacterCollection)_listviewCharacters.SelectedItem);
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

        #region Trigger
        private void DisableTrigger(TriggerManager tm)
        {
            if (_listviewCharacters.SelectedItem != null)
            {
                string character = ((CharacterCollection)(_listviewCharacters.SelectedItem)).CharacterProfile.Id;
                //check if character is already in the list
                //string found = tm.Trigger.ActiveCharacters.Single<string>(x => x.Contains(character));
                bool found = tm.Trigger.ActiveCharacters.Contains(character);
                if (found)
                {
                    tm.Trigger.RemoveCharacter(character);
                }
            }
        }
        private void EnableTrigger(TriggerManager tm)
        {
            if (_listviewCharacters.SelectedItem != null)
            {
                string character = ((CharacterCollection)(_listviewCharacters.SelectedItem)).CharacterProfile.Id;
                bool found = tm.Trigger.ActiveCharacters.Contains(character);
                if (!found)
                {
                    tm.Trigger.AddCharacter(character);
                }
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
            if (_mergemanager.Count > 0)
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
                Id = Guid.NewGuid().ToString(),
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
            string rval = Guid.NewGuid().ToString();
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
                newtrigger.RadioBasicTTS = true;
                newtrigger.BasicTTS = (String)jsontoken["TextToVoiceText"];
            }
            if ((bool)jsontoken["PlayMediaFile"])
            {
                newtrigger.RadioBasicPlay = true;
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
            if (tm.SubGroups.Count > 0)
            {
                foreach (TriggerManager subgroup in tm.SubGroups)
                {
                    if (subgroup.NodeType == "trigger")
                    {
                        //Ensure the child and parent information is correct
                        //Add the trigger ID to the parent node Triggers
                        //write the parent node to the DB
                        tm.TriggerGroup.Triggers.Add(subgroup.Trigger.Id);

                        //Add the Group Id to the trigger
                        //write the trigger to the DB
                        subgroup.Trigger.GroupId = tm.TriggerGroup.Id;
                        _triggermasterlist.Add(subgroup.Trigger);
                    }
                    if (subgroup.NodeType == "group")
                    {
                        //Ensure child and parent information is correct
                        //add the parent id to the dropped node, then import it
                        subgroup.TriggerGroup.ParentId = tm.TriggerGroup.Id;

                        //add the dropped node id to the parent subgroup
                        //write the parent group to the db
                        tm.TriggerGroup.SubGroups.Add(subgroup.TriggerGroup.Id);
                        _triggergroups.Add(subgroup.TriggerGroup);

                        //If we have more subgroups, keep processing
                        if (subgroup.SubGroups.Count > 0)
                        { ImportTriggerGroup(subgroup); }
                    }
                }
            }
        }
        #endregion

        #region Settings
        private void WriteSettings()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\settings.json"))
            {
                file.Write(JsonConvert.SerializeObject(_settings, Newtonsoft.Json.Formatting.Indented));
            }
        }
        private void buttonGeneralSave_Click(object sender, RoutedEventArgs e)
        {
            WriteSettings();
            _ribbon.HideBackStage();
        }

        private void buttonEQFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _settings.EQFolder = dialog.FileName;
            }
        }

        private void buttonMediaFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _settings.ImportedMediaFolder = dialog.FileName;
            }
        }

        private void buttonDataFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _settings.DataFolder = dialog.FileName;
            }
        }

        private void buttonAddSender_Click(object sender, RoutedEventArgs e)
        {
            //Check if the name already exists
            bool hasname = _settings.TrustedSenderList.Contains(txtboxSenderName.Text);
            if (!hasname)
            {
                _settings.TrustedSenderList.Add(txtboxSenderName.Text);
            }
            txtboxSenderName.Text = "";
        }

        private void buttonRemoveSender_Click(object sender, RoutedEventArgs e)
        {
            _settings.TrustedSenderList.Remove((string)listviewSenders.SelectedValue);
        }

        private void buttonSaveSharing_Click(object sender, RoutedEventArgs e)
        {

        }
        private void comboVisualStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedtheme = (string)comboVisualStyle.SelectedValue;
        }
        #endregion

        #region Context Menus

        private void ContextMenuGroupAdd_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager newmanager = ((TriggerManager)treeview.SelectedItem).AddTriggerGroup(_selectedtheme);
            if (newmanager != null)
            {
                _triggergroups.Add(newmanager.TriggerGroup);
            }
            WriteTriggerGroups();
        }

        private void ContextMenuGroupRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveTriggerGroup((TriggerManager)treeview.SelectedItem);
        }

        private void ContextMenuGroupEdit_Click(object sender, RoutedEventArgs e)
        {
            EditTriggerGroup(((TriggerManager)treeview.SelectedItem));
        }
        private void ContextMenuGroupMove_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager nodeToMove = ((TriggerManager)treeview.SelectedItem);
            //Remove node from parents list for json file
            nodeToMove.ParentNode.TriggerGroup.SubGroups.Remove(nodeToMove.TriggerGroup.Id);
            //Remove node from parents for collection
            nodeToMove.ParentNode.SubGroups.Remove(nodeToMove);
            //Set parent to null
            nodeToMove.TriggerGroup.ParentId = null;
            //set parent node to null
            nodeToMove.ParentNode = null;
            _triggermanager.Add(nodeToMove);
            WriteTriggerGroups();
        }

        private void ContextMenuTriggerAdd_Click(object sender, RoutedEventArgs e)
        {
            EQTrigger newtrigger = ((TriggerManager)treeview.SelectedItem).AddTrigger(_characters, _selectedtheme);
            if (newtrigger != null)
            {
                _triggermasterlist.Add(newtrigger);
            }
            WriteTriggers();
            WriteTriggerGroups();
        }

        private void ContextMenuTriggerEdit_Click(object sender, RoutedEventArgs e)
        {
            ((TriggerManager)treeview.SelectedItem).EditTrigger(_characters, _selectedtheme);
            WriteTriggers();
            WriteTriggerGroups();
        }

        private void ContextMenuTriggerRemove_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            selecteditem.RemoveTrigger();
            _triggermasterlist.Remove(selecteditem.Trigger);
            WriteTriggers();
            WriteTriggerGroups();
        }

        private void ContextMenuTriggerDisable_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            selecteditem.IsActive = false;
            WriteTriggers();
        }
        private void ContextMenuTriggerEnable_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            selecteditem.IsActive = true;
            WriteTriggers();
        }
        private void ContextMenuGroupShare_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContextMenuTriggerShare_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContextMenuTriggerDup_Click(object sender, RoutedEventArgs e)
        {
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            TriggerManager parentnode = selecteditem.ParentNode;

            //copy the trigger into a new instance
            //Add the trigger to the parent node
            EQTrigger newtrigger = new EQTrigger(selecteditem.Trigger);
            newtrigger.Name += "-Copy";
            Boolean added = parentnode.AddTrigger(_characters, newtrigger, _selectedtheme);
            if (added)
            {
                _triggermasterlist.Add(newtrigger);
                //write the trigger and group to disk
                WriteTriggers();
                WriteTriggerGroups();
            }

        }
        #endregion

        #region Backstage Functions
        private void BackstageAdvanced_GotFocus(object sender, RoutedEventArgs e)
        {
            sliderCores.DataContext = _settings;
        }

        private void BackstageGeneral_GotFocus(object sender, RoutedEventArgs e)
        {
            chkboxEnableSound.DataContext = _settings;
            chkboxEnableText.DataContext = _settings;
            chkboxEnableTimers.DataContext = _settings;
            chkboxStopFirstMatch.DataContext = _settings;
            chkboxSystemTray.DataContext = _settings;
            chkboxDisplayMatches.DataContext = _settings;
            chkboxLogMatches.DataContext = _settings;
            textboxMatchLog.DataContext = _settings;
            textboxClipboard.DataContext = _settings;
            textboxEQFolder.DataContext = _settings;
            textboxImportedMedia.DataContext = _settings;
            textboxDataFolder.DataContext = _settings;
            textboxMaxLogEntries.DataContext = _settings;
            textboxLogArchiveFolder.DataContext = _settings;
            chkboxAutoArchive.DataContext = _settings;
            chkboxCompressArchive.DataContext = _settings;
            chkboxDeleteArchive.DataContext = _settings;
            textboxDeleteArchive.DataContext = _settings;
            sliderMaster.DataContext = _settings;
            comboboxArchiveMethod.DataContext = _settings;
            comboboxArchiveSchedule.DataContext = _settings;
            listviewSenders.DataContext = _settings;
            chkboxSharing.DataContext = _settings;
            radioAcceptNobody.DataContext = _settings;
            radioAcceptAnybody.DataContext = _settings;
            radioAcceptTrusted.DataContext = _settings;
            radioMergeAnbody.DataContext = _settings;
            radioMergeNobody.DataContext = _settings;
            radioMergeTrusted.DataContext = _settings;
            textboxSharingURL.DataContext = _settings;
        }

        private void buttonLogArchiveFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                _settings.LogArchiveFolder = dialog.FileName;
            }
        }
        private void listviewSenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listviewSenders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtboxSenderName.Text = (string)listviewSenders.SelectedValue;
            _settings.TrustedSenderList.Remove(txtboxSenderName.Text);
        }
        #endregion

        #region Overlays
        private void buttonOverlayText_Click(object sender, RoutedEventArgs e)
        {
            OverlayTextEditor ote = new OverlayTextEditor();
            Boolean rval = (bool)ote.ShowDialog();
            if (rval)
            {
                _overlaytext.Add(ote.Overlay);
                SaveOverlayText();
                //open window
                OverlayTextWindow otw = new OverlayTextWindow(ote.Overlay);
                otw.ShowInTaskbar = false;
                _overlaytextwindows.Add(otw);
                otw.Show();
            }
        }
        private void buttonOverlayTimer_Click(object sender, RoutedEventArgs e)
        {
            OverlayTimerEditor ote = new OverlayTimerEditor();
            Boolean rval = (bool)ote.ShowDialog();
            if (rval)
            {
                _overlaytimer.Add(ote.OT);
                SaveOverlayTimer();
                //open window
                OverlayTimerWindow otw = new OverlayTimerWindow(ote.OT);
                otw.ShowInTaskbar = false;
                _overlaytimerwindows.Add(otw);
                otw.Show();
            }
        }
        private void LoadOverlayText()
        {
            //Load overlaytext.json
            if (File.Exists($"{GlobalVariables.workingdirectory}\\overlaytext.json"))
            {
                using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\overlaytext.json"))
                {
                    string json = r.ReadToEnd();
                    _overlaytext = JsonConvert.DeserializeObject<ObservableCollection<OverlayText>>(json);
                }
            }
            ribbonOverlays.ItemsSource = _overlaytext;
            //Open windows
            foreach (OverlayText overlay in _overlaytext)
            {
                OverlayTextWindow newWindow = new OverlayTextWindow(overlay);
                newWindow.ShowInTaskbar = false;
                _overlaytextwindows.Add(newWindow);
                newWindow.Show();
                UpdateText(newWindow, new OverlayTextItem());
            }
        }
        private void LoadOverlayTimer()
        {
            //Load overlaytimer.json
            if (File.Exists($"{GlobalVariables.workingdirectory}\\overlaytimer.json"))
            {
                using (StreamReader r = new StreamReader($"{GlobalVariables.workingdirectory}\\overlaytimer.json"))
                {
                    string json = r.ReadToEnd();
                    _overlaytimer = JsonConvert.DeserializeObject<ObservableCollection<OverlayTimer>>(json);
                }
            }
            ribbonOverlayTimers.ItemsSource = _overlaytimer;
            //Open windows
            foreach (OverlayTimer overlay in _overlaytimer)
            {
                OverlayTimerWindow newWindow = new OverlayTimerWindow(overlay);
                newWindow.ShowInTaskbar = false;
                _overlaytimerwindows.Add(newWindow);
                newWindow.Show();
                //UpdateText(newWindow, new OverlayTextItem());
            }
        }
        private void UpdateText(OverlayTextWindow otw, OverlayTextItem oti)
        {
            _syncontext.Post(new SendOrPostCallback(o =>
            {
                otw.AddItem((OverlayTextItem)o);
            }), oti);
        }
        private void SaveOverlayText()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\overlaytext.json"))
            {
                file.Write(JsonConvert.SerializeObject(_overlaytext, Newtonsoft.Json.Formatting.Indented));
            }
        }
        private void SaveOverlayTimer()
        {
            using (StreamWriter file = File.CreateText($"{GlobalVariables.workingdirectory}\\overlaytimer.json"))
            {
                file.Write(JsonConvert.SerializeObject(_overlaytimer, Newtonsoft.Json.Formatting.Indented));
            }
        }
        private void DropDownMenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            DropDownMenuItem overlayitem = sender as DropDownMenuItem;
            //find the overlay we're editing
            OverlayTextWindow otw = _overlaytextwindows.Where<OverlayTextWindow>(x => x.WindowProperties.Name == ((OverlayText)overlayitem.DataContext).Name).FirstOrDefault();
            OverlayTextEditor ote = new OverlayTextEditor(otw.WindowProperties);
            Boolean rval = (bool)ote.ShowDialog();
            if (rval)
            {
                SaveOverlayText();
            }
        }

        private void DropDownMenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            //remove overlaytext from collection and close window
            DropDownMenuItem overlayitem = sender as DropDownMenuItem;
            string windowname = ((OverlayText)overlayitem.DataContext).Name;
            OverlayTextWindow toremove = _overlaytextwindows.Where<OverlayTextWindow>(x => x.WindowProperties.Name == windowname).FirstOrDefault();
            toremove.Close();
            _overlaytextwindows.Remove(toremove);
            _overlaytext.Remove((OverlayText)overlayitem.DataContext);
            //write collection to file
            SaveOverlayText();
        }

        private void DropDownMenuTimerEdit_Click(object sender, RoutedEventArgs e)
        {
            DropDownMenuItem overlayitem = sender as DropDownMenuItem;
            //find the overlay we're editing
            OverlayTimerWindow otw = _overlaytimerwindows.Where<OverlayTimerWindow>(x => x.WindowProperties.Name == ((OverlayTimer)overlayitem.DataContext).Name).FirstOrDefault();
            OverlayTimerEditor ote = new OverlayTimerEditor(otw.WindowProperties);
            Boolean rval = (bool)ote.ShowDialog();
            if (rval)
            {
                SaveOverlayTimer();
            }
        }

        private void DropDownMenuTimerDelete_Click(object sender, RoutedEventArgs e)
        {
            //remove overlaytimer from collection and close window
            DropDownMenuItem overlayitem = sender as DropDownMenuItem;
            string windowname = ((OverlayTimer)overlayitem.DataContext).Name;
            OverlayTimerWindow toremove = _overlaytimerwindows.Where<OverlayTimerWindow>(x => x.WindowProperties.Name == windowname).FirstOrDefault();
            toremove.Close();
            _overlaytimerwindows.Remove(toremove);
            _overlaytimer.Remove((OverlayTimer)overlayitem.DataContext);
            //write collection to file
            SaveOverlayTimer();
        }

        #endregion

        private void ribbonCategoryAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ribbonCategoryRemove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ribbonCategoryDefault_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}