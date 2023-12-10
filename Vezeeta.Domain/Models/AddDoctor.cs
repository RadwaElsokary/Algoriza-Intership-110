using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
    public class AddDoctor
    {
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Pssword")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { set; get; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { set; get; }

        [Required]
        public Gender Gender { set; get; }



        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy")]
        [Display(Name = "Birth Of Date")]
        public DateTime BirthOfDate { set; get; }

        [Required]
        public IFormFile Photo { set; get; }

        public int Price { set; get; }

        public string Specialization { set; get; }


    }
}
