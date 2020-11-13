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
            try
            {
                TreeViewNode tvn = (TreeViewNode)value;
                if (((TriggerManager)tvn.Content).NodeType == "trigger")
                {
                    return true;
                }
            }
            catch(Exception ex)
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
        private static StringCollection _log = new StringCollection();
        private readonly SynchronizationContext syncontext;
        private ObservableCollection<EQTrigger> _triggermasterlist = new ObservableCollection<EQTrigger>();
        private string _selectedcharacter = "";

        #endregion
        public EATStyleWindow()
        {
            InitializeComponent();
            DataContext = this;
            syncontext = SynchronizationContext.Current;
            //po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            LoadSettings();
            LoadCharacter();
            LoadTriggers();                        
            ActivateLog();
            if(_characters.Count > 0)
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
            UpdateCheckedItems();
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
                        //check to see if some how monitor is enabled but monitoring disabled
                        if(newchar.Monitor)
                        {
                            newchar.Monitoring = true;
                        }
                        CharacterCollection newcollection = new CharacterCollection();
                        newcollection.CharacterProfile = newchar;
                        newcollection.Name = newchar.Name;
                        _characters.Add(newcollection);
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
            //Load Triggers into Collection
            String StartPath = $"{EQAudioTriggers.GlobalVariables.workingdirectory}\\TriggerGroups";
            DirectoryInfo di = new DirectoryInfo(StartPath);
            foreach(DirectoryInfo newdir in di.GetDirectories())
            {
                TriggerManager newtrigger = new TriggerManager { 
                    FullPath = newdir.FullName, 
                    Name = newdir.Name, 
                    NodeType = "group", 
                    Icon = GlobalVariables.foldericon, 
                    IsRootNode = true, 
                    ParentNode = null, 
                    IsActive = false 
                };
                treeview.LoadOnDemandCommand = newtrigger.TreeViewOnDemandCommand;
                _triggermanager.Add(newtrigger);
                WalkDirectoryTree(newtrigger);
                SetCheckedItems(newtrigger);
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
                            var activechars = _characters.Where(i => i.CharacterProfile.Monitoring == true && newtrigger.ActiveCharacters.Contains(i.CharacterProfile.Name));
                            if(activechars.Count() > 0)
                            {
                                _activetriggers.Collection.Add(newtrigger);
                            }

                            // Resursive call for each subdirectory.
                            TriggerManager newmanager = new TriggerManager
                            {
                                FullPath = fi.FullName,
                                Name = newtrigger.Name,
                                NodeType = "trigger",
                                Icon = GlobalVariables.triggericon,
                                Trigger = newtrigger,
                                ParentNode = root,
                                IsActive = false
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
                        ParentNode = root,
                        IsActive = false
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
        private Character CreateCharacter()
        {
            CharacterEdit chareditor = new CharacterEdit();
            Boolean rval = (bool)chareditor.ShowDialog();
            if (chareditor.ReturnChar != null)
            {
                return chareditor.ReturnChar;
            }
            else
            {
                return null;
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
            Character toadd = CreateCharacter();
            if (toadd != null)
            {
                CharacterCollection newcollection = new CharacterCollection();
                newcollection.Name = toadd.Profile;
                newcollection.CharacterProfile = toadd;
                _characters.Add(newcollection);
            }
        }
        private void editUser_Click(object sender, RoutedEventArgs e)
        {
            ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile.EditCharacter();
        }
        private void removeUser_Click(object sender, RoutedEventArgs e)
        {
            string name = ((CharacterCollection)_listviewCharacters.SelectedValue).Name;
            ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile.Delete();
            _characters.Remove((CharacterCollection)_listviewCharacters.SelectedValue);
            PurgeFromTriggers(name);
        }
        #endregion

        #region TreeView
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
            if (e.TargetNode != null)
            {
                try
                {
                    TriggerManager node = e.TargetNode.Content as TriggerManager;
                    if (((TriggerManager)((SfTreeView)sender).SelectedItem) != null)
                    {
                        if (((TriggerManager)((SfTreeView)sender).SelectedItem).NodeType == "trigger" && node.ParentNode == null)
                            e.Handled = true;
                    }
                    else
                    {
                        e.Handled = true;
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
                if(enable){ EnableTrigger(tm); }
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
                    SetCheckedItems(sub,enable);
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
                if(tm.Trigger.ActiveCharacters.Contains(_selectedcharacter))
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
            if(tm.NodeType == "group")
            {
                if(tm.SubGroups.Count > 1)
                {
                    string stop = "";
                }
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
                txtblockProfile.Text = ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Profile;
                _selectedcharacter = ((CharacterCollection)((ListView)e.Source).SelectedItem).CharacterProfile.Name;
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
            Character selected = ((CharacterCollection)_listviewCharacters.SelectedItem).CharacterProfile;
            selected.EditCharacter();
        }
        private void MenuItemCharDelete_Click(object sender, RoutedEventArgs e)
        {
            string name = ((CharacterCollection)_listviewCharacters.SelectedValue).Name;
            ((CharacterCollection)_listviewCharacters.SelectedValue).CharacterProfile.Delete();
            _characters.Remove((CharacterCollection)_listviewCharacters.SelectedValue);
            PurgeFromTriggers(name);
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
                SetCheckedItems(target,true);
            }
            //walk back up the tree and do parent checks
            target.ClimbTree(true);
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
            if(target.NodeType == "group")
            {
                SetCheckedItems(target, false);
            }
            //walk back up the tree and do parent unchecks
            target.ClimbTree(false);
        }

        #region Trigger
        private void DisableTrigger(TriggerManager tm)
        {
            string character = ((CharacterCollection)(_listviewCharacters.SelectedItem)).Name;
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
            _activetriggers.Refactor();
        }
        private void EnableTrigger(TriggerManager tm)
        {
            Console.WriteLine("Calling Node Checked");
            string character = ((CharacterCollection)(_listviewCharacters.SelectedItem)).Name;
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
            _activetriggers.Refactor();
        }
        #endregion
    }
}