using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.Models.Database;
using CourseManagementSystem.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Authorize(Policy = "TeacherCheck")]
    public class TeacherController : Controller
    {
        private myDatabase context;

        public TeacherController(myDatabase ctx)
        {
            context = ctx;
        }
        public IActionResult TeacherHomePage()
        {
            Teacher tea = new Teacher();
            string tid = HttpContext.Session.GetString("ReqTeacherId");
            tea = context.Teachers.Find(tid);

            List<Course> coursesUnder = new List<Course>();
            coursesUnder = context.Courses.Where(t => t.courseFaculty.Equals(tea.teacherid)).ToList();
            ViewBag.coursesList = coursesUnder;
            return View(tea);
        }
        public IActionResult StuGradeList(string cid)
        {
            TempData["ReqCourid"] = cid;
            List<StudentCourseData> ScdList = new List<StudentCourseData>();
            ScdList = context.StudentCoursesTable.Where(t => t.courseID.Equals(cid)).ToList();
            ViewBag.scdlcount = ScdList.Count();
            return View(ScdList);
        }

        [HttpGet]
        public IActionResult UpdateStuGrade(string stuid)
        {
            string cid = TempData.Peek("ReqCourid").ToString();
            List<StudentCourseData> ScdList = new List<StudentCourseData>();
            StudentCourseData Scd = new StudentCourseData();
            ScdList = context.StudentCoursesTable.Where(t => t.courseID.Equals(cid) && t.studentID.Equals(stuid)).ToList();
            Scd = ScdList[0];
            return View(Scd);
        }
        [HttpPost]
        public IActionResult UpdateStuGrade(StudentCourseData scd)
        {
            context.StudentCoursesTable.Update(scd);
            context.SaveChanges();
            return RedirectToAction("StuGradeList", "Teacher", new { cid = scd.courseID});
        }
    }
}