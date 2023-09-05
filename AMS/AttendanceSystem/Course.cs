using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public double Fees { get; set; }

        public virtual ICollection<CourseUser> CourseUsers { get; set; }
        public virtual ICollection<ClassSchedule> ClassSchedules { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
