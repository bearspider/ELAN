using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EQAudioTriggers.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Syncfusion.Windows.Shared;

namespace EQAudioTriggers.Views
{
    /// <summary>
    /// Interaction logic for CharacterEdit.xaml
    /// </summary>
    public partial class CharacterEdit : ChromelessWindow,INotifyPropertyChanged
    {
        SpeechSynthesizer voicesynth = new SpeechSynthesizer();
        private String _origProfileName;
        private Character _character;
        private Character _backupcharacter;

        public CharacterEdit()
        {
            _character = new Character();
            InitializeComponent();
            InitializeForm();
            this.DataContext = _character;
        }
        public CharacterEdit(Character character)
        {           
            InitializeComponent();
            InitializeForm();
            _character = character;
            this.DataContext = _character;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }

        public Character Character { get { return _character; } set { _character = value; RaisedOnPropertyChanged("Character"); } }
        public Character ReturnChar { get; set; }

        private void InitializeForm()
        {
            foreach (System.Speech.Synthesis.InstalledVoice installedVoice in voicesynth.GetInstalledVoices())
            {
                comboVoice.Items.Add(installedVoice.VoiceInfo.Name);
            }
            if (comboVoice.Items.Count > 0)
            {
                comboVoice.SelectedIndex = 0;
            }
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_character != null)
            {
                _character.AudioVolume = Convert.ToInt32(sliderVolume.Value);
            }
        }

        private void sliderRate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_character != null)
            {
                _character.VoiceSpeed = Convert.ToInt32(sliderRate.Value);
            }
        }

        private void comboVoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_character != null)
            {
                _character.AudioVoice = (String)((ComboBox)sender).SelectedValue;
            }
        }

        private void buttonPlayPhonetic_Click(object sender, RoutedEventArgs e)
        {
            _character.PhoenticName = textboxPhonetic.Text;
            voicesynth.Speak(_character.PhoenticName);
        }

        private void buttonPlaySample_Click(object sender, RoutedEventArgs e)
        {
            voicesynth.Speak(textboxSample.Text);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Boolean returnchar = true;
            this.ReturnChar = null;

            if(returnchar)
            {
                this.DialogResult = true;
                this.ReturnChar = _character;
            }
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void buttonLogFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Everquest Log Files|eqlog*.txt";
            string filePattern = @"eqlog_(.*)_(.*)\.txt";
            if (fileDialog.ShowDialog() == true)
            {
                _character.LogFile = fileDialog.FileName;
                Regex regexObj = new Regex(filePattern, RegexOptions.IgnoreCase);
                Match fileMatch = regexObj.Match(fileDialog.FileName);
                Group characterGroup = fileMatch.Groups[1];
                Group serverGroup = fileMatch.Groups[2];
                CaptureCollection characterCollection = characterGroup.Captures;
                CaptureCollection serverCollection = serverGroup.Captures;
                _character.Profile = characterCollection[0].ToString() + "(" + serverCollection[0].ToString() + ")";
                _character.Name = characterCollection[0].ToString();
                _character.PhoenticName = characterCollection[0].ToString();
            }
        }

    }
}
