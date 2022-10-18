using EQAudioTriggers.Command;
using EQAudioTriggers.Views;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace EQAudioTriggers.Models
{
    public class TriggerManager : INotifyPropertyChanged
    {
        private string _name;
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
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisedOnPropertyChanged("Name");
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
                if (sub.IsActive == null)
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
        public void EditTriggerGroup(string theme)
        {
            TriggerGroupEdit tge = new TriggerGroupEdit(this.TriggerGroup);
            tge.SetTheme(theme);
            Boolean rval = (bool)tge.ShowDialog();
            if (rval)
            {
                Name = tge.ReturnTriggerGroup.Name;
                TriggerGroup = tge.ReturnTriggerGroup;
            }
        }
        public TriggerManager AddTriggerGroup(string theme)
        {
            TriggerGroupEdit tge = new TriggerGroupEdit();
            tge.SetTheme(theme);
            Boolean rval = (bool)tge.ShowDialog();
            if (rval)
            {
                //build the new trigger manager
                TriggerManager tm = new TriggerManager
                {
                    Name = tge.ReturnTriggerGroup.Name,
                    ParentNode = this,
                    TriggerGroup = tge.ReturnTriggerGroup,
                    Icon = GlobalVariables.foldericon,
                };
                //Add the new ID to his subgroup
                this.TriggerGroup.SubGroups.Add(tm.TriggerGroup.Id);
                //Add this ID to the new group parent id
                tm.TriggerGroup.ParentId = this.TriggerGroup.Id;
                //add it as a child to this trigger manager
                SubGroups.Add(tm);
                return tm;
            }
            return null;
        }
        public void CreateRootTriggerGroup(string theme)
        {
            TriggerGroupEdit tge = new TriggerGroupEdit();
            tge.SetTheme(theme);
            Boolean rval = (bool)tge.ShowDialog();
            if (rval)
            {
                this.Name = tge.ReturnTriggerGroup.Name;
                this.TriggerGroup = tge.ReturnTriggerGroup;
            }
        }
        public void RemoveTriggerGroup()
        {
            //if we have sub groups, delete those too
            foreach (TriggerManager subgroup in SubGroups.ToList<TriggerManager>())
            {
                if (subgroup.NodeType == "group")
                {
                    subgroup.RemoveTriggerGroup();
                }
                if (subgroup.NodeType == "trigger")
                {
                    subgroup.RemoveTrigger();
                }
            }
            //Once we've cleared out all the children, remove ourself from the parent node.
            if (!this.IsRootNode)
            {
                ParentNode.SubGroups.Remove(this);
                ParentNode.TriggerGroup.SubGroups.Remove(TriggerGroup.Id);
                //Update the parent node in the DB
            }
        }
        public EQTrigger AddTrigger(ObservableCollection<CharacterCollection> Characters, string theme)
        {
            TriggerEdit te = new TriggerEdit(Characters);
            te.SetTheme(theme);
            Boolean rval = (bool)te.ShowDialog();
            if (rval)
            {
                //build the new trigger manager
                TriggerManager tm = new TriggerManager
                {
                    Name = te.ReturnTrigger.Name,
                    ParentNode = this,
                    Trigger = te.ReturnTrigger,
                    Icon = GlobalVariables.triggericon,
                    NodeType = "trigger",
                };

                //If the parent group says default enable, enable this trigger
                if (TriggerGroup.DefaultEnabled)
                {
                    foreach (CharacterCollection character in Characters)
                    {
                        tm.Trigger.ActiveCharacters.Add(character.Name);
                    }
                }

                //add it as a child to this trigger manager
                SubGroups.Add(tm);

                //add group id to trigger and trigger to group
                te.ReturnTrigger.GroupId = TriggerGroup.Id;
                TriggerGroup.Triggers.Add(te.ReturnTrigger.Id);
                return tm.Trigger;
            }
            return null;
        }
        public Boolean AddTrigger(ObservableCollection<CharacterCollection> Characters, EQTrigger copytrigger, string theme)
        {
            TriggerEdit te = new TriggerEdit(copytrigger, Characters);
            te.SetTheme(theme);
            Boolean rval = (bool)te.ShowDialog();
            if (rval)
            {
                //build the new trigger manager
                TriggerManager tm = new TriggerManager
                {
                    Name = te.ReturnTrigger.Name,
                    ParentNode = this,
                    Trigger = te.ReturnTrigger,
                    Icon = GlobalVariables.triggericon,
                    NodeType = "trigger",
                };

                //If the parent group says default enable, enable this trigger
                if (TriggerGroup.DefaultEnabled)
                {
                    foreach (CharacterCollection character in Characters)
                    {
                        tm.Trigger.ActiveCharacters.Add(character.Name);
                    }
                }

                //add it as a child to this trigger manager
                SubGroups.Add(tm);

                //add group id to trigger and trigger to group
                te.ReturnTrigger.GroupId = TriggerGroup.Id;
                TriggerGroup.Triggers.Add(te.ReturnTrigger.Id);
                return true;
            }
            return false;
        }
        public void RemoveTrigger()
        {
            //Remove trigger from this group and update the group entry in the database
            ParentNode.TriggerGroup.Triggers.Remove(Trigger.Id);

            //Remove this triggermanager from parent node
            ParentNode.SubGroups.Remove(this);

        }
        public void EditTrigger(ObservableCollection<CharacterCollection> charcollection, string theme)
        {
            TriggerEdit te = new TriggerEdit(this.Trigger, charcollection);
            te.SetTheme(theme);
            Boolean rval = (bool)te.ShowDialog();
            if (rval)
            {
                //See if the name changed, if it did, update the triggermanager
                if (_name != te.ReturnTrigger.Name)
                {
                    Name = te.ReturnTrigger.Name;
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            TriggerManager tm = currentNode.Content as TriggerManager;

            CollectionViewSource treevs = new CollectionViewSource();
            SortDescription desc = new SortDescription("Name", ListSortDirection.Ascending);
            SortDescription groupDesc = new SortDescription("NodeType", ListSortDirection.Ascending);
            PropertyGroupDescription pgd = new PropertyGroupDescription("NodeType");
            treevs.IsLiveFilteringRequested = true;
            treevs.GroupDescriptions.Add(pgd);
            treevs.SortDescriptions.Add(groupDesc);
            treevs.SortDescriptions.Add(desc);
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
            //This doesn't seem to be working right, returning false makes it work correctly.
            var tm = ((sender as TreeViewNode).Content as TriggerManager);
            if (SubGroups.Count > 0)
                return false;
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
