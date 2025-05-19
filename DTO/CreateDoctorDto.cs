namespace Hospital_OPD___Appointment_Management_System__HAMS_.DTO
{
    public class CreateDoctorDto
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string Specialization { get; set; }
        public string Availability { get; set; }
        public bool IsOnLeave { get; set; }
    }

}
