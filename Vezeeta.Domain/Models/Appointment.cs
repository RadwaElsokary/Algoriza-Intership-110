using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class Appointment
	{
		public int Id { set; get; }
		public int Price { set; get; }
		public Days Day { set; get; }
		public string DoctorId { set; get; }
		public Doctor Doctor { set; get; }
		public List<Time> Time { set; get; }

	}
}
