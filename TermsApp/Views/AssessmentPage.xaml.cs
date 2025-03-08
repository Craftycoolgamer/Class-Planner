using Microsoft.Maui.Controls;
using TermsApp.Entities;

namespace TermsApp
{
    public partial class AssessmentPage : ContentPage
    {
        private static Assessment? CurrentAssessment;
        public static Course? CurrentCourse;

        public List<string> Types = new List<string>
        {
            "Performance",
            "Objective"
        };

        public AssessmentPage(Course course)
        {
            CurrentAssessment = null;
            CurrentCourse = course;
            InitializeComponent();
            TypePicker.ItemsSource = Types;
            TypePicker.SelectedItem = "Performance";
            AssessmentStartNotify.IsEnabled = false;
            AssessmentEndNotify.IsEnabled = false;
        }

        public AssessmentPage(Assessment A)
        {
            InitializeComponent();
            CurrentAssessment = A;
            AssessmentName.Text = A.Name;
            AssessmentStartDate.Date = A.StartDate;
            AssessmentEndDate.Date = A.EndDate;
            AssessmentStartNotify.IsChecked = A.StartNotification;
            AssessmentEndNotify.IsChecked = A.EndNotification;
            TypePicker.ItemsSource = Types;
            TypePicker.SelectedItem = A.Type;
            AssessmentStartNotify.IsEnabled = true;
            AssessmentEndNotify.IsEnabled = true;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if(CurrentAssessment == null)
            {
                //Verify start date is before end date
                if (AssessmentStartDate.Date > AssessmentEndDate.Date)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Insert(new Assessment(TypePicker.SelectedItem.ToString(), AssessmentName.Text, AssessmentStartDate.Date, AssessmentEndDate.Date, "", CurrentCourse.Id));
            }
            else
            {
                CurrentAssessment.Name = AssessmentName.Text;
                CurrentAssessment.StartDate = AssessmentStartDate.Date;
                CurrentAssessment.EndDate = AssessmentEndDate.Date;
                CurrentAssessment.StartNotification = AssessmentStartNotify.IsChecked;
                CurrentAssessment.EndNotification = AssessmentEndNotify.IsChecked;
                CurrentAssessment.Type = TypePicker.SelectedItem.ToString();

                //Verify start date is before end date
                if (AssessmentStartDate.Date > AssessmentEndDate.Date)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Update(CurrentAssessment);
            }


            //await DisplayAlert("Save", "Assessment details saved!", "OK");
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (confirm)
            {
                GetSet.Delete(CurrentAssessment);
                await Navigation.PopAsync();
            }
        }
    }
}
