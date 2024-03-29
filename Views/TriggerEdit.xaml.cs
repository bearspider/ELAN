﻿using EQAudioTriggers.Models;
using Microsoft.Win32;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ToolTip = Syncfusion.UI.Xaml.Diagram.ToolTip;

namespace EQAudioTriggers.Views
{
    /// <summary>
    /// Convert a Boolean value to icon which will display the monitoring status of the character
    /// </summary>
    /// <returns>Returns the image to be used on the form, Monitoring or not monitoring</returns>
    public class InverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Boolean rval = true;
            if ((Boolean)value)
            {
                rval = false;
            }
            return rval;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Interaction logic for TriggerEdit.xaml
    /// </summary>
    public partial class TriggerEdit : ChromelessWindow
    {
        private EQTrigger _eqtrigger;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<CharacterCollection> _characters;
        public ObservableCollection<CharacterCollection > Characters { get { return _characters; } set { _characters = value; } }
        public ObservableCollection<Category> Categories { get { return _categories; } set { _categories = value; } }
        public EQTrigger EQTrigger { get { return _eqtrigger; } set { _eqtrigger = value; } }
        private Regex digestregex = new Regex(@"(?<digest>\w\s?\w?\s?\w?\s?\w?\s?\w?\s?\w?\s?\w?\s?)", RegexOptions.Compiled);
        private ScreenTip sc = new ScreenTip();
        public void SetTheme(string theme)
        {
            SfSkinManager.SetTheme(this, new Theme(theme));
        }
        public TriggerEdit()
        {
            InitializeComponent();
            _eqtrigger = new EQTrigger();
            DataContext = this;
        }
        public TriggerEdit(EQTrigger oldtrigger)
        {
            InitializeComponent();
            _eqtrigger = oldtrigger;
            DataContext = this;
            sc.Description = "Fast Check Digest";
            sc.HelpText = "Does a simple string check first before applying a regex.";
            checkboxFast.ToolTip = sc;
        }

        public TriggerEdit(ObservableCollection<CharacterCollection> characters, ObservableCollection<Category> categories)
        {
            InitializeComponent();
            _eqtrigger = new EQTrigger();
            Category defaultcategory = categories.Where(x => x.DefaultCategory == true).FirstOrDefault();
            _eqtrigger.Category = defaultcategory.CategoryName;
            DataContext = this;
            comboEndedTest.ItemsSource = comboEndingTest.ItemsSource = comboBasicTest.ItemsSource = characters;
            sc.Description = "Fast Check Digest";
            sc.HelpText = "Does a simple string check first before applying a regex.";
            checkboxFast.ToolTip = sc;
        }

        public TriggerEdit(EQTrigger oldtrigger, ObservableCollection<CharacterCollection> characters, ObservableCollection<Category> categories)
        {
            InitializeComponent();
            _eqtrigger = oldtrigger;
            _categories = categories;
            _characters = characters;
            DataContext = this;
            sc.Description = "Fast Check Digest";
            sc.HelpText = "Does a simple string check first before applying a regex.";
            checkboxFast.ToolTip = sc;
        }

        public EQTrigger ReturnTrigger { get; set; }

        private void buttonTimerCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void buttonTimerSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ReturnTrigger = _eqtrigger;
            this.Close();
        }

        private void PlaySoundFile(string filelocation)
        {
            SoundPlayer test = new SoundPlayer(filelocation);
            test.Stream.Position = 0;
            test.Play();
        }

