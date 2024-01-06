using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.ModelsDto
{
	public class PatientsBookingDto
	{
		public string FullName { set; get; }
		public string Image { set; get; }
		public string Email { set; get; }
		public string PhoneNumber { set; get; }
		public int Age { set; get; }
		public string Gender { set; get;}
		public string Appointment { set; get;}


	}
}
