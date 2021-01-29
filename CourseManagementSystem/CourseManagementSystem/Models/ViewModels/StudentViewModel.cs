using CourseManagementSystem.Models.Database;
using CourseManagementSystem.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.ViewModels
{
    public class StudentViewModel
    {
        public string studentID { get; set; }
        public Student thisStudent { get; set; }
        public List<StudentCourseData> coursesTaken{ get; set; }
        public List<Course> courseDetails { get; set; }
        public List<CourseViewModel> cvm { get; set; }

        /*
        [TempData]
        public string sturouteid { get; set; }
        */
    }
}
