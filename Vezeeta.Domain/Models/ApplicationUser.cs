using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? PhotoPath { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
		public Gender? Gender { set; get; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy"),]
		public DateTime BirthOfDate { set; get; }
		public List<Request>? Requests { set; get; }
	}
}
