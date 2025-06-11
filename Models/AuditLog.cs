using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string Action { get; set; }

        public string PerformedBy { get; set; }

        public DateTime PerformedAt { get; set; }

        public string Details { get; set; }
    }
}
