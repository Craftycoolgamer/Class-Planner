using TermsApp.Entities;
using Plugin.LocalNotification;
using TermsApp.Repository;


namespace TermsApp
{
	public partial class TermPage : ContentPage
	{
        public static List<Course> courses = new List<Course>();
        public static Dictionary<int, Course> courseList = new Dictionary<int, Course>();

        public TermPage(int TermId)
		{
			InitializeComponent();

            LoadCoursesUIData(TermId);

            //TODO: Fill in term data
        }

        private void LoadCoursesUIData(int TermId)
        {
            courses.Clear();
            courses = GetSet.GetAllCoursesByTerm(TermId);

            CourseStack.Children.Clear();

            foreach (Course course in courses)
            {
                Grid grid = new Grid
                {
                    BackgroundColor = Colors.White
                };
                Button button = new Button
                {
                    Text = course.Name,
                    BackgroundColor = Color.FromArgb("#1b7d2d"),
                };
                //button.Clicked += async (sender, args) => await Navigation.PushAsync(new CoursePage(course.Id));

                grid.Add(button);

                SwipeView swp = new SwipeView
                {
                    Content = grid
                };
                CourseStack.Children.Add(swp);
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

        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}