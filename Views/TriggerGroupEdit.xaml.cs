using EQAudioTriggers.Models;
using Newtonsoft.Json;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.IO;
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

        public TriggerGroupProperty TriggerGroup { 
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
