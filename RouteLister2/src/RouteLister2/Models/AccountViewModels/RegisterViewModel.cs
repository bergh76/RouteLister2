using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        //[Required]
        [Display(Name = "Role")]
        public string UserRole { get; set; }

        [Remote("CheckUserNameExists", "Admin")]
        [Required]
        [Display(Name = "User")]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "Användarnamnet får inte innehålla mellanslag")]
        public string UserName { get; set; }

        [Remote("CheckRegNrExists", "Admin")]
        [Display(Name = "Regnr")]
        [RegularExpression(@"^[A-Za-z]{3}\d{3}$", ErrorMessage = "Registreringsnummret måste ha formatet XXX111")]
        public string RegNr { get; set; }

        [Remote("CheckEmailExists", "Admin")]
        [Required(ErrorMessage = "Du måste ange en epostadress")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Du måste ange en epostadress")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} måste vara minst {2} och max {1} tecken långt.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Inmatningen matchar inte varandra!")]
        public string ConfirmPassword { get; set; }
    }
}
