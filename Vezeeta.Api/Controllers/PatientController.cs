using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Services.DoctorServices;
using VezeetaServices.PatientServices;
using VezeetaServices.RequestServices;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IRequestRepository requestRepository;
		private readonly IAdminDoctorRepository doctorRepository;
		private readonly IAdminpatientRepository patientRepository;
		public PatientController (IRequestRepository requestRepository, IAdminDoctorRepository doctorRepository,
			IAdminpatientRepository patientRepository)
		{
			this.requestRepository = requestRepository;
			this.doctorRepository = doctorRepository;
			this.patientRepository = patientRepository;
		}
		[HttpPost]
		[Route("AddRequest")]
		public  IActionResult AddRequest(string PatientId,AddRequestDto model)
		{
			var time = requestRepository.GetTime(model.Time);
			int Price = requestRepository.GetAppointmentPrice(model.Time);
			int CompletedRequestsCount = requestRepository.GetCompletedRequestNumToPatient(PatientId);
			var result = new Request();
			result.PatientId = PatientId;
			result.TimeId = time.id;
			result.Time = time;
			result.Status = StatusRequest.Pending;
			if (model.DiscoundCoupon == null && (time.RequestId == null || (time.RequestId != null && requestRepository.IsTimeCancelled(time.id))))
			{
				result.Discound = null;
				result.FinalPrice = Price;
			}
			else if (model.DiscoundCoupon != null && (time.RequestId == null || (time.RequestId != null && requestRepository.IsTimeCancelled(time.id))))
			{
				var Coupon = requestRepository.GetDiscound(model.DiscoundCoupon);
				if(Coupon.IsActive && CompletedRequestsCount > Coupon.RequestNumber)
				{
					if (Coupon.Type == DiscoundType.Value)
					{
						result.Discound = Coupon;
						result.FinalPrice = Price - Coupon.Value;
					}
					else if (Coupon.Type == DiscoundType.Precentage)
					{
						result.Discound = Coupon;
						result.FinalPrice = Price * Coupon.Value/100;
					}
				}
				else
				{
					return StatusCode(400,false);
				}
				
			}
			else
			{
				return StatusCode(400, false);
			}
			
			var AddRequest = requestRepository.AddRequest(PatientId, result);

			if (AddRequest)
			{
				return StatusCode(200, true);
			}
			else
			{
				return StatusCode(400, false);
			}
			
		}

		[HttpPut]
		[Route("CancelRequest")]
		public IActionResult CancelRequest(int RequestId, string PatientId)
		{
			var result = requestRepository.CancelRequest(RequestId, PatientId);
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
		[Route("GetAllDoctors")]
		public async Task<IActionResult> GetAllDoctors(string? Srearch, string? SortBy, int Page = 1, int PagesLimit = 10)
		{
			var result = await doctorRepository.GetAllDoctors(Srearch, SortBy, Page, PagesLimit);

			Response.Headers.Add("X-Total-Count",
				result.TotalCount.ToString());
			Response.Headers.Add("X-Total-Pages",
				result.TotalPages.ToString());
			
			
			
			var doctorDto = new
			{
				doctors = result.Doctors.Select(doctor => new
				{
					Image = doctor.PhotoPath,
					FullName = doctor.FirstName + " " + doctor.LastName,
					Email = doctor.Email,
					PhoneNumber = doctor.PhoneNumber,
					Specialize = doctorRepository.GetSpecialization(doctor.Id).Result,
					Gender = doctor.Gender.Value.ToString(),
					Appointments = requestRepository.GetDoctorAppointment(doctor.Id).Select(appointment => new
					{
						Day = appointment.Day.GetDisplayName().ToString(),
						Price = appointment.Price,
						Times = requestRepository.GetDoctorTime(appointment.Id).Select( time => new
						{
							 time.Times
						}).Where(t => t.Times != null).ToList()
					}).Where(a => a.Times.Any()).ToList()
				})
			};

			var doctorsWithTimes = doctorDto.doctors.Where(d => d.Appointments != null && d.Appointments.Any()).ToList();

			if (doctorsWithTimes.Count == 0)
			{
				return Ok();
			}

			var DoctorDtoWithTime = new
			{
				doctors = doctorsWithTimes
			};

			return Ok(DoctorDtoWithTime);
		}

		[HttpGet]
		[Route("GetHisRequests")]
		public IActionResult GetHisRequests (string PatientId)
		{
			var request = patientRepository.PatientRequests(PatientId).ToList();
			var RequestDto = new
			{
				Request = request.Select(requestData => new{
				DoctorImage = patientRepository.GetDoctorRequest(requestData.Id).Result.PhotoPath,
				DoctorName = patientRepository.GetDoctorRequest(requestData.Id).Result.FirstName + " " + patientRepository.GetDoctorRequest(requestData.Id).Result.LastName,
				DoctorSpecialization = patientRepository.GetDoctorSpecializationRequest(requestData.Id).Result,
				AppointmentDay = patientRepository.GetDoctorAppointmentRequest(requestData.Id).Result.Day.GetDisplayName(),
				AppointmentTime = patientRepository.GetDoctorTimeRequest(requestData.Id).Result ,
				OldPrice = patientRepository.GetDoctorAppointmentRequest(requestData.Id).Result.Price,
				FinalPrice = requestData.FinalPrice,
				StatusRequest = requestData.Status.GetDisplayName(),
				DiscoundCoupon = patientRepository.GetDoctorDiscoundRequest(requestData.Id).Result

				}).ToList(),
			};
			return Ok(RequestDto);
		}

	}
}
