using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public enum UserType
    {
        Admin,
        Teacher,
        Student
    }

    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }

        public virtual ICollection<CourseUser> CourseUsers { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
    }

}
