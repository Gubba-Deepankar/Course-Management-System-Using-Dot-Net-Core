using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.DomainModels
{
    public class StudentCourseData
    {
        [Key]
        public string stucourseID { get; set; }
        public string studentID { get; set; }
        public string courseID { get; set; }
        public string courseGrade { get; set; }
        public string courseStatus { get; set; }
    }
}
