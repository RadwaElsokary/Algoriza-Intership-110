using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;
using VezeetaServices.AppointmentServices;
using VezeetaServices.RequestServices;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorController : ControllerBase
	{
		private readonly IAppointmentRepository appointmentRepository;
		private readonly IRequestRepository requestRepository;
		private readonly UserManager<ApplicationUser> userManager;
		
		public DoctorController(IAppointmentRepository appointmentRepository, IRequestRepository requestRepository,UserManager<ApplicationUser> userManager)
		{
			this.appointmentRepository = appointmentRepository;
			this.requestRepository = requestRepository;
			this.userManager = userManager;
		}

		private Appointment MapAppointmentTime (string DoctorId, AddAppointmentDto addAppointment)
		{
			var result = new Appointment();
			result.Day = addAppointment.Day;
			result.DoctorId = DoctorId;
			result.Price = addAppointment.Price;
			result.Time = new List<Time>();
			addAppointment.Time.ForEach(time =>
			{
				var NewTime = new Time();
				NewTime.AppointmentId = result.Id;
				NewTime.Times = time;
				result.Time.Add(NewTime);
			});
			return result;
		}

		[HttpPost]
		[Route("AddAppointment")]
		public async Task<IActionResult> AddAppointment(string DoctorId,AddAppointmentDto appointment)
		{
			var NewAppointmewnt = MapAppointmentTime(DoctorId, appointment);
			var result = appointmentRepository.AddAppointment(DoctorId,NewAppointmewnt);
			if (result)
			{
				return StatusCode(200, true);
			}
			else
			{
				return StatusCode(400, false);
			}
		}
		[HttpPut]
		[Route("EditAppointment")]
		public  IActionResult EditAppointment(int TimeId ,EditTimeDto editTime)
		{
		  var result = appointmentRepository.UpdateAppointment(TimeId, editTime);
			if (result)
			{
				return StatusCode(200,true);
			}
			else
			{
				return StatusCode(400, false);
			}

		}

		[HttpDelete]
		[Route("DeleteAppointmentTime")]
		public IActionResult DeleteAppointment ( int TimeId)
		{
			var result = appointmentRepository.DeleteAppointment(TimeId);
			if (result)
			{
				return StatusCode(200, true);
			}
			else
			{
				return StatusCode(400, false);
			}
		}

		[HttpPut]
		[Route("ConfirmRequest")]
		public IActionResult ConfirmRequest(int RequestId, string DoctorId)
		{
			var result = requestRepository.ConfirmRequest(RequestId, DoctorId);
			if (result)
			{
				return StatusCode(200, true);
			}
			else
			{
				return StatusCode(400, false);
			}
		}

		[HttpGet]
		[Route("GetAllPatientsBooking")]
		public async Task<IActionResult> GetAllPatients(string DoctorId,string? SrearchByDate, int Page = 1, int PagesLimit = 10)
		{
			var result = await appointmentRepository.GetAllPatientBooking(SrearchByDate, Page, PagesLimit,DoctorId);

			Response.Headers.Add("X-Total-Count",
				result.TotalCount.ToString());
			Response.Headers.Add("X-Total-Pages",
				result.TotalPages.ToString());

			var patients = result.Requests.Where(a=>a.Status == StatusRequest.Pending).Select(booking => new PatientsBookingDto
			{
				Image = appointmentRepository.GetPatient(booking.PatientId).PhotoPath,
				FullName = appointmentRepository.GetPatient(booking.PatientId).FirstName + " " + appointmentRepository.GetPatient(booking.PatientId).LastName,
				Email = appointmentRepository.GetPatient(booking.PatientId).Email,
				PhoneNumber = appointmentRepository.GetPatient(booking.PatientId).PhoneNumber,
				Gender = appointmentRepository.GetPatient(booking.PatientId).Gender.Value.ToString(),
				Age = appointmentRepository.CalculateAge(appointmentRepository.GetPatient(booking.PatientId).BirthOfDate),
				Appointment = $"{appointmentRepository.GetPatientDay(booking.TimeId)} { appointmentRepository.GetPatientTime(booking.TimeId)}"
			});

			return Ok(patients);

		}
	}
}
