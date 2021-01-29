using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.DomainModels
{
    public class Course
    {
        [Key]
        public string courseid { get; set; }
        public string courseName { get; set; }
        public string courseFaculty { get; set; }
    }
}
