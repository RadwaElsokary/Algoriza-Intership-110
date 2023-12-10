using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;


namespace Vezeeta.Services.SpecializationServices
{
	public static class SpecializationsStore
	{
		private static List<Specialization> specialization;

		public static void SeedSpecialization(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Specialization>().HasData(
				specialization = new List<Specialization>
				{
					new Specialization(){Id="0",Name="Endocrinologist"},
					new Specialization(){Id="1",Name="Pediatrician"},
					new Specialization(){Id="2",Name="Internist"},
					new Specialization(){Id="3",Name="Neurologist"},
					new Specialization(){Id="4",Name="Psychiatrist"},
					new Specialization(){Id="5",Name="Ophthalmologist"},
				});
		}

	}
}
