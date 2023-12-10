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
	public class DoctorDto
	{
		
		public string FullName { set; get; }
		public string Image { set; get; }
		public string Email { set; get;  }

		public string PhoneNumber { set; get; }
		public string Specialize { set; get; }
		public string Gender { set; get; }
		

		
	}
}
