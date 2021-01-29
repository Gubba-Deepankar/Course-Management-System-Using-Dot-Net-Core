using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.Models.Database;
using CourseManagementSystem.Models.DomainModels;
using CourseManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CourseManagementSystem.Controllers
{
    [Authorize(Policy = "StudentCheck")]
    public class StudentController : Controller
    {

        private myDatabase context;

        public StudentController(myDatabase ctx)
        {
            context = ctx;
        }

        public IActionResult studentHomepage()
        {
            StudentViewModel svm = new StudentViewModel();
            svm.studentID = HttpContext.Session.GetString("ReqStudentId");
            //svm.studentID = TempData.Peek("ReqStuId").ToString(); ;
            //svm.studentID = stumodel.studentID;

            //start
            svm.thisStudent = new Student();
            svm.thisStudent = context.Students.Find(svm.studentID);
            //end
            svm.cvm = new List<CourseViewModel>();
            svm.cvm = getCourses(svm.studentID);

            return View(svm);
        }
        [HttpGet]
        public IActionResult studentRegister()
        {
            string stuid = HttpContext.Session.GetString("ReqStudentId");
            //string stuid = TempData["ReqStuId"].ToString();
            //TempData.Keep("ReqStuId");

            List<Course> coursesAvailable = null;
            coursesAvailable = context.Courses.ToList();

            ViewBag.CoursesAvailableList = coursesAvailable;

            Student thisstudent = new Student();
            thisstudent.studentID = stuid;
            return View(thisstudent);
        }

        [HttpPost]
        public IActionResult studentRegister(Student stumodel)
        {
            ViewBag.ErrCour = "";
            if (stumodel.courseids1.Equals(stumodel.courseids2) || stumodel.courseids1.Equals(stumodel.courseids3) || stumodel.courseids3.Equals(stumodel.courseids2))
            {
                ViewBag.ErrCour = "Choose 3 different courses";

                List<Course> coursesAvailable = null;
                coursesAvailable = context.Courses.ToList();

                ViewBag.CoursesAvailableList = coursesAvailable;

                return View(stumodel);
            }
            if (ModelState.IsValid)
            {
                ViewBag.ErrCour = "";
                context.Students.Add(stumodel);
                context.SaveChanges();

                List<string> courseids = new List<string>();
                courseids.Add(stumodel.courseids1); courseids.Add(stumodel.courseids2); courseids.Add(stumodel.courseids3);
                int i = 0;
                foreach (var ite in courseids)
                {
                    i++;
                    StudentCourseData thisstudent = new StudentCourseData();
                    thisstudent.stucourseID = stumodel.studentID + i.ToString();
                    thisstudent.studentID = stumodel.studentID;
                    thisstudent.courseID = ite;
                    thisstudent.courseStatus = "OnGoing";
                    thisstudent.courseGrade = "-";
                    context.StudentCoursesTable.Add(thisstudent);
                    context.SaveChanges();
                }
                //for passing values
                StudentViewModel svm = new StudentViewModel();
                svm.studentID = stumodel.studentID;

                return RedirectToAction("studentHomepage", "Student", svm);
                //return View("studentHomepage",svm);
            }
            else
            {
                List<Course> coursesAvailable = null;
                coursesAvailable = context.Courses.ToList();

                ViewBag.CoursesAvailableList = coursesAvailable;

                return View(stumodel);
            }
        }

        public IActionResult AssessmentStuList(string id)
        {
            TempData["ReqStuCourAseid"] = id;
            List<StudentAssignmentData> sadl = new List<StudentAssignmentData>();
            sadl = getAssessment();
            getCourseTeachName();
            ViewBag.sadlcount = sadl.Count();
            return View(sadl);
        }

        //basic functions
        public List<CourseViewModel> getCourses(string studentID)
        {
            List<CourseViewModel> cvm = new List<CourseViewModel>();
            List<StudentCourseData> coursesTaken = new List<StudentCourseData>();
            coursesTaken = context.StudentCoursesTable.Where(t => t.studentID.Equals(studentID)).ToList();
            foreach (var ite in coursesTaken)
            {
                List<Course> newcourse = context.Courses.Where(t => t.courseid.Equals(ite.courseID)).ToList();

                CourseViewModel thismodel = new CourseViewModel();
                thismodel.courseid = ite.courseID;
                thismodel.courseGrade = ite.courseGrade;
                thismodel.courseName = newcourse[0].courseName;

                Teacher thisTeacher = new Teacher();
                thisTeacher = getTeacher(newcourse[0].courseFaculty);
                thismodel.courseFaculty = thisTeacher.teachername;

                cvm.Add(thismodel);
            }
            return cvm;
        }
        public Teacher getTeacher(string tid)
        {
            Teacher thisTeacher = new Teacher();
            thisTeacher = context.Teachers.Find(tid);
            return thisTeacher;
        }

        public List<StudentAssignmentData> getAssessment()
        {
            string cid = TempData.Peek("ReqStuCourAseid").ToString();
            string stuid = HttpContext.Session.GetString("ReqStudentId");
            
            List<StudentAssignmentData> sadl = new List<StudentAssignmentData>();
            sadl = context.StudentAssignmentTable.Where(t => t.courseid.Equals(cid) && t.studentID.Equals(stuid)).ToList();
            
            return sadl;
        }
        public string getAseMaxMarks(string asmid)
        {
            Assessment asm = new Assessment();
            asm = context.Assessments.Find(asmid);
            return asm.assessmentMaxMarks;
        }

        public void getCourseTeachName()
        {
            Course course = new Course();
            string cid = TempData.Peek("ReqStuCourAseid").ToString();
            course = context.Courses.Find(cid);
            ViewBag.CourseName = course.courseName;
            Teacher tea = new Teacher();
            tea = getTeacher(course.courseFaculty);
            ViewBag.TeacherName = tea.teachername;
        }
    }
}