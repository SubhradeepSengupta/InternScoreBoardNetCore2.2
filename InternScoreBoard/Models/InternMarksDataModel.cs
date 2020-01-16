using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternScoreBoard.Models
{
    public class InternMarksDataModel
    {
        public string CourseName { get; set; }
        public List<string> ColumnNames { get; set; }
        public List<List<string>> InternNameAndScores { get; set; }
    }

    public class InternDetails
    {
        public string InternName { get; set; }
        public string Email { get; set; }
        public List<CourseMarks> Courses { get; set; }
        public double AllOverScore { get; set; }
    }

    public class CourseMarks
    {
        public string CourseName { get; set; }
        public string Percentage { get; set; }
    }
}
