using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class AttendanceDbContext : DbContext
    {
        private readonly string _connectionString;
        public AttendanceDbContext()
        {
            _connectionString = "Server=DESKTOP-353N469\\SQLEXPRESS;Database=attendance_db;User Id=siddik;Password=123123;TrustServerCertificate=True";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var admin1 = new User { UserId = 1, Name = "Admin", Username = "admin", Password = "123456", UserType = 0 };
            modelBuilder.Entity<User>().HasData(admin1);

            // User to Course (Many-to-Many through CourseUser)
            modelBuilder.Entity<CourseUser>()
                .HasKey(cu => new { cu.UserId, cu.CourseId });

            modelBuilder.Entity<CourseUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.CourseUsers)
                .HasForeignKey(cu => cu.UserId);

            modelBuilder.Entity<CourseUser>()
                .HasOne(cu => cu.Course)
                .WithMany(c => c.CourseUsers)
                .HasForeignKey(cu => cu.CourseId);

            // Course to ClassSchedule (One-to-Many)
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.ClassSchedules)
                .HasForeignKey(cs => cs.CourseId);

            // Attendance to Course (Many-to-One)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Attendances)
                .HasForeignKey(a => a.CourseId);

            // Attendance to User (Many-to-One)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }

    }
}
