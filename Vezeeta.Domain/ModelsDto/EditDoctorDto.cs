using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;

namespace Vezeeta.Domain.ModelsDto
{
	public class EditDoctorDto 
	{
		
		public string FirstName { set; get; }
	
		public string LastName { set; get; }

		
		[EmailAddress]
		public string Email { set; get; }


		[Display(Name = "Phone Number")]
		public string PhoneNumber { set; get; }

		public Gender Gender { set; get; }


		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:mm/dd/yyyy")]
		[Display(Name = "Birth Of Date")]
		public DateTime BirthOfDate { set; get; }

	
		public IFormFile Photo { set; get; }

		public int Price { set; get; }

		public string Specialization { set; get; }
		
		
	}
}
