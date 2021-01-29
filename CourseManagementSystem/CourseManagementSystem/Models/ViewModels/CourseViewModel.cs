using CourseManagementSystem.Models.Database;
using CourseManagementSystem.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.ViewModels
{
    public class CourseViewModel
    {
        public string courseid { get; set; }
        public string courseName { get; set; }
        public string courseFaculty { get; set; }
        public string courseGrade { get; set; }
        public string courseStatus { get; set; }

    }
}
