using SQLite;


namespace TermsApp.Entities
{
    [Table("Assessments")]
    public class Assessment
    {
        public Assessment() { }
        public Assessment(string type, string name, DateTime startDate, DateTime endDate, string details, int courseId)
        {
            Type = type;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Details = details;
            CourseId = courseId;
            DueDate = endDate;
        }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool StartNotification { get; set; }
        public bool EndNotification { get; set; }
        public string? Details { get; set; }
        public int CourseId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
