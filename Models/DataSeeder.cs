using BCrypt.Net;
using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using System.Linq;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Models
{
    public static class DataSeeder
    {
        public static void SeedAdminUser(AppDbContext context)
        {
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new ApplicationUser
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), 
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
