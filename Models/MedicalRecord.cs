namespace Hospital_OPD___Appointment_Management_System__HAMS_.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Notes { get; set; }
        public string Prescription { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }

}
