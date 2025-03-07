using SQLite;
using TermsApp.Repository;
using TermsApp.Entities;


namespace TermsApp
{
    class GetSet
    {
        public static bool Insert<TEntity>(TEntity entity)
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    connection.Insert(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inserting Data: {ex.Message}");
                return false;
            }
        }
        public static bool Update<TEntity>(TEntity entity)
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    connection.Update(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Updating Data: {ex.Message}");
                return false;
            }
        }

        public static bool Delete<TEntity>(TEntity entity)
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    connection.Delete(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Deleting Data: {ex.Message}");
                return false;
            }
        }

        //Terms
        public static bool AddNewTerm()
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    var query = connection.Query<Term>($"SELECT * FROM Terms ORDER BY Id DESC LIMIT 1");
                    Term latestTerm = query.First();
                    string termName = "Term " + (latestTerm.Id + 1).ToString();

                    Term newTerm = new(termName, DateTime.Now, DateTime.Now.AddDays(60));
                    Insert(newTerm);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static List<Term> GetAllTerms()
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    return connection.Query<Term>("SELECT * FROM Terms").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving terms: {ex.Message}");
                return [];
            }
        }

        //Courses
        public static List<Course> GetAllCoursesByTerm(int termId)
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    return connection.Query<Course>($"SELECT * FROM Courses WHERE TermId={termId}").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Courses: {ex.Message}");
                return [];
            }
        }
        public static bool AddNewCourse(int termId)
        {
            try
            {
                Course course = new Course(termId, 1, "New Course", DateTime.Now, DateTime.Now.AddMonths(4), "Plan to Take", "Enter Course Details Here:");
                Insert(course);
                //MainPage.SyncDatabaseFields();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Instructors
        public static List<Instructor> GetAllInstructors()
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    return connection.Query<Instructor>("SELECT * FROM Instructors").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Instructors: {ex.Message}");
                return [];
            }
        }

        //Assessments
        public static List<Assessment> GetAssessmentsByCourse(int courseId)
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    return connection.Query<Assessment>($"SELECT * FROM Assessments WHERE CourseId={courseId}").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Assessments: {ex.Message}");
                return [];
            }
        }
        public static List<Assessment> GetAllAssessments()
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    return connection.Query<Assessment>("SELECT * FROM Assessments").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving All Assessments: {ex.Message}");
                return [];
            }
        }

        //Notes
        public static List<Note> GetNotesByCourse(int courseId)
        {
            try
            {
                using (SQLiteConnection connection = new(LocalDbService.DBPath))
                {
                    return connection.Query<Note>($"SELECT * FROM Notes WHERE CourseId={courseId}").ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving Notes: {ex.Message}");
                return [];
            }
        }
    }
}
