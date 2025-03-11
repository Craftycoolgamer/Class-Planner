using TermsApp.Entities;
using Plugin.LocalNotification;
using TermsApp.Repository;



namespace TermsApp
{
	public partial class TermPage : ContentPage
	{
        public static List<Course> courses = new List<Course>();
        public static Term? CurrentTerm;

        public TermPage()
        {
            CurrentTerm = null;
            InitializeComponent();
        }
        public TermPage(Term T)
		{
			InitializeComponent();
            CurrentTerm = T;
            LoadCoursesUIData(T);
        }
        protected override void OnAppearing()
        {
            if (CurrentTerm != null)
            {
                LoadCoursesUIData(CurrentTerm);
            }
            else
            {
                AddCourse.IsEnabled = false;
                DeleteTerm.IsEnabled = false;
            }

            MainPage.SyncDatabaseFields();
        }

        private void LoadCoursesUIData(Term T)
        {
            courses.Clear();
            CourseStack.Children.Clear();
            courses = GetSet.GetAllCoursesByTerm(T.Id);
            CurrentTerm = T;
            TermTitle.Text = T.Name;
            TermStartDate.Date = T.StartDate;
            TermEndDate.Date = T.EndDate;

            foreach (Course course in courses)
            {
                Button button = new Button
                {
                    Text = course.Name,
                    Padding = 5,
                    TextColor = Colors.White,
                    CornerRadius = 5,
                };


                button.Clicked += async (sender, args) => await Navigation.PushAsync(new CoursePage(course));
                
                CourseStack.Children.Add(button);
            }
            if (courses.Count >= 6)
            {
                AddCourse.IsEnabled = false;
                
            }
            else
            {
                AddCourse.IsEnabled = true;
            }

        }

        private async void AddCourseClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(new CoursePage(CurrentTerm));
        }

        private async void OnSave(object sender, EventArgs e)
        {
            if (CurrentTerm == null)
            {
                //Verify start date is before end date
                if (TermStartDate.Date > TermEndDate.Date)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Insert(new Term(TermTitle.Text, TermStartDate.Date, TermEndDate.Date));
            }
            else
            {
                CurrentTerm.Name = TermTitle.Text;
                CurrentTerm.StartDate = TermStartDate.Date;
                CurrentTerm.EndDate = TermEndDate.Date;

                //Verify start date is before end date
                if (CurrentTerm.StartDate > CurrentTerm.EndDate)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Update(CurrentTerm);
            }
            MainPage.SyncDatabaseFields();
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete", "Are you sure you want to delete this course?", "Yes", "No");
            if (confirm)
            {
                GetSet.Delete(CurrentTerm);

                await Navigation.PopAsync();
            }
        }
    }
}