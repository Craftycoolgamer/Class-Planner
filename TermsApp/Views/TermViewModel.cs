using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TermsApp
{
    class TermViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Courses { get; set; }
        private string _selectedCourse;

        public string SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                if (_selectedCourse != value)
                {
                    _selectedCourse = value;
                    OnPropertyChanged(nameof(SelectedCourse));
                }
            }
        }

        public TermViewModel()
        {
            Courses = new ObservableCollection<string> { "Course 1", "Course 2", "Course 3", "Course 4", "Course 5", "Course 6" };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

