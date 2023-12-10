using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;

namespace Vezeeta.Domain.ModelsDto
{
	public class DiscoundDto
	{
		public string DiscoundCodeCoupon { set; get; }
		public DiscoundType Type { set; get; }
		public int Value { set; get; }
		public int RequestNumber { set; get; }
	}
}
