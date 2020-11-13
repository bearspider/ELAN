using EQAudioTriggers.Command;
using EQAudioTriggers.Views;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Grid.ScrollAxis;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace EQAudioTriggers.Models
{
    public class TriggerManager : INotifyPropertyChanged
    {
        private string _name;
        private string _comments;
        private Boolean _defaultenable;
        private string _fullpath;
        private bool _hasChildNodes;
        private string _nodetype;
        private string _icon;
        private bool? _isactive;
        private Boolean _isrootnode;
        private TriggerManager _parentnode;
        private EQTrigger _trigger;
        private TriggerGroupProperty _triggergroup;

        DispatcherTimer timer;
        TreeViewNode currentNode;
        string currentCharacter;

        private ObservableCollection<TriggerManager> _subGroups;
        private ObservableCollection<Character> _characters;

        public TriggerManager()
        {
            _name = "";
            _comments = "";
            _defaultenable = false;
            _fullpath = "";
            _subGroups = new ObservableCollection<TriggerManager>();
            _nodetype = "group";
            _icon = "";
            _isactive = false;
            _characters = new ObservableCollection<Character>();
            _trigger = new EQTrigger();
            _triggergroup = new TriggerGroupProperty();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            timer.Tick += Timer_Tick;
            TreeViewOnDemandCommand = new OnDemandCommand(ExecuteOnDemandLoading, CanExecuteOnDemandLoading);
        }

        private ICommand treeViewOnDemandCommand;
        public ICommand TreeViewOnDemandCommand
        {
            get { return treeViewOnDemandCommand; }
            set { treeViewOnDemandCommand = value; }
        }

        #region Public Access

        public TriggerGroupProperty TriggerGroup
        {
            get { return _triggergroup; }
            set
            {
                _triggergroup = value;
                RaisedOnPropertyChanged("TriggerGroup");
            }
        }

        public TriggerManager ParentNode
        {
            get { return _parentnode; }
            set
            {
                _parentnode = value;
                RaisedOnPropertyChanged("ParentNode");
            }
        }

        public Boolean IsRootNode
        {
            get { return _isrootnode; }
            set
            {
                _isrootnode = value;
                RaisedOnPropertyChanged("IsRootNode");

            }
        }

        public bool? IsActive
        {
            get { return _isactive; }
            set
            {
                _isactive = value;
                RaisedOnPropertyChanged("IsActive");
            }
        }

        public EQTrigger Trigger
        {
            get { return _trigger; }
            set
            {
                _trigger = value;
                RaisedOnPropertyChanged("Trigger");
            }
        }

        public ObservableCollection<Character> Characters
        {
            get { return _characters; }
            set
            {
                _characters = value;
                RaisedOnPropertyChanged("Characters");
            }
        }

        public Boolean HasChildNodes
        {
            get { return _hasChildNodes; }
            set
            {
                _hasChildNodes = value;
                RaisedOnPropertyChanged("HasChildNodes");
            }
        }

        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisedOnPropertyChanged("Icon");
            }
        }
        public string NodeType
        {
            get { return _nodetype; }
            set
            {
                _nodetype = value;
                RaisedOnPropertyChanged("NodeType");
            }
        }

        public ObservableCollection<TriggerManager> SubGroups
        {
            get { return _subGroups; }
            set
            {
                _subGroups = value;
                RaisedOnPropertyChanged("SubGroups");
            }
        }

        public string FullPath
        {
            get { return _fullpath; }
            set
            {
                _fullpath = value;
                RaisedOnPropertyChanged("FullPath");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisedOnPropertyChanged("Name");
            }
        }

        public string Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                RaisedOnPropertyChanged("Comments");
            }
        }
        public Boolean DefaultEnable
        {
            get { return _defaultenable; }
            set
            {
                _defaultenable = value;
                RaisedOnPropertyChanged("DefaultEnable");
            }
        }
        #endregion
        public void ClimbTree(Boolean enable)
        {
            //get the active count of the sub nodes, this will be used in a couple spots
            int totalsubs = this.SubGroups.Count;
            int activecount = 0;
            Boolean partial = false; 
            foreach (TriggerManager sub in this.SubGroups)
            {
                if (sub.IsActive == true)
                {
                    activecount++;
                }
                if(sub.IsActive == null)
                {
                    partial = true;
                }
            }
            if (partial)
            {
                this.IsActive = null;
            }
            else
            {
                Console.WriteLine($"Node: {Name} | Total Subs[{this.SubGroups.Count}] Active Count[{activecount}]");
                if (activecount == totalsubs && this.SubGroups.Count > 0)
                { this.IsActive = true; }
                else if (activecount > 0 && activecount < totalsubs)
                { this.IsActive = null; }
                else if (activecount == 0 && this.SubGroups.Count > 0)
                { this.IsActive = false; }
            }
            if (ParentNode != null)
            {
                //Mark nodes first, then climb the tree
                ParentNode.ClimbTree(enable);
            }
        }

        public void EditTriggerGroup()
        {
            TriggerGroupEdit tge = new TriggerGroupEdit(this.TriggerGroup);
            Boolean rval = (bool)tge.ShowDialog();
            if (rval)
            {
                //check if the name changed, if true, then we need to rename the old folder
                if(_name != tge.ReturnTriggerGroup.Name)
                {
                    Directory.Move(_fullpath, $"{_parentnode.FullPath}\\{tge.ReturnTriggerGroup.Name}");
                    FullPath = $"{_parentnode.FullPath}\\{tge.ReturnTriggerGroup.Name}";
                    Name = tge.ReturnTriggerGroup.Name;
                }
                Comments = tge.ReturnTriggerGroup.Comments;
                DefaultEnable = tge.ReturnTriggerGroup.DefaultEnabled;
                TriggerGroup = tge.ReturnTriggerGroup;

                //open file stream
                using (StreamWriter file = File.CreateText($"{_fullpath}\\properties.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, _triggergroup);
                }
            }
        }

        public void AddTriggerGroup()
        {
            TriggerGroupEdit tge = new TriggerGroupEdit();
            Boolean rval = (bool)tge.ShowDialog();
            if (rval)
            {
                //Check if this node has any children, because now it does
                if (HasChildNodes != true)
                {
                    HasChildNodes = true;
                }

                //build the new trigger manager
                TriggerManager tm = new TriggerManager
                {
                    Name = tge.ReturnTriggerGroup.Name,
                    Comments = tge.ReturnTriggerGroup.Comments,
                    DefaultEnable = tge.ReturnTriggerGroup.DefaultEnabled,
                    ParentNode = this,
                    TriggerGroup = tge.ReturnTriggerGroup,
                    Icon = GlobalVariables.foldericon,
                    FullPath = $"{this.FullPath}\\{tge.ReturnTriggerGroup.Name}",
                };
                tm.TriggerGroup.FullPath = tm.FullPath;
                //add it as a child to this trigger manager
                SubGroups.Add(tm);

                //create directory
                Directory.CreateDirectory(tm.FullPath);
                //create properties.json in folder
                using (StreamWriter file = File.CreateText($"{tm.FullPath}\\properties.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, tm.TriggerGroup);
                }
            }
        }

        public void CreateRootTriggerGroup()
        {
            TriggerGroupEdit tge = new TriggerGroupEdit(_fullpath);
            Boolean rval = (bool)tge.ShowDialog();
            if (rval)
            {
                Name = tge.ReturnTriggerGroup.Name;
                Comments = tge.ReturnTriggerGroup.Comments;
                DefaultEnable = tge.ReturnTriggerGroup.DefaultEnabled;
                TriggerGroup = tge.ReturnTriggerGroup;
                Icon = GlobalVariables.foldericon;
                FullPath = $"{tge.ReturnTriggerGroup.FullPath}\\{tge.ReturnTriggerGroup.Name}";
            }
            //create directory
            Directory.CreateDirectory(FullPath);
            //create properties.json in folder
            using (StreamWriter file = File.CreateText($"{FullPath}\\properties.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, TriggerGroup);
            }
        }

        public void RemoveTriggerGroup()
        {
            Directory.Delete(_fullpath, true);
            if(!this.IsRootNode)
            {
                ParentNode.SubGroups.Remove(this);
            }
            
        }

        public EQTrigger AddTrigger(ObservableCollection<CharacterCollection> Characters)
        {
            TriggerEdit te = new TriggerEdit(Characters);
            Boolean rval = (bool)te.ShowDialog();
            if(rval)
            {
                //Check if this node has any children, because now it does
                if (HasChildNodes != true)
                {
                    HasChildNodes = true;
                }

                //build the new trigger manager
                te.ReturnTrigger.Path = $"{this.FullPath}\\{te.ReturnTrigger.Name}.json";
                TriggerManager tm = new TriggerManager
                {
                    Name = te.ReturnTrigger.Name,
                    ParentNode = this,
                    Trigger = te.ReturnTrigger,
                    Icon = GlobalVariables.triggericon,
                    NodeType = "trigger",
                    FullPath = te.ReturnTrigger.Path,
                };
                
                //If the parent group says default enable, enable this trigger
                if(DefaultEnable)
                {
                    foreach(CharacterCollection character in Characters)
                    {
                        tm.Trigger.ActiveCharacters.Add(character.Name);
                    }
                }

                //add it as a child to this trigger manager
                SubGroups.Add(tm);

                //create properties.json in folder
                using (StreamWriter file = File.CreateText($"{tm.FullPath}"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, tm.Trigger);
                }
                return tm.Trigger;
            }
            return null;
        }

        public void UpdateTriggerLocation(string filename)
        {
            string oldfile = this.FullPath;
            this.FullPath = $"{filename}\\{this.Name}.json";
            this.Trigger.Path = this.FullPath;            
            //write new trigger
            using (StreamWriter file = File.CreateText($"{this.FullPath}"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, this.Trigger);
            }
            //check if the new file exists before deleting trigger
            if(File.Exists(this.FullPath))
            {
                File.Delete(oldfile);
            }            
        }

        public void UpdateTriggerGroupLocation(string filename)
        {
            string oldfile = this.FullPath;
            this.FullPath = $"{filename}\\{this.Name}";
            this.TriggerGroup.FullPath = this.FullPath;
            //check if the new destination makes this a root node

            //Walk through the tree, use this function recursively

            //check if a folder with the triggergroupname exists
            //   no => create new folder
            if(!Directory.Exists(this.FullPath))
            {
                Directory.CreateDirectory(this.FullPath);
            }

            //create new properties.json
            using (StreamWriter file = File.CreateText($"{this.FullPath}\\properties.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, this.TriggerGroup);
            }

            //check if this has subgroups
            //   yes => go through each node and call updatetriggerlocation or updatetriggergrouplocation
            if (this.SubGroups.Count > 0)
            {
                foreach(TriggerManager sub in SubGroups)
                {
                    if(sub.NodeType == "trigger")
                    {
                        sub.UpdateTriggerLocation($"{this.FullPath}");
                    }
                    if(sub.NodeType == "group")
                    {
                        sub.UpdateTriggerGroupLocation($"{this.FullPath}");
                    }
                }
            }
            //Check if there are subdirectories, if there are don't do anything.  If only properties.json, then delete folder.
            if ((Directory.EnumerateDirectories(oldfile).Count() == 0))
            {
                //Finally, delete properties.json and old folder if empty
                try
                {
                    File.Delete($"{oldfile}\\properties.json");
                    Directory.Delete(oldfile);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Directory Not Empty");
                }
            }
            
        }

        public void RemoveTrigger()
        {
            File.Delete(_fullpath);
            ParentNode.SubGroups.Remove(this);
        }

        public void EditTrigger(ObservableCollection<CharacterCollection> charcollection)
        {
            TriggerEdit te = new TriggerEdit(this.Trigger, charcollection);
            Boolean rval = (bool)te.ShowDialog();
            if(rval)
            {
                if(_name != te.ReturnTrigger.Name)
                {
                    File.Delete(_fullpath);
                    FullPath = $"{this.ParentNode.FullPath}\\{te.ReturnTrigger.Name}.json";
                    Name = te.ReturnTrigger.Name;
                }
                //open file stream
                using (StreamWriter file = File.CreateText(this.FullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, _trigger);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TriggerManager tm = currentNode.Content as TriggerManager;
            //currentNode.IsChecked = tm.IsActive;

            CollectionViewSource treevs = new CollectionViewSource();
            SortDescription desc = new SortDescription("Name", ListSortDirection.Ascending);
            PropertyGroupDescription pgd = new PropertyGroupDescription("NodeType");
            treevs.IsLiveFilteringRequested = true;
            treevs.SortDescriptions.Add(desc);
            treevs.GroupDescriptions.Add(pgd);
            treevs.Source = tm.SubGroups;

            //Populating child items for the node in on-demand
            currentNode.PopulateChildNodes(treevs.View);
            if (tm.SubGroups.Count() > 0)
                //Expand the node after child items are added.
                currentNode.IsExpanded = true;

            //Stop the animation after load on demand is executed, if animation not stopped, it remains still after execution of load on demand.
            currentNode.ShowExpanderAnimation = false;

            //refactor checkmarks
            timer.Stop();
        }

        /// <summary>
        /// CanExecute method is called before expanding and initialization of node. Returns whether the node has child nodes or not.
        /// Based on return value, expander visibility of the node is handled.  
        /// </summary>
        /// <param name="sender">TreeViewNode is passed as default parameter </param>
        /// <returns>Returns true, if the specified node has child items to load on demand and expander icon is displayed for that node, else returns false and icon is not displayed.</returns>
        private bool CanExecuteOnDemandLoading(object sender)
        {
            var tm = ((sender as TreeViewNode).Content as TriggerManager);
            if (tm.HasChildNodes)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Execute method is called when any item is requested for load-on-demand items.
        /// </summary>
        /// <param name="obj">TreeViewNode is passed as default parameter </param>
        private void ExecuteOnDemandLoading(object obj)
        {
            var node = obj as TreeViewNode;

            // Skip the repeated population of child items when every time the node expands.
            if (node.ChildNodes.Count > 0)
            {
                node.IsExpanded = true;
                return;
            }

            //Animation starts for expander to show progressing of load on demand
            node.ShowExpanderAnimation = true;
            var treeView = Application.Current.MainWindow.FindName("treeview") as SfTreeView;
            treeView.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                currentNode = node;
                timer.Start();
            }));
        }
    }
}
