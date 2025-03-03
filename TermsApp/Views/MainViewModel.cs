using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace TermsApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Terms { get; set; }
        private string _selectedTerm;

        public string SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                if (_selectedTerm != value)
                {
                    _selectedTerm = value;
                    OnPropertyChanged(nameof(SelectedTerm));
                }
            }
        }

        public ICommand TermCommand { get; }

        public MainViewModel()
        {
            Terms = new ObservableCollection<string> { "Term 1", "Term 2", "Term 3", "Term 4" };
            //TermCommand = new Command<string>(OnTermSelected);
            //LoadButtons();
        }

        //public void LoadButtons()
        //{
        //    Terms = Terms;
        //}

        //private void OnTermSelected(string buttonText)
        //{
        //    SelectedTerm = buttonText;
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
