using TermsApp.Entities;
using Plugin.LocalNotification;
using TermsApp.Repository;



namespace TermsApp
{ 
    public partial class MainPage : ContentPage
    {
        //private static Term? selectedTerm;
        private static List<Term> terms = new List<Term>();
        //public static Dictionary<Term, List<Course>> courses = new Dictionary<Term, List<Course>>();
        //public static Dictionary<int, Course> courseList = new Dictionary<int, Course>();
        //public static Dictionary<int, Instructor> instructors = new Dictionary<int, Instructor>();
        public static IList<NotificationRequest> notificationRequests = new List<NotificationRequest>();

        public MainPage()
        {
            InitializeComponent();
            if (File.Exists(LocalDbService.DBPath))
            {
                LoadTermsUIData();
            }
            else
            {
                LocalDbService.SeedData();
                LoadTermsUIData();
            }
            HandleNotifications();
        }



        #region Notifications
        public static async void HandleNotifications()
        {
            //Request Notification Permission
            notificationRequests = await LocalNotificationCenter.Current.GetPendingNotificationList();
            if (!(await LocalNotificationCenter.Current.AreNotificationsEnabled()))
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            //Handle Notifications
            var requests = new List<NotificationRequest>();
            var cancelledRequests = new List<int>();

            //Process Courses
            foreach (var course in GetSet.GetAllCourses())
            {
                AddCourseNotifications(course, requests, cancelledRequests, DateTime.Now);
            }

            //Process Assessments
            foreach (var assessment in GetSet.GetAllAssessments())
            {
                AddAssessmentNotifications(assessment, requests, cancelledRequests, DateTime.Now);
            }

            //Cancel Notifications
            foreach (var requestId in cancelledRequests)
            {
                var notification = notificationRequests.FirstOrDefault(n => n.NotificationId == requestId);
                notification?.Cancel();
            }
            
            //Show Notifications
            foreach (var request in requests)
            {
                await LocalNotificationCenter.Current.Show(request);
            }
        }
        
        private static void AddCourseNotifications(Course course, List<NotificationRequest> requests, List<int> cancelledRequests, DateTime currentDateTime)
        {
            if (!course.StartNotification)
            {
                cancelledRequests.Add(course.Id + 1000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(course.Id + 1000, "Course Starting Reminder", course.Name + " Starting", course.StartDate, currentDateTime));
            }

            if (!course.EndNotification)
            {
                cancelledRequests.Add(course.Id + 2000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(course.Id + 2000, "Course Ending Reminder", course.Name + " Ending", course.EndDate, currentDateTime));
            }
        }
        private static void AddAssessmentNotifications(Assessment assessment, List<NotificationRequest> requests, List<int> cancelledRequests, DateTime currentDateTime)
        {
            if (!assessment.StartNotification)
            {
                cancelledRequests.Add(assessment.Id + 3000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(assessment.Id + 3000, "Assessment Starting Reminder", assessment.Name + " Starting", assessment.StartDate, currentDateTime));
            }

            if (!assessment.EndNotification)
            {
                cancelledRequests.Add(assessment.Id + 4000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(assessment.Id + 4000, "Assessment Ending Reminder", assessment.Name + " Ending", assessment.EndDate, currentDateTime));
            }
        }
        private static NotificationRequest CreateNotificationRequest(int notificationId, string title, string description, DateTime date, DateTime currentDateTime)
        {
            return new NotificationRequest
            {
                NotificationId = notificationId,
                Title = title,
                Description = description + " Tomorrow",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = date.AddDays(-3),
                    //NotifyTime = currentDateTime.AddSeconds(5),
                    RepeatType = NotificationRepeat.Daily
                }
            };
        }
        #endregion



        protected override void OnAppearing()
        {
            LoadTermsUIData();

            HandleNotifications();
        }

        private void LoadTermsUIData()
        {
            terms.Clear();
            terms = GetSet.GetAllTerms();
            TermStack.Children.Clear();
            
            foreach (Term term in terms)
            {
                Button button = new Button
                {
                    Text = term.Name,
                    Padding = 5,
                    //BackgroundColor = Colors.LightGreen,
                    TextColor = Colors.White,
                    CornerRadius = 5,
                };
                
                button.Clicked += async (sender, args) => await Navigation.PushAsync(new TermPage(term));
                TermStack.Children.Add(button);
            }
        }

        private async void AddTermClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermPage());
        }

    }
}
 