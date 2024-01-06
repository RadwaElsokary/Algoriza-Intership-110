using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;
using Vezeeta.Repository.Repository;

namespace VezeetaServices.AppointmentServices
{

	public class DoctorAppointmentRepository : IAppointmentRepository
	{

		private IRepository<Appointment> repository;
		private IRepository<Time> repositoryTime;
		private UserManager<ApplicationUser> userManager;
		private ApplicationDbContext context;
		public DoctorAppointmentRepository(IRepository<Appointment> repository, IRepository<Time> repositoryTime, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			this.repository = repository;
			this.repositoryTime = repositoryTime;
			this.context = context;
			this.userManager = userManager;
		}
		public bool AddAppointment(string DoctorId, Appointment appointment)
		{
			repository.Insert(appointment);
			repository.SaveChanges();
			return true;


		}
		public Time GetTime(int id)
		{
			var result = repositoryTime.GetId(id);
			return result;
		}

		public bool DeleteAppointment(int TimeId)
		{
			Time time = repositoryTime.GetId(TimeId);
			if (time.RequestId == null) 
		    { 
			    repositoryTime.Remove(time);
				repositoryTime.SaveChanges();

			}
			else
			{
				return false;
			}
			return true;
		}
		public  bool UpdateAppointment(int TimeId ,EditTimeDto dtotime)
		{

				Time time = repositoryTime.GetId(TimeId);
				if(time.RequestId == null)
			{
				time.Times = dtotime.Time;
				repositoryTime.Update(time);
			}
			else
			{
				return false;
			}
			return true;
		}
		public int CalculateAge(DateTime dateOfBirth)
		{
			DateTime birth = dateOfBirth;
			DateTime today = DateTime.Now;
			TimeSpan span = today - birth;
			DateTime age = DateTime.MinValue + span;

			return age.Year;
		}
		public string GetPatientTime(int TimeId)
		{
			var time = context.Times.Where(a => a.id == TimeId).Select(a=>a.Times).FirstOrDefault();
			return time.ToString();
		}
		public string GetPatientDay(int TimeId)
		{
			var appointmenet = context.Times.Include(a => a.Appointment).Where(a => a.id == TimeId).FirstOrDefault();
			var day = context.Appointments.Where(a => a.Id == appointmenet.AppointmentId).Select(a=>a.Day).FirstOrDefault();
			return day.ToString();
		}
		public List<Request> BookingByDoctorId(string DoctorId)
		{
			var requests = context.Requests.Include(r => r.Time).ThenInclude(t => t.Appointment)
				.Where(t=>t.Time.Appointment.DoctorId == DoctorId).ToList();
				//.Where(r => r.Time.Appointment.Doctor.Id == DoctorId).ToList();
			return requests;
		}
		public ApplicationUser GetPatient(string PatientId)
		{
			var patient = userManager.Users.FirstOrDefault(a => a.Id == PatientId);
			return patient;
		}
		public async Task<PageRequest> GetAllPatientBooking(string? term,int? page, int limit,string DoctorId)
		{

			//IQueryable<Request> requests;
			var requests = BookingByDoctorId(DoctorId).AsQueryable();

			//search by (day(int of enum (0,1,2,...)))
			if (string.IsNullOrWhiteSpace(term))
			{
				requests = BookingByDoctorId(DoctorId).AsQueryable();
			}
			else
			{
				term = term.Trim().ToLower();

				if (Enum.TryParse(typeof(Days), term, out var dayOfWeekEnum))
				{
					// Filter the requests based on the selected day
					requests = context.Requests
						.Where(r => r.Time.Appointment.Day == (Days)dayOfWeekEnum)
						.Include(r => r.Time)
						.ThenInclude(t => t.Appointment);
				}
				else
				{
					// Handle invalid day of the week (empty result)
					requests = Enumerable.Empty<Request>().AsQueryable();
				}
			}

			
			//pagination
			var totalCount = await requests.Where(a=>a.Status == StatusRequest.Pending).CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalCount /limit);

			var pageRequests = await requests.Skip((int)((page - 1) * limit)).Take(limit).ToListAsync();

			var pageRequestData = new PageRequest
			{
				Requests = pageRequests,
				TotalCount = totalCount,
				TotalPages = totalPages
			};
			return pageRequestData;
		}
		

	}
}
