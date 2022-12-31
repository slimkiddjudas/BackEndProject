using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Models
{
    public class RegisterModel
    {
        [Required]
        [DisplayName("User Name")]
        [StringLength(60)]
        public string UserName { get; set; }

        [Required]
        [DisplayName("FirstName")]
        [StringLength(60)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("FirstName")]
        [StringLength(60)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match each other.")]
        public string ConfirmPassword { get; set; }
    }
}
