using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagementSystem.Models.Database;
using CourseManagementSystem.Models.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    [Authorize(Policy = "TeacherCheck")]
    public class AssessmentController : Controller
    {
        private myDatabase context;

        public AssessmentController(myDatabase ctx)
        {
            context = ctx;
        }
        public IActionResult AssessmentList(string courseid)
        {
            TempData["ReqCourseid"] = courseid;

            List<Assessment> assignmentList = new List<Assessment>();
            assignmentList = context.Assessments.Where(t => t.courseid.Equals(courseid)).ToList();

            ViewBag.asmcount = assignmentList.Count();
            getCourseTeachName();
            return View(assignmentList);
        }

        [HttpGet]
        public IActionResult AddAssessment()
        {
            string courseid = TempData.Peek("ReqCourseid").ToString();
            Course cse = new Course();
            cse = context.Courses.Find(courseid);

            getCourseTeachName();

            Assessment ase = new Assessment();
            ase.courseid = courseid;
            ase.teacherid = cse.courseFaculty;
            return View(ase);
        }
        [HttpPost]
        public IActionResult AddAssessment(Assessment ast)
        {
            if (ModelState.IsValid)
            {
                // Checking if assessment already exists
                string courseid = TempData.Peek("ReqCourseid").ToString();
                
                int asecheckcount = context.Assessments.Where(t => t.assessmentName.Equals(ast.assessmentName) && t.courseid.Equals(courseid)).Count();
                if (asecheckcount != 0)
                {
                    ViewBag.ExistErr = "Assessment already exists. Add new";
                    return View(ast);
                }

                ViewBag.ExistErr = "";
                int astcount = context.Assessments.ToList().Count();
                ast.Keyassessmentid = (astcount + 1).ToString();
                context.Assessments.Add(ast);
                context.SaveChanges();

                List<StudentCourseData> scd = new List<StudentCourseData>();
                scd = context.StudentCoursesTable.Where(t => t.courseID.Equals(ast.courseid)).ToList();

                //List<Student> stulist = new List<Student>();
                Student thisStudent = new Student();
                foreach (var ite in scd)
                {
                    StudentAssignmentData sad = new StudentAssignmentData();
                    //stulist.Add(context.Students.Find(ite.studentID));
                    thisStudent = context.Students.Find(ite.studentID);
                    sad.studentID = thisStudent.studentID;
                    sad.assignmentID = ast.assessmentName;
                    sad.courseid = ast.courseid;
                    sad.assignmentMarks = "-";
                    sad.assignmentMaxmarks = ast.assessmentMaxMarks;
                    int lenite = context.StudentAssignmentTable.Count();

                    sad.stuAssessId = (lenite + 1).ToString();


                    context.StudentAssignmentTable.Add(sad);
                    context.SaveChanges();
                }

                return RedirectToAction("AssessmentList", "Assessment", new {courseid = ast.courseid });
            }
            else
            {
                return View(ast);
            }
        }

        //[HttpGet]
        public IActionResult StuAssessList(string reqid)
        {
            /*
            string[] abc = new string[2];
            abc = reqid.Split(",");
            thatreqid = reqid;

            string asmid = abc[0]; //assessment id
            string cid = abc[1]; //course id*/

            string cid = TempData.Peek("ReqCourseid").ToString();
            TempData["ReqAseid"] = reqid;
            List<StudentAssignmentData> sad = new List<StudentAssignmentData>();
            sad = context.StudentAssignmentTable.Where(t => t.assignmentID.Equals(reqid) && t.courseid.Equals(cid)).ToList();

            //for obtaining student name
            //can be omitted if name is not required
            getCourseTeachName();
            ViewBag.sadcount = sad.Count();
            
            return View(sad);
        }
        /*
        [HttpPost]
        public IActionResult StuAssessList(List<StudentAssignmentData> sad)
        {
            context.StudentAssignmentTable.UpdateRange(sad);
            context.SaveChanges();
            return View(sad);
        }*/


        [HttpGet]
        public IActionResult UpdateStuMarks(string stuid)
        {
            string cid = TempData.Peek("ReqCourseid").ToString();
            string aseid = TempData.Peek("ReqAseid").ToString();
            List<StudentAssignmentData> sadList = context.StudentAssignmentTable.Where(t => t.assignmentID.Equals(aseid) && t.courseid.Equals(cid) && t.studentID.Equals(stuid)).ToList();
            StudentAssignmentData sad = new StudentAssignmentData();
                sad = sadList[0];
            return View(sad);
        }

        [HttpPost]
        public IActionResult UpdateStuMarks(StudentAssignmentData sad)
        {
            context.StudentAssignmentTable.Update(sad);
            context.SaveChanges();
            return RedirectToAction("StuAssessList", "Assessment", new { reqid = sad.assignmentID });
        }

        public IActionResult DeleteAssessment(string daid)
        {
            string cid = TempData.Peek("ReqCourseid").ToString();

            Assessment ase = new Assessment();
            ase = context.Assessments.Where(t => t.assessmentName.Equals(daid) && t.courseid.Equals(cid)).Single();

            List<StudentAssignmentData> sadl = new List<StudentAssignmentData>();
            sadl = context.StudentAssignmentTable.Where(t => t.assignmentID.Equals(daid) && t.courseid.Equals(cid)).ToList();

            context.Assessments.Remove(ase);
            context.StudentAssignmentTable.RemoveRange(sadl);
            context.SaveChanges();

            return RedirectToAction("AssessmentList", "Assessment", new { courseid = cid });
        }

        //basic functions
        public void getCourseTeachName()
        {
            Course course = new Course();
            string cid = TempData.Peek("ReqCourseid").ToString();
            course = context.Courses.Find(cid);
            ViewBag.CourseName = course.courseName;
            Teacher tea = new Teacher();
            tea = context.Teachers.Find(course.courseFaculty);
            ViewBag.TeacherName = tea.teachername;
        }
    }
}