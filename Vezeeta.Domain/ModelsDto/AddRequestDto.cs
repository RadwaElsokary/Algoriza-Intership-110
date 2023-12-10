using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.ModelsDto
{
	public class AddRequestDto
	{
		public string Time { set; get; }
		public string? DiscoundCoupon { set; get; }
	}
}
