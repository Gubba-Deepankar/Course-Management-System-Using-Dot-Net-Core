using CourseManagementSystem.Models.DomainModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagementSystem.Models.Database
{
    public class myDatabase : IdentityDbContext<User>
    {
        public myDatabase(DbContextOptions<myDatabase> options)
           : base(options)
        { }
        
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<StudentCourseData> StudentCoursesTable { get; set; }
        public DbSet<StudentAssignmentData> StudentAssignmentTable { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().HasData(
                new Course { courseid = "CIS1000", courseName="C-Sharp" ,courseFaculty = "T2001"},
                new Course { courseid = "CIS1001", courseName = "Java", courseFaculty = "T2002"},
                new Course { courseid = "CIS1002", courseName = "Android", courseFaculty = "T2003" },
                new Course { courseid = "CIS1003", courseName = "Ios", courseFaculty = "T2002" },
                new Course { courseid = "CIS1004", courseName = "Operating Sysytems", courseFaculty = "T2003" }
            );
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { teacherid = "T2001", teachername = "Deepu"},
                new Teacher { teacherid = "T2002", teachername = "Krishna" },
                new Teacher { teacherid = "T2003", teachername = "Radha" }
            );
        }
    }
}
