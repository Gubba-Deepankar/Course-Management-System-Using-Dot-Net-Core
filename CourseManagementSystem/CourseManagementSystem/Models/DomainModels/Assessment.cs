using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.DomainModels
{
    public class Assessment
    {
        [Key]
        public string Keyassessmentid { get; set; }
        public string assessmentName { get; set; }
        public string assessmentMarks { get; set; }
        public string assessmentMaxMarks { get; set; }
        public string teacherid{ get; set; }
        public string courseid { get; set; }
    }
}
