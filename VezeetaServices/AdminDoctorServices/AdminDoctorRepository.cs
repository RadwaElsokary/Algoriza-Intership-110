using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;
using Vezeeta.Repository.Repository;


namespace Vezeeta.Services.DoctorServices
{
	public class AdminDoctorRepository : IAdminDoctorRepository
	{
		private IRepository<Doctor> repository;
		private readonly ApplicationDbContext context;

		public AdminDoctorRepository(IRepository<Doctor> repository, ApplicationDbContext context)
		{
			this.context = context;
			this.repository = repository;
		}

		public bool DeleteDoctor(string Id)
		{
			Doctor doctor = GetDoctor(Id);
			repository.Remove(doctor);
			repository.SaveChanges();
			return true;
		}

		public async Task<PageDoctor> GetAllDoctors(string? term, string? sort, int? page, int limit)
		{
			IQueryable<Doctor> doctors ;
		
			//search by (firstname or lastname)
			if (string.IsNullOrWhiteSpace(term))
			{
				doctors = context.Doctors;
			}
			else
			{
				term = term.Trim().ToLower();

				doctors = context.Doctors.Where(d => d.FirstName.ToLower().Contains(term) || d.LastName.ToLower().Contains(term));
			}

			//sorting
			if (!string.IsNullOrWhiteSpace(sort))
			{
				var sortFields = sort.Split(',');
				StringBuilder orderQueryBuilder = new StringBuilder();
				PropertyInfo[] propertyInfo = typeof(Doctor).GetProperties();
				foreach (var field in sortFields)
				{
					string sortOrder = "ascending";
					var sortField = field.Trim();
					if (sortField.StartsWith("-"))
					{
						sortField = sortField.TrimStart('-');
						sortOrder = "descending";
					}

					var property = propertyInfo.FirstOrDefault(a => a.Name.Equals(sortField, StringComparison.OrdinalIgnoreCase));
					if (property == null)
						continue;
					orderQueryBuilder.Append($"{property.Name.ToString()} {sortOrder},");
				}
				string orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ',');
				if (!string.IsNullOrWhiteSpace(orderQuery))
				{
					doctors = doctors.OrderBy(orderQuery);
				}
				else
				{
					doctors = doctors.OrderBy(a => a.Id);
				}
			}

			//pagination
			var totalCount = await context.Doctors.CountAsync();
			var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

			var pageDoctors = await doctors.Skip((int)((page - 1) * limit)).Take(limit).ToListAsync();

			var pageDoctorData = new PageDoctor
			{
				Doctors = pageDoctors,
				TotalCount = totalCount,
				TotalPages = totalPages
			};
			return pageDoctorData;
		}

		public async Task<string> GetSpecialization(string id)
		{
			var doctor = await context.Doctors.Include(d => d.Specialist).FirstOrDefaultAsync(d => d.Id == id);
			return doctor.Specialist.Name ;
		}
		public  Doctor GetDoctor(string Id)
		{	
			var result= repository.GetById(Id);
			return result;
		}

		public int GetDoctorsNum()
		{
			return repository.GetAll().Count();
		}

		public bool UpdateDoctor(Doctor doctor)
		{
			repository.Update(doctor);
			repository.SaveChanges();
			return true;
			
		}

		public Specialization AddSpecialization(string SpecializationDoctor)
		{
			var result =  context.Specializations.FirstOrDefault(x => x.Name == SpecializationDoctor);
			return result;
			
		}
	}
}
