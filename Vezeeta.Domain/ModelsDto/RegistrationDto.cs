using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;

namespace Vezeeta.Domain.ModelsDto
{
    public class RegistrationDto
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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { set; get; }

		[Required]
		[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:mm/dd/yyyy")]
        [Display(Name = "Birth Of Date")]
        public DateTime BirthOfDate { set; get; }

        public IFormFile? Photo { set; get; }





    }
}
