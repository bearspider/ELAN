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
            if(monitored)
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
            //var type = (string)value;
            //Visibility.Hidden;
            return Visibility.Visible;
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

        private int _totallinecount = 0;
        private CharacterCollection _characters = new CharacterCollection();
        private ActivatedTriggerCollection _activatedtriggers = new ActivatedTriggerCollection();
        private ObservableCollection<EQTrigger> _activetriggers = new ObservableCollection<EQTrigger>();
        private ObservableCollection<TriggerManager> _triggermanager = new ObservableCollection<TriggerManager>();
        private static StringCollection _log = new StringCollection();
        private readonly SynchronizationContext syncontext;
        private ObservableCollection<EQTrigger> _triggermasterlist = new ObservableCollection<EQTrigger>();

        #endregion
        public EATStyleWindow()
        {
            InitializeComponent();
            syncontext = SynchronizationContext.Current;
            //po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            LoadSettings();
            LoadCharacter();
            LoadTriggers();                        
            ActivateLog();
            foreach(Character character in _characters)
            {
                if(character.Monitor)
                {
                    StartMonitor(character);
                }
            }

            //set datacontext for status bar
            txtblockStatus.DataContext = _totallinecount;
        }
        private void RefactorActiveTriggers()
        {
            _activetriggers = new ObservableCollection<EQTrigger>(_activetriggers.Where(i => i.ActiveCharacters.Count > 0));
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
                                    Parallel.ForEach(_activetriggers, (EQTrigger doc, ParallelLoopState state) =>
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
        }
        private void LoadCharacter()
        {
            var charfiles = Directory.EnumerateFiles($"{GlobalVariables.workingdirectory}/Characters","*.json");
            try
            {
                foreach(string curfile in charfiles)
                {
                    using(StreamReader r = new StreamReader(curfile))
                    {
                        string json = r.ReadToEnd();
                        Character newchar = JsonConvert.DeserializeObject<Character>(json);
                        _characters.Add(newchar);
                    }
                }
            }
            catch(Exception e)
            {
                //no characters to load
            }
            CollectionViewSource charactervs = new CollectionViewSource();
            SortDescription desc = new SortDescription("Name", ListSortDirection.Ascending);
            charactervs.IsLiveFilteringRequested = true;
            charactervs.SortDescriptions.Add(desc);
            charactervs.Source = _characters;
            _listviewCharacters.ItemsSource = charactervs.View;
            if(_characters.Count > 0)
            {
                _listviewCharacters.SelectedIndex = 0;
            }
        }
        private void ActivateLog()
        {
            activatedDatagrid.ItemsSource = _activatedtriggers.Collection;
        }
        private void LoadTriggers()
        {
            treeview.QueryNodeSize += SfTreeView_QueryNodeSize;
            treeview.NodeChecked += SfTreeView_NodeChecked;
            treeview.CheckBoxMode = CheckBoxMode.Recursive;

            //Load Triggers into Collection
            String StartPath = $"{EQAudioTriggers.GlobalVariables.workingdirectory}\\TriggerGroups";
            DirectoryInfo di = new DirectoryInfo(StartPath);
            foreach(DirectoryInfo newdir in di.GetDirectories())
            {
                TriggerManager newtrigger = new TriggerManager { FullPath = newdir.FullName, Name = newdir.Name, NodeType = "group", Icon = GlobalVariables.foldericon, IsRootNode = true };
                treeview.LoadOnDemandCommand = newtrigger.TreeViewOnDemandCommand;
                _triggermanager.Add(newtrigger);
                WalkDirectoryTree(newtrigger);
            }
            //Assign trigger collection to tree view
            treeview.ItemsSource = _triggermanager;
        }
        private void WalkDirectoryTree(TriggerManager root)
        {
            FileInfo[] files = null;
            DirectoryInfo rootDir = new DirectoryInfo(root.FullPath);

            // First, process all the files directly under this folder
            //This is Triggers
            try
            {
                files = rootDir.GetFiles("*.json");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                _log.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                root.HasChildNodes = true;
                foreach (FileInfo fi in files)
                {
                    //Parse Trigger Group Info 'properties.json'
                    if(fi.Name.Contains("properties.json"))
                    {
                        //Set the trigger group properties
                        Console.WriteLine("Found Properties File");
                        using (StreamReader r = new StreamReader(fi.FullName))
                        {
                            string json = r.ReadToEnd();
                            TriggerGroupProperty tgproperty = JsonConvert.DeserializeObject<TriggerGroupProperty>(json);
                            tgproperty.Name = root.Name;
                            root.TriggerGroup = tgproperty;
                            root.Comments = tgproperty.Comments;
                            root.DefaultEnable = tgproperty.DefaultEnabled;
                        }
                    }
                    else
                    {

                        using (StreamReader r = new StreamReader(fi.FullName))
                        {
                            string json = r.ReadToEnd();
                            EQTrigger newtrigger = JsonConvert.DeserializeObject<EQTrigger>(json);
                            newtrigger.Path = fi.FullName;
                            _triggermasterlist.Add(newtrigger);
                            //Cross check the active character list of the trigger to current characters with monitoring active
                            var activechars = _characters.Where(i => i.Monitoring == true && newtrigger.ActiveCharacters.Contains(i.Name));
                            if(activechars.Count() > 0)
                            {
                                _activetriggers.Add(newtrigger);
                            }

                            // Resursive call for each subdirectory.
                            TriggerManager newmanager = new TriggerManager
                            {
                                FullPath = fi.FullName,
                                Name = newtrigger.Name,
                                NodeType = "trigger",
                                Icon = GlobalVariables.triggericon,
                                Trigger = newtrigger,
                                ParentNode = root
                            };
                            root.SubGroups.Add(newmanager);
                        }
                        Console.WriteLine(fi.FullName);
                    }
                }

                //This is Trigger Groups
                // Now find all the subdirectories under this directory.
                foreach (DirectoryInfo dirInfo in rootDir.GetDirectories())
                {
                    // Resursive call for each subdirectory.
                    TriggerManager newmanager = new TriggerManager
                    {
                        Name = dirInfo.Name,
                        FullPath = dirInfo.FullName,
                        NodeType = "group",
                        Icon = GlobalVariables.foldericon,
                        ParentNode = root
                    };
                    root.SubGroups.Add(newmanager);
                    WalkDirectoryTree(newmanager);
                }
            }
        }
        private void PurgeFromTriggers(string name)
        {
            foreach(EQTrigger trigger in _triggermasterlist)
            {
                if(trigger.ActiveCharacters.Contains(name))
                {
                    trigger.ActiveCharacters.Remove(name);
                    using (StreamWriter file = File.CreateText(trigger.Path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(file, trigger);
                    }
                }
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

        private void dockingmanager_LayoutUpdated(object sender, EventArgs e)
        {

        }

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
            Character toadd = _characters.CreateCharacter();
            if (toadd != null)
            {
                _characters.Add(toadd);
            }
        }
        private void editUser_Click(object sender, RoutedEventArgs e)
        {
            ((Character)_listviewCharacters.SelectedValue).EditCharacter();
        }
        private void removeUser_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Character)_listviewCharacters.SelectedValue).Name;
            ((Character)_listviewCharacters.SelectedValue).Delete();
            _characters.Remove((Character)_listviewCharacters.SelectedValue);
            PurgeFromTriggers(name);
        }
        #endregion

        #region TreeView
        private void SfTreeView_NodeChecked(object sender, NodeCheckedEventArgs e)
        {
            TreeViewNode tvn = e.Node;
            TriggerManager tm = (TriggerManager)tvn.Content;
            string character = ((Character)(_listviewCharacters.SelectedItem)).Name;
            if (tm.NodeType == "trigger")
            {
                if ((bool)tvn.IsChecked)
                {
                    Console.WriteLine($"Checked Trigger{tm.Name}");
                    try
                    {
                        //check if character is already in the list
                        string found = tm.Trigger.ActiveCharacters.Single<string>(x => x.Contains(character));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Character not in list");
                        //If checked, Add character to trigger
                        tm.Trigger.AddCharacter(character);

                        //write updates to trigger file
                        //open file stream
                        using (StreamWriter file = File.CreateText(tm.Trigger.Path))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            //serialize object directly into file stream
                            serializer.Serialize(file, tm.Trigger);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Unchecked Trigger{tm.Name}");
                    try
                    {
                        //check if character is already in the list
                        string found = tm.Trigger.ActiveCharacters.Single<string>(x => x.Contains(character));
                        if (found == character)
                        {
                            tm.Trigger.RemoveCharacter(character);

                            //write updates to trigger file
                            //open file stream
                            using (StreamWriter file = File.CreateText(tm.Trigger.Path))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                //serialize object directly into file stream
                                serializer.Serialize(file, tm.Trigger);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Character not in list, no need to remove");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Checked Group{tm.Name}");
            }
            RefactorActiveTriggers();
        }
        private void SfTreeView_QueryNodeSize(object sender, QueryNodeSizeEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                //Returns specified item height for items.
                e.Height = 20;
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
            TriggerManager selecteditem = (TriggerManager)treeview.SelectedItem;
            if (selecteditem.NodeType.Equals("group"))
            {
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
        private void treeview_ItemDropped(object sender, TreeViewItemDroppedEventArgs e)
        {
            //Folder where our item is going
            if (e.TargetNode != null)
            {
                TriggerManager desttm = (TriggerManager)e.TargetNode.Content;

                //The item we're draggin
                TriggerManager sourcetm = (TriggerManager)((SfTreeView)sender).SelectedItem;

                if (e.TargetNode.ParentNode == null && sourcetm.NodeType == "trigger")
                {
                    MessageBoxResult mbox = Xceed.Wpf.Toolkit.MessageBox.Show("Can't Move Trigger to Root", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //if it's a trigger, delete the old json and write the new one in the correct location
                    if (sourcetm.NodeType == "trigger")
                    {
                        sourcetm.UpdateTriggerLocation(desttm.FullPath);
                    }
                    else if (sourcetm.NodeType == "group")
                    {
                        sourcetm.UpdateTriggerGroupLocation(desttm.FullPath);
                    }
                }
            }
        }
        private void treeview_ItemDropping(object sender, TreeViewItemDroppingEventArgs e)
        {
            //Restrict the dropping on certain nodes
            TriggerManager node = e.TargetNode.Content as TriggerManager;
            if (((TriggerManager)((SfTreeView)sender).SelectedItem).NodeType == "trigger" && node.ParentNode == null)
                e.Handled = true;
        }
        private void treeview_ItemDragOver(object sender, TreeViewItemDragOverEventArgs e)
        {
            if (e.TargetNode != null)
            {
                TriggerManager node = e.TargetNode.Content as TriggerManager;
                if (node.SubGroups.Count() > 0)
                {
                    treeview.ExpandNode(e.TargetNode);
                }
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
            if(selecteditem == null || selecteditem.IsRootNode)
            {
                TriggerManager newtrigger = new TriggerManager { FullPath = $"{GlobalVariables.workingdirectory}\\TriggerGroups", NodeType = "group", Icon = GlobalVariables.foldericon, IsRootNode = true };
                newtrigger.CreateRootTriggerGroup();
                _triggermanager.Add(newtrigger);
                //resort collection
                _triggermanager = new ObservableCollection<TriggerManager>(_triggermanager.OrderBy(i => i.Name));
                treeview.ItemsSource = _triggermanager;
            }
            else if(selecteditem.NodeType.Equals("trigger"))
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
            if(selecteditem.IsRootNode)
            {
                _triggermanager.Remove(selecteditem);
            }
        }
        #endregion

        #region Trigger Clicks
        private void addTrigger_Click(object sender, RoutedEventArgs e)
        {
            EQTrigger newtrigger = ((TriggerManager)treeview.SelectedItem).AddTrigger(_characters);
            if(newtrigger != null)
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
            string todelete = selecteditem.FullPath;
            selecteditem.ParentNode.SubGroups.Remove(selecteditem);
            File.Delete(todelete);
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
                string name = ((Character)((ListView)e.Source).SelectedItem).Name;
                txtblockProfile.Text = ((Character)((ListView)e.Source).SelectedItem).Profile;
            }
            //Update treeview checkboxes for new character
        }
        private void _listviewCharacters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Boolean monitorstatus = ((Character)((ListView)e.Source).SelectedItem).Monitoring;
            if (monitorstatus)
            {
                ((Character)((ListView)e.Source).SelectedItem).Monitoring = false;
            }
            else
            {
                ((Character)((ListView)e.Source).SelectedItem).Monitoring = true;
                StartMonitor((Character)((ListView)e.Source).SelectedItem);
            }
        }
        private void MenuItemCharEdit_Click(object sender, RoutedEventArgs e)
        {
            Character selected = (Character)_listviewCharacters.SelectedItem;
            selected.EditCharacter();
        }
        private void MenuItemCharDelete_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Character)_listviewCharacters.SelectedValue).Name;
            ((Character)_listviewCharacters.SelectedValue).Delete();
            _characters.Remove((Character)_listviewCharacters.SelectedValue);
            PurgeFromTriggers(name);
        }
        private void MenuItemStartMonitor_Click(object sender, RoutedEventArgs e)
        {
            Character selected = (Character)_listviewCharacters.SelectedItem;
            selected.Monitoring = true;
            StartMonitor(selected);
        }
        private void MenuItemStopMonitor_Click(object sender, RoutedEventArgs e)
        {
            Character selected = (Character)_listviewCharacters.SelectedItem;
            selected.Monitoring = false;
        }
        private void UpdateCheckboxes()
        {
            treeview.CheckedItems.Clear();
            Character selectedchar = (Character)_listviewCharacters.SelectedItem;
            foreach (TriggerManager tm in _triggermanager)
            {
                if(tm.Characters.Contains(selectedchar))
                {
                    treeview.CheckedItems.Add(tm);
                }
            }
        }
        #endregion
    }
}
