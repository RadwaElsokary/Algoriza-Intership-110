using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.ModelsDto
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
		public string Email { set; get; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { set; get; }

		//public IList<AuthenticationScheme>? ExternalLogin { set; get; }
	}
}