        private string GetSoundFile()
        {
            string rval = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Trigger Sound Files|*.wav";
            string filePattern = @"*.wav";
            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }
            return rval;
        }

        private void buttonBasicSoundFile_Click(object sender, RoutedEventArgs e)
        {
            string soundfile = GetSoundFile();
            if (!string.IsNullOrEmpty(soundfile))
            {
                _eqtrigger.BasicPlayFile = soundfile;
            }
        }

        private void buttonBasicTest_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)radioBasicTTS.IsChecked && !string.IsNullOrEmpty(textboxBasicTTS.Text))
            {
                ((CharacterCollection)comboBasicTest.SelectedItem).CharacterProfile.Speak(textboxBasicTTS.Text);
            }
            if ((bool)radioBasicPlay.IsChecked && !string.IsNullOrEmpty(textboxBasicSoundFile.Text))
            {
                PlaySoundFile(textboxBasicSoundFile.Text);
            }
        }

        private void buttonEndingSoundFile_Click(object sender, RoutedEventArgs e)
        {
            string soundfile = GetSoundFile();
            if (!string.IsNullOrEmpty(soundfile))
            {
                _eqtrigger.EndingSoundFile = soundfile;
            }
        }

        private void buttonEndingTest_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)radioEndingTTS.IsChecked && !string.IsNullOrEmpty(textboxEndingTTS.Text))
            {
                ((CharacterCollection)comboEndingTest.SelectedItem).CharacterProfile.Speak(textboxEndingTTS.Text);
            }
            if ((bool)radioEndingPlay.IsChecked && !string.IsNullOrEmpty(textboxEndingSoundFile.Text))
            {
                PlaySoundFile(textboxEndingSoundFile.Text);
            }
        }

        private void buttonEndedSoundFile_Click(object sender, RoutedEventArgs e)
        {
            string soundfile = GetSoundFile();
            if (!string.IsNullOrEmpty(soundfile))
            {
                _eqtrigger.EndedSoundFile = soundfile;
            }
        }

        private void buttonEndedTest_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)radioEndedTTS.IsChecked && !string.IsNullOrEmpty(textboxEndedTTS.Text))
            {
                ((CharacterCollection)comboEndedTest.SelectedItem).CharacterProfile.Speak(textboxEndedTTS.Text);
            }
            if ((bool)radioEndedPlay.IsChecked && !string.IsNullOrEmpty(textboxEndedSoundFile.Text))
            {
                PlaySoundFile(textboxEndedSoundFile.Text);
            }
        }

        private void checkboxEndedNotify_Unchecked(object sender, RoutedEventArgs e)
        {
            //uncheck display text
            checkboxEndedDisplay.IsChecked = false;
            //clear display text
            textboxEndedDisplay.Text = "";
            //uncheck clipboard text
            checkboxEndedClipboard.IsChecked = false;
            //clear clipboard text
            textboxEndedClipboard.Text = "";
            //radio button = no sound
            radioEndedNoSound.IsChecked = true;
            //clear tts text box
            textboxEndedTTS.Text = "";
            //clear interrupt checkbox
            checkboxEndedInterrupt.IsChecked = false;
            //clear soundfile textbox
            textboxEndedSoundFile.Text = "";
        }

        private void checkboxCounterNotify_Unchecked(object sender, RoutedEventArgs e)
        {
            //clear reset counter text boxes
            textboxCounterHours.Text = "0";
            textboxCounterMinutes.Text = "0";
            textboxCounterSeconds.Text = "0";
        }

        private void checkboxEndingNotify_Unchecked(object sender, RoutedEventArgs e)
        {
            //reset timer textboxes
            textboxEndingHours.Text = "0";
            textboxEndingMinutes.Text = "0";
            textboxEndingSeconds.Text = "0";
            //uncheck display text, clear textbox
            textboxEndingDisplay.Text = "";
            checkboxEndingDisplay.IsChecked = false;
            //uncheck clipboard text, clear textbox
            textboxEndingClipboard.Text = "";
            checkboxEndingClipboard.IsChecked = false;
            //check no sound
            radioEndingNoSound.IsChecked = true;
            //clear tts textbox
            textboxEndingTTS.Text = "";
            //uncheck interrupt
            checkboxEndingInterrupt.IsChecked = false;
            //clear sound file textbox
            textboxEndingSoundFile.Text = "";
        }

        private void comboTimerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabEnded != null && tabEnding != null && comboTimerType.SelectedValue != null)
            {
                string selection = ((ComboBoxItem)comboTimerType.SelectedValue).Content.ToString();
                switch (selection)
                {
                    case "No Timer":
                        tabEnding.IsEnabled = false;
                        tabEnded.IsEnabled = false;
                        labelTimerName.IsEnabled = false;
                        labelTimerDuration.IsEnabled = false;
                        textboxTimerHours.IsEnabled = false;
                        textboxTimerMinutes.IsEnabled = false;
                        textboxTimerSeconds.IsEnabled = false;
                        textboxTimerHours.Text = "0";
                        textboxTimerMinutes.Text = "0";
                        textboxTimerSeconds.Text = "0";
                        labelTimerTriggered.IsEnabled = false;
                        labelEarlyText.IsEnabled = false;
                        datagridEarly.IsEnabled = false;
                        _eqtrigger.EndEarlyTriggers.Clear();
                        comboTriggered.IsEnabled = false;
                        textboxTimerName.IsEnabled = false;
                        break;
                    case "Timer(Count Down)":
                        labelTimerName.IsEnabled = true;
                        labelTimerDuration.IsEnabled = true;
                        textboxTimerHours.IsEnabled = true;
                        textboxTimerMinutes.IsEnabled = true;
                        textboxTimerSeconds.IsEnabled = true;
                        labelTimerTriggered.IsEnabled = true;
                        labelEarlyText.IsEnabled = true;
                        datagridEarly.IsEnabled = true;
                        tabEnding.IsEnabled = true;
                        tabEnded.IsEnabled = true;
                        comboTriggered.IsEnabled = true;
                        textboxTimerName.IsEnabled = true;
                        break;
                    case "Stopwatch(Count Up)":
                        labelTimerName.IsEnabled = true;
                        labelTimerDuration.IsEnabled = true;
                        textboxTimerHours.IsEnabled = true;
                        textboxTimerMinutes.IsEnabled = true;
                        textboxTimerSeconds.IsEnabled = true;
                        labelTimerTriggered.IsEnabled = true;
                        labelEarlyText.IsEnabled = true;
                        datagridEarly.IsEnabled = true;
                        tabEnding.IsEnabled = true;
                        tabEnded.IsEnabled = true;
                        comboTriggered.IsEnabled = true;
                        textboxTimerName.IsEnabled = true;
                        break;
                    case "Repeating Timer":
                        labelTimerName.IsEnabled = true;
                        labelTimerDuration.IsEnabled = true;
                        textboxTimerHours.IsEnabled = true;
                        textboxTimerMinutes.IsEnabled = true;
                        textboxTimerSeconds.IsEnabled = true;
                        labelTimerTriggered.IsEnabled = true;
                        labelEarlyText.IsEnabled = true;
                        datagridEarly.IsEnabled = true;
                        tabEnding.IsEnabled = true;
                        tabEnded.IsEnabled = true;
                        comboTriggered.IsEnabled = true;
                        textboxTimerName.IsEnabled = true;
                        break;
                }
            }
        }

        private void checkboxFast_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            string preview = digestregex.Match(textboxSearch.Text).Groups["digest"].Value;
            sc.Content = preview;
        }
    }
}
