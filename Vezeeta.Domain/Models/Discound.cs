using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.Models
{
	public class Discound
	{
		public int Id { set; get; }
		public string DiscoundCode { set; get;}
		public DiscoundType Type{set; get;}
		public int Value { set; get; }
		public int RequestNumber { set; get; }
		public bool IsActive { set; get; }
		public List<Request>? Requests { set; get; }

	}
}
