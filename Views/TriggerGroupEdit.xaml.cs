using EQAudioTriggers.Models;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System.Windows;

namespace EQAudioTriggers.Views
{
    /// <summary>
    /// Interaction logic for TriggerGroupEdit.xaml
    /// </summary>
    public partial class TriggerGroupEdit : ChromelessWindow
    {
        private TriggerGroupProperty _triggergroup;

        public TriggerGroupEdit()
        {
            InitializeComponent();
            _triggergroup = new TriggerGroupProperty();
            _tgeditor.DataContext = _triggergroup;
        }

        public TriggerGroupEdit(TriggerGroupProperty tgp)
        {
            InitializeComponent();
            _triggergroup = tgp;
            _tgeditor.DataContext = _triggergroup;
        }
        public void SetTheme(string theme)
        {
            SfSkinManager.SetTheme(this, new Theme(theme));
        }

        public TriggerGroupProperty TriggerGroup
        {
            get { return _triggergroup; }
            set { _triggergroup = value; }
        }
        public TriggerGroupProperty ReturnTriggerGroup { get; set; }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.ReturnTriggerGroup = _triggergroup;
            this.Close();
        }
    }
}
