using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;
using Vezeeta.Repository.Repository;

namespace VezeetaServices.RequestServices
{
	public class RequestRepository : IRequestRepository
	{
		private readonly IRepository<Request> repository;
		private readonly ApplicationDbContext context;
		public RequestRepository(IRepository<Request> repository, ApplicationDbContext context)
		{
			this.repository = repository;
			this.context = context;
		}
		public bool AddRequest(string PatientId , Request model)
		{
			repository.Insert(model);
			repository.SaveChanges();
			return true;
		}
		public bool CancelRequest(int id, string PatientId)
		{
			var request = repository.GetId(id);
			if(request.PatientId == PatientId && request.Status == StatusRequest.Pending)
			{
				request.Status = StatusRequest.Cancel;
				repository.Update(request);
				repository.SaveChanges();
			}
			else
			{
				return false;
			}
			return true;
		}
		public bool ConfirmRequest(int RequestId, string DoctorId)
		{
			var request = repository.GetId(RequestId);
			var doctor = GetDoctorId(RequestId);
			if(doctor == DoctorId && request.Status == StatusRequest.Pending)
			{
				request.Status = StatusRequest.Complete;
				repository.Update(request);
				repository.SaveChanges();
			}
			else
			{
				return false;
			}
			return true;
		}
		public string GetDoctorId(int RequiestId)
		{
			var time = context.Times.FirstOrDefault(a => a.RequestId == RequiestId);
			var result = context.Appointments.FirstOrDefault(a => a.Id == time.AppointmentId);
			return result.DoctorId;
		}
		public async Task<string> GetTimeValue(int id)
		{
			var result = await context.Requests.Include(r => r.Time).FirstOrDefaultAsync(r => r.Id == id);
			return result.Time.Times;
		}
		public Time GetTime(string time)
		{
			var result = context.Times.FirstOrDefault(x => x.Times == time);
			return result;
		}
		public Discound GetDiscound (string Coupon)
		{
			var result = context.Discounds.FirstOrDefault(x => x.DiscoundCode == Coupon);
			return result;
		}
		public int GetAppointmentPrice(string time)
		{
			var Time =  context.Times.FirstOrDefault(a => a.Times == time);
			var result = context.Appointments.FirstOrDefault(a => a.Id == Time.AppointmentId);
			return result.Price;
		}
		public int GetAllRequestNum()
		{
			int AllRequest = repository.GetAll().Count();
			return AllRequest; 
		}
		public int GetPendingRequestNum()
		{
			int PendingRequest = repository.GetAll().Where(a => a.Status == StatusRequest.Pending).Count();
			return PendingRequest;
		}
		public int GetCompleteRequestNum()
		{
			int CompleteRequest = repository.GetAll().Where(a => a.Status == StatusRequest.Complete).Count();
			return CompleteRequest;
		}
		public int GetCancelRequestNum()
		{
			int CancelRequest = repository.GetAll().Where(a => a.Status == StatusRequest.Cancel).Count();
			return CancelRequest;
		}
		public int GetCompletedRequestNumToPatient(string PatientId )
		{
			var num = repository.GetAll().Where(a => a.PatientId == PatientId && a.Status == StatusRequest.Complete).Count();
			return num;
		}
		public  List<Appointment> GetDoctorAppointment(string DoctorId)
		{
			var appointment = context.Appointments.Where(a => a.DoctorId == DoctorId).ToList();
			return appointment;
		}
		public  List<Time> GetDoctorTime(int AppointmentId)
		{
			var time = context.Times.Where(a => a.AppointmentId == AppointmentId ).ToList();
			var cancelTime = context.Requests.Where(a => a.Time.AppointmentId == AppointmentId && a.Status == StatusRequest.Cancel).Select(r=>r.Time).ToList();
			var availableTime = time.Where(a => a.RequestId == null || cancelTime.Contains(a)).ToList();
			return availableTime;
		}
		public bool IsTimeCancelled(int TimeId)
		{
			var isCancelled = context.Requests.Any(a => a.TimeId == TimeId && a.Status == StatusRequest.Cancel);
			return isCancelled;
		}





	}
}
