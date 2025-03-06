using TermsApp.Entities;
using Plugin.LocalNotification;
using TermsApp.Repository;


namespace TermsApp
{
	public partial class TermPage : ContentPage
	{
        public static List<Course> courses = new List<Course>();
        //public static Dictionary<int, Course> courseList = new Dictionary<int, Course>();
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
            

            //TODO: Refresh Notifications
        }

        private void LoadCoursesUIData(Term T)
        {
            courses.Clear();
            CourseStack.Children.Clear();
            courses = GetSet.GetAllCoursesByTerm(T.Id);
            CurrentTerm = T;
            TermTitleEntry.Text = T.Name;
            TermStartDate.Date = T.StartDate;
            TermEndDate.Date = T.EndDate;

            foreach (Course course in courses)
            {
                Button button = new Button
                {
                    Text = course.Name,
                    Padding = 5,
                    TextColor = Colors.Black,
                    CornerRadius = 5,
                };

                //button.Clicked += async (sender, args) => await Navigation.PushAsync(new CoursePage(course.Id));
                CourseStack.Children.Add(button);
            }
            if (courses.Count >= 6)
            {
                AddCourse.IsEnabled = false;
                
            }

        }

        private async void AddCourseClicked(object sender, EventArgs e)
		{
            //await Navigation.PushModalAsync(new MainPage());
            await Navigation.PopModalAsync();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            
            if (CurrentTerm == null)
            {
                GetSet.Insert(new Term(TermTitleEntry.Text, TermStartDate.Date, TermEndDate.Date));
            }
            else
            {
                CurrentTerm.Name = TermTitleEntry.Text;
                CurrentTerm.StartDate = TermStartDate.Date;
                CurrentTerm.EndDate = TermEndDate.Date;
                GetSet.Update(CurrentTerm);
            }
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            GetSet.Delete(CurrentTerm);
            await Navigation.PopAsync();
        }
    }
}