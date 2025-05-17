using Hospital_OPD___Appointment_Management_System__HAMS_.Models;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IDepartmentService
    {
        public Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        public Task<Department> GetDepartmentByIdAsync(int id);
        public Task<Department> CreateDepartmentAsync(Department department);
        public  Task<bool> UpdateDepartmentAsync(Department department);
        public Task<bool> DeleteDepartmentAsync(int id);
    }

}
