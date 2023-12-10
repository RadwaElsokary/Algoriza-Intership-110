using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class PageDoctor
	{
		public IEnumerable<Doctor> Doctors { set; get; }
		public int TotalCount { set; get; }
		public int TotalPages { set; get; }
	}
}
