using System.ComponentModel.DataAnnotations;
namespace Hospital_OPD___Appointment_Management_System__HAMS_.DTO
{
    

    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }


}
