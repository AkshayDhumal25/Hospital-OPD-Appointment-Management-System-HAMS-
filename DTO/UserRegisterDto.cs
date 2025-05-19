using System.ComponentModel.DataAnnotations;
namespace Hospital_OPD___Appointment_Management_System__HAMS_.DTO
{
    

    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(Admin|Doctor|Reception)$", ErrorMessage = "Role must be 'Admin', 'Doctor', or 'Patient'.")]
        public string Role { get; set; }
    }


}
