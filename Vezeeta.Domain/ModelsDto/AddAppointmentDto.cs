using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;

namespace Vezeeta.Domain.ModelsDto
{
	public class AddAppointmentDto
	{
		public int Price { set; get; }
		public Days Day { set; get; }

		public List<string> Time { set; get; }
	}
}
