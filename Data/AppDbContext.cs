using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Patient> patients { get; set; }

        public DbSet<Department> Departments { get; set; }
    }
}
