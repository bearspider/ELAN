using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQAudioTriggers.Models
{
    public class CategoryWrapper : INotifyPropertyChanged
    {
        private Category _categoryitem;
        private ObservableCollection<OverlayText> _overlaytexts;
        private ObservableCollection<OverlayTimer> _overlaytimers;
        //private CharacterOverride _selectedoverride;
        public Category CategoryItem { get { return _categoryitem; } set { _categoryitem = value; NotifyPropertyChanged("CategoryItem"); } }

        public ObservableCollection<OverlayText> OverlayTexts { get { return _overlaytexts; } set { _overlaytexts = value; NotifyPropertyChanged("OverlayTexts"); } }
        public ObservableCollection<OverlayTimer> OverlayTimers { get { return _overlaytimers; } set { _overlaytimers = value; NotifyPropertyChanged("OverlayTimers"); } }

        //public CharacterOverride SelectedOverride { get { return _selectedoverride; } set { _selectedoverride = value; NotifyPropertyChanged("SelectedOverride"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            Console.WriteLine($"Modified: {propName}");
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public CategoryWrapper()
        {
            CategoryItem = new Category();
            OverlayTexts = new ObservableCollection<OverlayText>();
            OverlayTimers = new ObservableCollection<OverlayTimer>();
            //SelectedOverride = new CharacterOverride();
        }
    }
}
