using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Hospital_OPD___Appointment_Management_System__HAMS_.Models
{
    public class Patient
    {

        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }


        [Required]
        public DateOnly DOB { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        [Required]
        public string Address { get; set; }

        


        //[Required,NotNull]

        //public string Password { get; set; } 



    }
}
