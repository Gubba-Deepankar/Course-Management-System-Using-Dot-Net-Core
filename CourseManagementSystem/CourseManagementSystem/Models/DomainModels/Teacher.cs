using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.DomainModels
{
    public class Teacher
    {
        [MaxLength(9)]
        public string teacherid { get; set; }
        public string teachername { get; set; }
        public string courseid { get; set; }
        public Course CourseofTeacher { get; set; }
    }
}
