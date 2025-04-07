using System.ComponentModel.DataAnnotations;

namespace DynamicTime_TableGenerator.Models
{
    public class TimeTableModel
    {
        public int WorkingDays { get; set; }
        public int SubjectsPerDay { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalHours => WorkingDays * SubjectsPerDay;
        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }

    public class Subject
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }
    public class SubjectModel
    {
        public string Name { get; set; }
        public int Hours { get; set; }
    }
}
