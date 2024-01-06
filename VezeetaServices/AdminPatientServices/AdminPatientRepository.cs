using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Vezeeta.Domain.Models;
using Vezeeta.Repository;
using Vezeeta.Repository.Repository;
using Microsoft.AspNetCore.Identity;

namespace VezeetaServices.PatientServices
{
	public class AdminPatientRepository : IAdminpatientRepository
	{
	
		private readonly UserManager<ApplicationUser> userManger;
		private readonly ApplicationDbContext context;
			
		public AdminPatientRepository( UserManager<ApplicationUser> userManger, ApplicationDbContext context)
		{
			this.userManger = userManger;
			this.context = context;
		}
		public async Task<PagePatient> GetAllPatients(string? term, string? sort, int? page, int limit)
		{
			
			IQueryable<ApplicationUser> patients;

			//search by (firstname or lastname)
			if (string.IsNullOrWhiteSpace(term))
			{
				patients = context.Users;
			}
			else
			{
				term = term.Trim().ToLower();

				patients = context.Users.Where(d => d.FirstName.ToLower().Contains(term) || d.LastName.ToLower().Contains(term));
			}

			//sorting
			if (!string.IsNullOrWhiteSpace(sort))
			{
				var sortFields = sort.Split(',');
				StringBuilder orderQueryBuilder = new StringBuilder();
				PropertyInfo[] propertyInfo = typeof(ApplicationUser).GetProperties();
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
					patients = patients.OrderBy(orderQuery);
				}
				else
				{
					patients = patients.OrderBy(a => a.Id);
				}
			}

			//pagination
			var totalCount =await GetPatientsNum();
			var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

			var pagePatients = await patients.Skip((int)((page - 1) * limit)).Take(limit).ToListAsync();

			var pagePatientData = new PagePatient
			{
				Patients = pagePatients,
				TotalCount = totalCount,
				TotalPages = totalPages
			};
			return pagePatientData;
		}
		public List<Request> PatientRequests (string PatientId)
		{
			var result = context.Requests.Where(a => a.PatientId == PatientId).ToList();
			return result;
		}
		public int CalculateAge(DateTime dateOfBirth)
		{
			DateTime birth = dateOfBirth;
			DateTime today = DateTime.Now;
			TimeSpan span = today - birth;
			DateTime age = DateTime.MinValue + span;

			return age.Year;
		}
		public ApplicationUser GetPatientById(string PatientId)
		{
			var patient = userManger.Users.FirstOrDefault(a => a.Id == PatientId);
			return patient ;

		}
		public async Task<int> GetPatientsNum()
		{
			var Patients = await userManger.GetUsersInRoleAsync("Patient");
			var PatientsCount = Patients.Count();
			return PatientsCount;
		}
		public async Task<Doctor> GetDoctorRequest(int RequestId)
		{
			var time = context.Times.Include(a => a.Appointment).FirstOrDefault(a => a.RequestId == RequestId);
			var appointment = context.Appointments.Include(a => a.Doctor).FirstOrDefault(a => a.Id == time.AppointmentId);
			var doctor = context.Doctors.FirstOrDefault(a => a.Id == appointment.DoctorId);
			return doctor;
		}
		public async Task<Appointment> GetDoctorAppointmentRequest(int RequestId)
		{
			var time = context.Times.Include(a => a.Appointment).FirstOrDefault(a => a.RequestId == RequestId);
			var appointment = context.Appointments.FirstOrDefault(a => a.Id == time.AppointmentId);
			return appointment;
		}
		public async Task<string> GetDoctorTimeRequest(int RequestId)
		{
			var request = context.Requests.Include(a => a.Time).FirstOrDefault(a => a.Id == RequestId);
			var time = context.Times.FirstOrDefault(a => a.id == request.TimeId);
			return time.Times;
		}
		public async Task<string> GetDoctorDiscoundRequest(int RequestId)
		{
			var request = context.Requests.Include(a => a.Discound).FirstOrDefault(a => a.Id == RequestId);
			if (request.DiscoundId == null)
			{
				return null;
			}
			else
			{
				var discound = context.Discounds.FirstOrDefault(a => a.Id == request.DiscoundId);
				return discound.DiscoundCode;
			}
		}
		public  async Task<string> GetDoctorSpecializationRequest(int RequestId)
		{
			var time = context.Times.Include(a => a.Appointment).FirstOrDefault(a => a.RequestId == RequestId);
			var appointment = context.Appointments.Include(a => a.Doctor).FirstOrDefault(a => a.Id == time.AppointmentId);
			var doctor = context.Doctors.Include(a => a.Specialist).FirstOrDefault(a => a.Id == appointment.DoctorId);
			var specialist = context.Specializations.FirstOrDefault(a => a.Id == doctor.SpecialistId);
			return specialist.Name;
		}
	}
}
