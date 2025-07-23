namespace Hospital_OPD___Appointment_Management_System__HAMS_.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; } 

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }

}
