using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class Request
	{
		public int Id { set; get; }
		public StatusRequest Status { set; get; }
		public int FinalPrice { set; get; }
		public int TimeId { set; get; }
		public Time Time { set; get; }
		//public string DoctorId { set; get; }
		public string PatientId { set; get; }
		public ApplicationUser Patient { set; get; }
		public int?  DiscoundId {set; get;}
		public Discound? Discound { set; get; } 

	}
}
