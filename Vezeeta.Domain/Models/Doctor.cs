using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class Doctor : ApplicationUser
	{
		public int Price {set; get; }
		public string SpecialistId { set; get; }
		public Specialization? Specialist { set; get; }
		public List<Appointment> Appointments { set; get; }
	}
}
