using TermsApp.Entities;
using Plugin.LocalNotification;
using TermsApp.Repository;



namespace TermsApp
{ 
    public partial class MainPage : ContentPage
    {
        //private static Term? selectedTerm;
        private static List<Term> terms = new List<Term>();
        public static Dictionary<Term, List<Course>> courses = new Dictionary<Term, List<Course>>();
        public static Dictionary<int, Course> courseList = new Dictionary<int, Course>();
        public static Dictionary<int, Instructor> instructors = new Dictionary<int, Instructor>();
        public static IList<NotificationRequest> notificationRequests = new List<NotificationRequest>();

        public MainPage()
        {
            terms = GetSet.GetAllTerms();
            InitializeComponent();
            LocalDbService.CreateTables();
            LocalDbService.SeedData();
            LoadTermsUIData();
        }

        #region Notifications
        private async void HandleNotifications()
        {
            //Request Notification Permission
            notificationRequests = await LocalNotificationCenter.Current.GetPendingNotificationList();
            if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            //Handle Notifications
            var requests = new List<NotificationRequest>();
            var cancelledRequests = new List<int>();
            DateTime currentDateTime = DateTime.Now;

            ProcessCourses(courses.Values, requests, cancelledRequests, currentDateTime);
            ProcessAssessments(GetSet.GetAllAssessments(), requests, cancelledRequests, currentDateTime);

            CancelNotifications(cancelledRequests, notificationRequests.ToList());
            await ShowNotifications(requests);
        }
        private void ProcessCourses(IEnumerable<List<Course>> courseLists, List<NotificationRequest> requests, List<int> cancelledRequests, DateTime currentDateTime)
        {
            foreach (var listCourse in courseLists)
            {
                foreach (var course in listCourse)
                {
                    AddCourseNotifications(course, requests, cancelledRequests, currentDateTime);
                }
            }
        }
        private void AddCourseNotifications(Course course, List<NotificationRequest> requests, List<int> cancelledRequests, DateTime currentDateTime)
        {
            if (course.StartNotification == 0)
            {
                cancelledRequests.Add(course.Id + 1000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(course.Id + 1000, "Course Starting Reminder", course.Name, course.StartDate, course.StartNotification, currentDateTime));
            }

            if (course.EndNotification == 0)
            {
                cancelledRequests.Add(course.Id + 2000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(course.Id + 2000, "Course Ending Reminder", course.Name, course.EndDate, course.EndNotification, currentDateTime));
            }
        }
        private void ProcessAssessments(IEnumerable<Assessment> assessments, List<NotificationRequest> requests, List<int> cancelledRequests, DateTime currentDateTime)
        {
            foreach (var assessment in assessments)
            {
                AddAssessmentNotifications(assessment, requests, cancelledRequests, currentDateTime);
            }
        }
        private void AddAssessmentNotifications(Assessment assessment, List<NotificationRequest> requests, List<int> cancelledRequests, DateTime currentDateTime)
        {
            if (assessment.StartNotification == 0)
            {
                cancelledRequests.Add(assessment.Id + 3000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(assessment.Id + 3000, "Assessment Starting Reminder", assessment.Name, assessment.StartDate, assessment.StartNotification, currentDateTime));
            }

            if (assessment.EndNotification == 0)
            {
                cancelledRequests.Add(assessment.Id + 4000);
            }
            else
            {
                requests.Add(CreateNotificationRequest(assessment.Id + 4000, "Assessment Ending Reminder", assessment.Name, assessment.EndDate, assessment.EndNotification, currentDateTime));
            }
        }
        private NotificationRequest CreateNotificationRequest(int notificationId, string title, string description, DateTime date, int notificationDaysBefore, DateTime currentDateTime)
        {
            return new NotificationRequest
            {
                NotificationId = notificationId,
                Title = title,
                Description = description + " Starting soon",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = date.AddDays(-notificationDaysBefore).AddHours(currentDateTime.Hour).AddMinutes(currentDateTime.Minute + 1),
                    RepeatType = NotificationRepeat.Daily
                }
            };
        }
        private void CancelNotifications(List<int> cancelledRequests, List<NotificationRequest> notificationRequests)
        {
            foreach (var requestId in cancelledRequests)
            {
                var notification = notificationRequests.FirstOrDefault(n => n.NotificationId == requestId);
                notification?.Cancel();
            }
        }
        private async Task ShowNotifications(List<NotificationRequest> requests)
        {
            foreach (var request in requests)
            {
                await LocalNotificationCenter.Current.Show(request);
            }
        }
        #endregion

        protected override void OnAppearing()
        {
            if (terms.Count > 0)
            {
                LoadTermsUIData();
            }

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
                    TextColor = Colors.Black,
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
 