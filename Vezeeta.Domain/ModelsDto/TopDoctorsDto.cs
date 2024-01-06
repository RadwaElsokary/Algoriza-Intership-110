using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Domain.ModelsDto
{
	public class TopDoctorsDto
	{
		public string Image { set; get; }
		public string FullName { set; get; }
		public string Specialize { set; get; }
		public int RequestsNum { set; get; }
	}
}
