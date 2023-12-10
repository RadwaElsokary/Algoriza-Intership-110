using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class Specialization
	{
		public string Id { set; get; }
		public string Name { set; get; }

		public List<Doctor>? Doctor { set; get; }
	}
}
