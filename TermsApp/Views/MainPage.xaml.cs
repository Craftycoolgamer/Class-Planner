using TermsApp.Entities;
using Plugin.LocalNotification;
using TermsApp.Repository;



namespace TermsApp
{ 
    public partial class MainPage : ContentPage
    {
        private static Term? selectedTerm;
        private static List<Term> terms = new List<Term>();
        public static Dictionary<Term, List<Course>> courses = new Dictionary<Term, List<Course>>();
        public static Dictionary<int, Course> courseList = new Dictionary<int, Course>();
        public static Dictionary<int, Instructor> instructors = new Dictionary<int, Instructor>();
        public static IList<NotificationRequest> notificationRequests = new List<NotificationRequest>();

        public MainPage()
        {
            InitializeComponent();

            terms.Clear();
            terms = GetSet.GetAllTerms();

            LocalDbService.CreateTables();
            LocalDbService.SeedData();
            LoadTermsUIData();

        }
        
        private void LoadTermsUIData()
        {
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
                
                button.Clicked += async (sender, args) => await Navigation.PushAsync(new TermPage(term.Id));
                TermStack.Children.Add(button);
            }
        }

        private async void AddTermClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermPage(terms.Count +1));
        }

    }
}
 