using Hospital_OPD___Appointment_Management_System__HAMS_.Data;
using Hospital_OPD___Appointment_Management_System__HAMS_.Models;
using Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            var Dept = await _context.Departments.ToListAsync();
            if (Dept == null)
            {
                return null;
            }
            return Dept;
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
            => await _context.Departments.FindAsync(id);

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            var existing = await _context.Departments.FindAsync(department.Id);
            if (existing == null) return false;

            existing.Name = department.Name;
            existing.Description = department.Description;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var existing = await _context.Departments.FindAsync(id);
            if (existing == null) return false;

            _context.Departments.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
