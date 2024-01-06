using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class PageRequest
	{
		public IEnumerable<Request> Requests { set; get; }
		public int TotalCount { set; get; }
		public int TotalPages { set; get; }
	}
}
