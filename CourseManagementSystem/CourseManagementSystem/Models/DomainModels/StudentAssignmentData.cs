using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.DomainModels
{
    public class StudentAssignmentData
    {
        [Key]
        public string stuAssessId { get; set; }
        public string studentID { get; set; }
        public string assignmentID { get; set; }
        public string assignmentMarks { get; set; }
        public string assignmentMaxmarks { get; set; }
        public string courseid { get; set; }
    }
}
