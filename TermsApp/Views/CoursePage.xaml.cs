using TermsApp.Entities;
using Plugin.LocalNotification;

namespace TermsApp 
{ 
    public partial class CoursePage : ContentPage
    {
        public static List<Instructor> Instructors = new List<Instructor>();
        public static List<Assessment> Assessments = new List<Assessment>();
	    public static List<Note> Notes = new List<Note>();
        public static Term? CurrentTerm;
        public static Course? CurrentCourse;
        public static Instructor? CurrentInstructor;

	    public CoursePage(Term Term)
	    {
            CurrentTerm = Term;
		    CurrentCourse = null;
		    InitializeComponent();
	    }
	    public CoursePage(Course Course)
	    {
		    InitializeComponent();
		    CurrentCourse = Course;
            LoadCoursesUIData(Course);
        }
        protected override void OnAppearing()
        {
            if (CurrentCourse != null)
            {
                LoadCoursesUIData(CurrentCourse);
            }
            else
            {
                AddAssessment.IsEnabled = false;
                DeleteAssessment.IsEnabled = false;
                NoteDetails.IsEnabled = false;
                ShareNotes.IsEnabled = false;
                SaveNotes.IsEnabled = false;
            }

            //TODO: Refresh Notifications
        }

        private void LoadCoursesUIData(Course Course)
        {
            Instructors.Clear();
            Assessments.Clear();
            Notes.Clear();
            AssessmentStack.Children.Clear();

            Instructors = GetSet.GetAllInstructors();
            Assessments = GetSet.GetAssessmentsByCourse(Course.Id);
            Notes = GetSet.GetNotesByCourse(Course.Id);
            
            CurrentCourse = Course;
            foreach (Instructor person in Instructors)
            {
                if(person.Id == CurrentCourse.InstructorId)
                {
                    CurrentInstructor = person;
                }
            }

            //Fill out Course Info
            CourseName.Text = Course.Name;
            CourseDescription.Text = Course.Details;
            InstructorName.Text = CurrentInstructor.Name;
            InstructorEmail.Text = CurrentInstructor.Email;
            InstructorPhone.Text = CurrentInstructor.Phone;
            CourseStartDate.Date = Course.StartDate;
            CourseEndDate.Date = Course.EndDate;

            //TODO: IDK whats wrong with this
            ////NoteDetails.Text = Notes[Notes.Count-1].Content;
            //NoteDetails.Text = ;

            //TODO: Add picker for status (in progress, completed, dropped, plan to take)
            //      Add saving validation to prevent null values


            foreach (Assessment Assess in Assessments)
            {
                Button button = new Button
                {
                    Text = Assess.Name,
                    Padding = 5,
                    TextColor = Colors.Black,
                    CornerRadius = 5,
                };

                //button.Clicked += async (sender, args) => await Navigation.PushAsync(new AssessmentPage(Assess.Id));
                AssessmentStack.Children.Add(button);
            }
            if (Assessments.Count >= 2)
            {
                AddAssessment.IsEnabled = false;
            }
            else
            {
                AddAssessment.IsEnabled = true;
            }
        }

        private async void AddAssessmentClicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new MainPage());
            await Navigation.PopAsync();
        }
        private async void SaveNoteClicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new MainPage());
            await Navigation.PopAsync();
        }

        private async void ShareNoteClicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new MainPage());
            await Navigation.PopAsync();
        }


        private async void OnSave(object sender, EventArgs e)
        {
            if (CurrentCourse == null)
            {
                //TODO: Check through all the instructors for names matching and fill in the ID from that
                //              Just make a new Instructor for every new course?

                Course TempCourse = new Course(CurrentTerm.Id, CurrentInstructor.Id, CourseName.Text, 
                                               CourseStartDate.Date, CourseEndDate.Date, "In Progress", CourseDescription.Text);

                //Verify start date is before end date
                if (TempCourse.StartDate > TempCourse.EndDate)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Insert(TempCourse);
            }
            else
            {
                CurrentCourse.Name = CourseName.Text;
                CurrentCourse.StartDate = CourseStartDate.Date;
                CurrentCourse.EndDate = CourseEndDate.Date;
                CurrentCourse.Details = CourseDescription.Text;
                //CurrentCourse.Status = "Active";

                //Verify start date is before end date
                if (CurrentCourse.StartDate > CurrentCourse.EndDate)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Update(CurrentCourse);
            }
            await Navigation.PopAsync();
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete", "Are you sure you want to delete this course?", "Yes", "No");
            if (confirm)
            {
                GetSet.Delete(CurrentCourse);
                await Navigation.PopAsync();
            }
        }

    }
}