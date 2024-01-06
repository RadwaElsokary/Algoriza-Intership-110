using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.ModelsDto
{
	public class PatientDto
	{

		public string FullName { set; get; }
		public string Image { set; get; }
		public string Email { set; get; }
		public string PhoneNumber { set; get; }
		public string DateOfBirth { set; get; }
		public string Gender { set; get; }
	}
}
