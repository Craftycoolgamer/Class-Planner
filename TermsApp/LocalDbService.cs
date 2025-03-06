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
                new Course(1, 1, "Course 1", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 2", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 3", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 4", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 5", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(1, 1, "Course 6", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),

                //term2
                new Course(2, 1, "Course 1", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(2, 1, "Course 2", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(2, 1, "Course 3", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(2, 1, "Course 4", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(2, 1, "Course 5", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
                new Course(2, 1, "Course 6", DateTime.Now, DateTime.Now.AddMonths(4), "In Progress", "Enter Course Details Here:"),
            };
            foreach (var course in courses)
            {
                GetSet.Insert(course);

                //TODO: Add assessments here and notes

            }

            //courses = GetSet.GetAllCoursesByTerm(1);
            
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
