using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class LoginViewModel
    {

        [Display(Name = "EmailAddress")]
        [Required(ErrorMessage ="Email address is required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }   

    }
}
