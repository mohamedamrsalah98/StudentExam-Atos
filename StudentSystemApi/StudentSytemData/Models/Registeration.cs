
using System.ComponentModel.DataAnnotations;


namespace StudentSytemData.Models
{
    public class Registeration
    {
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required, StringLength(128)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }
    }
}
