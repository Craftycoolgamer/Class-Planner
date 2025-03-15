using TermsApp.Entities;
using Plugin.LocalNotification;
using static System.Net.Mime.MediaTypeNames;


//
//
//TODO: Course instructor cant be updated (bug)
//      
//
//


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
        public static Note? CurrentNote;


        public List<string> Stat = new List<string>
        {
            "In Progress",
            "Completed",
            "Dropped",
            "Plan To Take"
        };

	    public CoursePage(Term Term)
	    {
            CurrentTerm = Term;
		    CurrentCourse = null;
            CurrentInstructor = null;
            CurrentNote = null;
            InitializeComponent();
            StatusPicker.ItemsSource = Stat;
            StatusPicker.SelectedItem = "Plan To Take";
        }
	    public CoursePage(Course Course)
	    {
		    InitializeComponent();
		    CurrentCourse = Course;
            CurrentNote = null;
            LoadCoursesUIData(Course);
            StatusPicker.ItemsSource = Stat;
        }
        protected override void OnAppearing()
        {
            if (CurrentCourse != null)
            {
                LoadCoursesUIData(CurrentCourse);
                
                DeleteAssessment.IsEnabled = true;
                NoteDetails.IsEnabled = true;
                NoteDetails.Placeholder = "Enter Note";
                ShareNotes.IsEnabled = true;
                SaveNotes.IsEnabled = true;
                NewNote.IsEnabled = true;
                CourseStartNotify.IsEnabled = true;
            }
            else
            {
                AddAssessment.IsEnabled = false;
                DeleteAssessment.IsEnabled = false;
                NoteDetails.IsEnabled = false;
                NoteDetails.Placeholder = "Please Create Course First";
                ShareNotes.IsEnabled = false;
                SaveNotes.IsEnabled = false;
                NewNote.IsEnabled = false;
                CourseStartNotify.IsEnabled = false;
                CourseEndNotify.IsEnabled = false;
            }

            MainPage.HandleNotifications();
        }

        private void LoadCoursesUIData(Course Course)
        {
            Instructors.Clear();
            Assessments.Clear();
            Notes.Clear();
            AssessmentStack.Children.Clear();
            NoteStack.Children.Clear();

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
            CourseStartNotify.IsChecked = Course.StartNotification;
            CourseEndNotify.IsChecked = Course.EndNotification;
            InstructorName.Text = CurrentInstructor.Name;
            InstructorEmail.Text = CurrentInstructor.Email;
            InstructorPhone.Text = CurrentInstructor.Phone;
            CourseStartDate.Date = Course.StartDate;
            CourseEndDate.Date = Course.EndDate;
            StatusPicker.SelectedItem = Course.Status;
            //try{NoteDetails.Text = Notes[Notes.Count - 1].Content;}catch (Exception) { }

            foreach (Note N in Notes)
            {
                Button button = new Button
                {
                    Text = N.Content,
                    Padding = 5,
                    TextColor = Colors.White,
                    CornerRadius = 5,
                    WidthRequest = 150,
                    FontSize = 12,
                };

                button.Clicked += void (sender, args) => NoteClicked(N);
                NoteStack.Children.Add(button);
            }


            foreach (Assessment Assess in Assessments)
            {
                Button button = new Button
                {
                    Text = Assess.Name,
                    Padding = 5,
                    TextColor = Colors.White,
                    CornerRadius = 5,
                };

                button.Clicked += async (sender, args) => await Navigation.PushAsync(new AssessmentPage(Assess, Course));
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
            await Navigation.PushAsync(new AssessmentPage(CurrentCourse));
        }
        private async void SaveNoteClicked(object sender, EventArgs e)
        {
            if(CurrentNote == null)
            {
                GetSet.Insert(new Note(CurrentCourse.Id, NoteDetails.Text));
                await DisplayAlert("Note", "Note Successfully Added", "OK");
            }
            else
            {
                CurrentNote.Content = NoteDetails.Text;
                GetSet.Update(CurrentNote);
                await DisplayAlert("Note", "Note Successfully Saved", "OK");
            }
            LoadCoursesUIData(CurrentCourse);
        }
        private async void NewNoteClicked(object sender, EventArgs e)
        {
            GetSet.Insert(new Note(CurrentCourse.Id, NoteDetails.Text));
            await DisplayAlert("Note", "Note Successfully Added", "OK");
            LoadCoursesUIData(CurrentCourse);
        }
        private void NoteClicked(Note N)
        {
            CurrentNote = N;
            NoteDetails.Text = N.Content;
        }
        private async void ShareNoteClicked(object sender, EventArgs e)
        {
            await Share.Default.RequestAsync(new ShareTextRequest { Text = NoteDetails.Text });
        }


        private async void OnSave(object sender, EventArgs e)
        {
            //Validate Instructor Info is not Empty
            if (InstructorName.Text == null || InstructorEmail.Text == null || InstructorPhone.Text == null)
            {
                await DisplayAlert("Error", "Instructor Info is Empty", "OK");
                return;
            }
            Instructors = GetSet.GetAllInstructors();
            if (CurrentCourse == null)
            {
                //Check through all the Instructors to see if any names match, if not create new instructor
                CurrentInstructor = null;
                foreach (Instructor person in Instructors)
                {
                    if (person.Name == InstructorName.Text || person.Email == InstructorEmail.Text || person.Phone == InstructorPhone.Text)
                    {
                        CurrentInstructor = person;
                        break;
                    }
                }
                if (CurrentInstructor == null)
                {
                    CurrentInstructor = new Instructor(InstructorName.Text, InstructorPhone.Text, InstructorEmail.Text);
                }


                //Verify start date is before end date
                if (CourseStartDate.Date > CourseEndDate.Date)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Insert(CurrentInstructor);
                GetSet.Insert(new Course(CurrentTerm.Id, CurrentInstructor.Id, CourseName.Text, CourseStartDate.Date, CourseEndDate.Date, StatusPicker.SelectedItem.ToString(), CourseDescription.Text));
            }
            else
            {
                CurrentCourse.Name = CourseName.Text;
                CurrentCourse.StartDate = CourseStartDate.Date;
                CurrentCourse.EndDate = CourseEndDate.Date;
                CurrentCourse.Details = CourseDescription.Text;
                CurrentCourse.Status = StatusPicker.SelectedItem.ToString();
                CurrentCourse.StartNotification = CourseStartNotify.IsChecked;
                CurrentCourse.EndNotification = CourseEndNotify.IsChecked;

                //Update Instructor Info
                CurrentInstructor = null;
                foreach (Instructor person in Instructors)
                {
                    if (person.Name == InstructorName.Text || person.Email == InstructorEmail.Text || person.Phone == InstructorPhone.Text)
                    {
                        CurrentInstructor = person;
                        break;
                    }
                }
                if(CurrentInstructor == null)
                {
                    CurrentInstructor = new Instructor(InstructorName.Text, InstructorPhone.Text, InstructorEmail.Text);
                    GetSet.Insert(CurrentInstructor);
                }
                CurrentCourse.InstructorId = CurrentInstructor.Id;

                //Save Note
                if(!(NoteDetails.Text == null))
                {
                    if (CurrentNote == null)
                    {
                        GetSet.Insert(new Note(CurrentCourse.Id, NoteDetails.Text));
                        //await DisplayAlert("Note", "Note Successfully Added", "OK");
                    }
                    else
                    {
                        CurrentNote.Content = NoteDetails.Text;
                        GetSet.Update(CurrentNote);
                        //await DisplayAlert("Note", "Note Successfully Saved", "OK");
                    }
                }


                //Verify start date is before end date
                if (CurrentCourse.StartDate > CurrentCourse.EndDate)
                {
                    await DisplayAlert("Error", "Start Date is after End Date", "OK");
                    return;
                }

                GetSet.Update(CurrentCourse);
            }
            MainPage.HandleNotifications();
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