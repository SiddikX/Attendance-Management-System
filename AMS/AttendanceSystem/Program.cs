using AttendanceSystem;
using Microsoft.EntityFrameworkCore;


// Set the console output encoding to UTF-8
Console.OutputEncoding = System.Text.Encoding.UTF8;

ActionHandling actionHandling = new ActionHandling();

using (var db = new AttendanceDbContext())
{
    // Initialize database
    db.Database.EnsureCreated();

    // Main menu loop
    while (true)
    {
        Console.WriteLine("Welcome to Attendance Management System!");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Exit");
        var choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine("\nPlease Enter your username and password.");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            var user = db.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
                    
            if (user != null)
            {
                Console.WriteLine("√ Login successful.\n");
                switch (user.UserType)
                {
                    case UserType.Admin:
                        actionHandling.HandleAdminActions(db);
                        break;
                    case UserType.Teacher:
                        actionHandling.HandleTeacherActions(db);
                        break;
                    case UserType.Student:
                        actionHandling.HandleStudentActions(db);
                        break;
                }
            }
            else
            {
                Console.WriteLine("! Invalid username or password.\n");
            }
        }
        else if (choice == "2")
        {
            break;
        }
        else
        {
            Console.WriteLine("! Invalid choice.\n");
        }
    }
}