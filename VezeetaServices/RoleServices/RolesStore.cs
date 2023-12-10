using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vezeeta.Services.RoleServices
{
	public  static class RolesStore
	{
		private static List<IdentityRole> roles;

		public static void SeedRoles(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityRole>().HasData(
				roles = new List<IdentityRole>
				{
					new IdentityRole(){Id="0" ,Name="Admin",NormalizedName="ADMIN"},
					new IdentityRole(){Id="1" ,Name="Doctor",NormalizedName="DOCTOR"},
					new IdentityRole(){Id="2" ,Name="Patient",NormalizedName="PATIENT"}
				});
		}
	}
}
