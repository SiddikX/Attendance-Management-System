using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class ActionHandling
    {
        // Admin logic
        public void HandleAdminActions(AttendanceDbContext db)
        {
            while (true)
            {
                Console.WriteLine("Admin Actions:");
                Console.WriteLine("1. Add Teacher");
                Console.WriteLine("2. Add Student");
                Console.WriteLine("3. Add Course");
                Console.WriteLine("4. Assign Teacher to Course");
                Console.WriteLine("5. Assign Student to Course");
                Console.WriteLine("6. Set Class Schedule");
                Console.WriteLine("7. Logout");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Add Teacher
                        Console.WriteLine("\nAdding a new teacher");
                        Console.WriteLine("Enter teacher name: ");
                        var teacherName = Console.ReadLine();
                        Console.WriteLine("Enter teacher username: ");
                        var teacherUsername = Console.ReadLine();
                        Console.WriteLine("Enter teacher password: ");
                        var teacherPassword = Console.ReadLine();
                        db.Users.Add(new User { Name = teacherName, Username = teacherUsername, Password = teacherPassword, UserType = UserType.Teacher });
                        db.SaveChanges();
                        Console.WriteLine("√ Teacher added successfully.\n");

                        break;
                    case "2":
                        // Add Student
                        Console.WriteLine("\nAdding a new student");
                        Console.WriteLine("Enter student name: ");
                        var studentName = Console.ReadLine();
                        Console.WriteLine("Enter student username: ");
                        var studentUsername = Console.ReadLine();
                        Console.WriteLine("Enter student password: ");
                        var studentPassword = Console.ReadLine();
                        db.Users.Add(new User { Name = studentName, Username = studentUsername, Password = studentPassword, UserType = UserType.Student });
                        db.SaveChanges();
                        Console.WriteLine("√ Student added successfully.\n");
                        break;
                    case "3":
                        // Add Course
                        Console.WriteLine("\nAdding a new Course.");
                        Console.WriteLine("Enter course name: ");
                        var courseName = Console.ReadLine();
                        Console.WriteLine("Enter course fee: ");
                        var courseFee = int.Parse(Console.ReadLine());
                        db.Courses.Add(new Course { CourseName = courseName, Fees = courseFee });
                        db.SaveChanges();
                        Console.WriteLine("√ Course added successfully.\n");
                        break;
                    case "4":
                        // Assign Teacher to Course

                        // Display all courses
                        Console.WriteLine("\nList of courses:");
                        var allCoursesForTeacher = db.Courses.ToList();
                        foreach (var course in allCoursesForTeacher)
                        {
                            Console.WriteLine($"{course.CourseId}. {course.CourseName}");
                        }

                        Console.WriteLine("\nSelect a course by ID to assign a teacher: ");
                        var courseIdForTeacher = int.Parse(Console.ReadLine());

                        // Display all teachers
                        Console.WriteLine("\nList of teachers:");
                        var allTeachers = db.Users.Where(u => u.UserType == UserType.Teacher).ToList();
                        foreach (var teacher in allTeachers)
                        {
                            Console.WriteLine($"{teacher.UserId}. {teacher.Name}");
                        }

                        Console.WriteLine("\nSelect a teacher by ID to assign to the course: ");
                        var teacherId = int.Parse(Console.ReadLine());

                        // Assign the teacher to the course
                        db.CourseUsers.Add(new CourseUser { UserId = teacherId, CourseId = courseIdForTeacher });
                        db.SaveChanges();

                        Console.WriteLine("√ Teacher assigned to the course successfully.\n");
                        break;

                    case "5":
                        // Assign Student to Course

                        // Display all courses
                        Console.WriteLine("\nList of courses:");
                        var allCoursesForStudent = db.Courses.ToList();
                        foreach (var course in allCoursesForStudent)
                        {
                            Console.WriteLine($"{course.CourseId}. {course.CourseName}");
                        }

                        Console.WriteLine("\nSelect a course by ID to assign a student: ");
                        var courseIdForStudent = int.Parse(Console.ReadLine());

                        // Display all students
                        Console.WriteLine("\nList of students:");
                        var allStudents = db.Users.Where(u => u.UserType == UserType.Student).ToList();
                        foreach (var student in allStudents)
                        {
                            Console.WriteLine($"{student.UserId}. {student.Name}");
                        }

                        Console.WriteLine("\nSelect a student by ID to assign to the course: ");
                        var studentId = int.Parse(Console.ReadLine());

                        // Assign the student to the course
                        db.CourseUsers.Add(new CourseUser { UserId = studentId, CourseId = courseIdForStudent });
                        db.SaveChanges();

                        Console.WriteLine("√ Student assigned to the course successfully.\n");
                        break;

                    case "6":
                        // Set Class Schedule

                        // Display all courses
                        Console.WriteLine("\nList of courses:");
                        var allCoursesForSchedule = db.Courses.ToList();
                        foreach (var course in allCoursesForSchedule)
                        {
                            Console.WriteLine($"{course.CourseId}. {course.CourseName}");
                        }

                        Console.WriteLine("\nSelect a course by ID to set the schedule: ");
                        var courseIdForSchedule = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the day for the class (e.g., Monday): ");
                        var classDay = Console.ReadLine();

                        Console.WriteLine("Enter the start time (HH:mm format): ");
                        var startTime = TimeSpan.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the end time (HH:mm format): ");
                        var endTime = TimeSpan.Parse(Console.ReadLine());

                        Console.WriteLine("Enter total number of classes: ");
                        var totalClasses = int.Parse(Console.ReadLine());

                        // Set the schedule for the selected course
                        db.ClassSchedules.Add(new ClassSchedule { Day = classDay, StartTime = startTime, EndTime = endTime, TotalClasses = totalClasses, CourseId = courseIdForSchedule });
                        db.SaveChanges();

                        Console.WriteLine("√ Class schedule set successfully.\n");
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("! Invalid choice.\n");
                        break;
                }
            }
        }

        // Teacher logic
        public void HandleTeacherActions(AttendanceDbContext db)
        {
            while (true)
            {
                Console.WriteLine("Teacher Actions:");
                Console.WriteLine("1. View Attendance Report");
                Console.WriteLine("2. Logout");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // View Attendance Report
                        Console.WriteLine("Enter the course name for attendance report:");
                        var courseName = Console.ReadLine();
                        var course = db.Courses.SingleOrDefault(c => c.CourseName == courseName);

                        if (course != null)
                        {
                            var attendances = db.Attendances
                                                .Where(a => a.CourseId == course.CourseId)
                                                .Include(a => a.Student)
                                                .ToList();

                            Console.WriteLine($"Attendance report for {courseName}");

                            foreach (var attendance in attendances)
                            {
                                var mark = attendance.IsPresent ? " √ " : " X ";
                                Console.WriteLine($"\n{attendance.Student.Name}: {mark}");
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Course not found.\n");
                        }
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("! Invalid choice.\n");
                        break;
                }
            }
        }

        // Student logic
        public void HandleStudentActions(AttendanceDbContext db)
        {
            while (true)
            {
                Console.WriteLine("Student Actions:");
                Console.WriteLine("1. Record Attendance");
                Console.WriteLine("2. Logout");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Record Attendance
                        Console.WriteLine("Enter the course name for attendance:");
                        var courseName = Console.ReadLine();
                        var course = db.Courses.SingleOrDefault(c => c.CourseName == courseName);

                        if (course != null)
                        {
                            // Taking Attendance
                            Console.WriteLine("Write your UserName to give attendance:");
                            var studentName = Console.ReadLine();
                            var students = db.Users.SingleOrDefault(u => u.Username == studentName && u.UserType == UserType.Student);
                            
                            if (students != null)
                            {
                                var studentId = students.UserId;
                                var attendance = new Attendance
                                {
                                    StudentId = studentId,
                                    CourseId = course.CourseId,
                                    IsPresent = true
                                };

                                db.Attendances.Add(attendance);
                                db.SaveChanges();
                                Console.WriteLine("Attendance recorded.\n");

                            }
                            else
                            {
                                Console.WriteLine("Your name is incorrect.\n");
                            }
   
                        }
                        else
                        {
                            Console.WriteLine("Course not found.\n");
                        }
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("! Invalid choice.\n");
                        break;
                }
            }
        }
    }
}
