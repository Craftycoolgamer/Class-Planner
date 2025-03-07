using SQLite;
using TermsApp.Entities;

namespace TermsApp.Repository
{
    internal class LocalDbService
    {
        public static string DBPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "TermsDatabase.db"
        );

        public static void SeedData()
        {
            File.Delete(DBPath);
            CreateTables();


            // Insert Terms
            List<Term> terms = new List<Term>
            {
                new Term("Term 1", DateTime.Now, DateTime.Now.AddMonths(6)),
                new Term("Term 2", DateTime.Now.AddMonths(6), DateTime.Now.AddMonths(12)),
            };
            foreach (var term in terms)
            {
                GetSet.Insert(term);
            }

            //Insert Instructors
            List<Instructor> instructors = new List<Instructor>
            {
                new Instructor("Anika Patel", "555-123-4567", "anika.patel@strimeuniversity.edu"),
            };
            foreach (var i in instructors)
            {
                GetSet.Insert(i);
            }

            // Insert Courses
            List<Course> courses = new List<Course>
            {
                //term1
                new Course(1, 1, "Course 1", DateTime.Now, DateTime.Now.AddMonths(4), "Completed", "Enter Course Details Here:"),
                new Course(1, 1, "Course 2", DateTime.Now, DateTime.Now.AddMonths(5), "Dropped", "Enter Course Details Here:"),
                new Course(1, 1, "Course 3", DateTime.Now, DateTime.Now.AddMonths(3), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 4", DateTime.Now, DateTime.Now.AddMonths(2), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 5", DateTime.Now, DateTime.Now.AddMonths(6), "Plan To Take", "Enter Course Details Here:"),
                new Course(1, 1, "Course 6", DateTime.Now, DateTime.Now.AddMonths(9), "Plan To Take", "Enter Course Details Here:"),

                //term2
                new Course(2, 1, "Course 1", DateTime.Now, DateTime.Now.AddMonths(7), "Plan To Take", "Enter Course Details Here:"),
                new Course(2, 1, "Course 2", DateTime.Now, DateTime.Now.AddMonths(3), "Plan To Take", "Enter Course Details Here:"),
                new Course(2, 1, "Course 3", DateTime.Now, DateTime.Now.AddMonths(4), "Plan To Take", "Enter Course Details Here:"),
                new Course(2, 1, "Course 4", DateTime.Now, DateTime.Now.AddMonths(6), "Plan To Take", "Enter Course Details Here:"),
                new Course(2, 1, "Course 5", DateTime.Now, DateTime.Now.AddMonths(2), "Dropped", "Enter Course Details Here:"),
                new Course(2, 1, "Course 6", DateTime.Now, DateTime.Now.AddMonths(1), "In Progress", "Enter Course Details Here:"),
            };
            foreach (var course in courses)
            {
                GetSet.Insert(course);
            }

            courses = GetSet.GetAllCoursesByTerm(1);

            foreach (var course in courses)
            {
                //Assessments
                GetSet.Insert(new Assessment(1, "Performance Assessment #1", DateTime.Now, DateTime.Now.AddMonths(3), "Enter details about assessment here:", course.Id));
                GetSet.Insert(new Assessment(0, "Objective Assessment #1", DateTime.Now, DateTime.Now.AddMonths(3), "Enter details about assessment here:", course.Id));
                
                //Notes
                GetSet.Insert(new Note(course.Id, "Test note"));
                GetSet.Insert(new Note(course.Id, "Test note 2"));
            }

        }

        public static void CreateTables()
        {
            using (SQLiteConnection connection = new(DBPath))
            {
                connection.CreateTable<Term>();
                connection.CreateTable<Course>();
                connection.CreateTable<Instructor>();
                connection.CreateTable<Assessment>();
                connection.CreateTable<Note>();
            }
        }
    }
}
