namespace Hospital_OPD___Appointment_Management_System__HAMS_.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string Specialization { get; set; }
        public string Availability { get; set; }  // Example: "10:00-14:00"
        public bool IsOnLeave { get; set; }

        public Department Department { get; set; }
    }

}
