namespace Hospital_OPD___Appointment_Management_System__HAMS_.Services.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

}
