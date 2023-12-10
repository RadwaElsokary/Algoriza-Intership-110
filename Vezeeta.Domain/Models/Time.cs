using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class Time
	{
		public int id { set; get; }

		[DataType(DataType.Time)]
		public string Times { set; get; }

		public int AppointmentId { set; get; }
		public Appointment Appointment { set; get; }
		public int? RequestId {set; get;}
		public Request? Request { set; get; }


	}
}
