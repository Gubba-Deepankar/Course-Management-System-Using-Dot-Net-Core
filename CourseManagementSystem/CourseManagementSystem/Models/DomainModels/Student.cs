using CourseManagementSystem.Models.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.DomainModels
{
    public class Student
    {
        [Key]
        [Required]
        public string studentID { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public string studentname { get; set; }
        [Required (ErrorMessage = "This is a required field")]
        [EmailAddress]
        public string emailaddr { get; set; }

        [Required(ErrorMessage = "This is a required field")]
        public string major { get; set; }
        //public List<Course> coursestaken { get; set; }

        [Required(ErrorMessage = "Choose a course")]
        public string courseids1 { get; set; }
        [Required(ErrorMessage = "Choose another course")]
        public string courseids2 { get; set; }
        [Required(ErrorMessage = "choose another course")]
        public string courseids3 { get; set; }

    }
}
