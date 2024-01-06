using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using VezeetaServices.PatientServices;
using VezeetaServices.RequestServices;

namespace Vezeeta.Api.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdministartionPatientController : ControllerBase
	{
		
		
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IAdminpatientRepository patientRepository;
		private readonly IRequestRepository requestRepository;


		public AdministartionPatientController(IRequestRepository requestRepository,
			IAdminpatientRepository patientRepository,
			UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
			this.patientRepository = patientRepository;
			this.requestRepository = requestRepository;
		}


		[HttpGet]
		[Route("GetAllPatients")]
		public async Task<IActionResult> GetAllPatients(string? Srearch, string? SortBy, int Page = 1, int PagesLimit = 10)
		{
			var result = await patientRepository.GetAllPatients(Srearch, SortBy, Page, PagesLimit);

			Response.Headers.Add("X-Total-Count",
				result.TotalCount.ToString());
			Response.Headers.Add("X-Total-Pages",
				result.TotalPages.ToString());

			
			var patients = result.Patients.Where(p=> userManager.IsInRoleAsync(p,"Patient").Result).Select(patient => new PatientDto
			{
				Image = patient.PhotoPath,
				FullName = patient.FirstName + " " + patient.LastName,
				Email = patient.Email,
				PhoneNumber = patient.PhoneNumber,
				Gender = patient.Gender.Value.ToString(),
				DateOfBirth = patient.BirthOfDate.Date.ToShortDateString()
			});

			return Ok(patients);

		}

		[HttpGet]
		[Route("GetDPatientById")]
		public async Task<IActionResult> GetPatientById(string PatientId)
		{
			 
			var patient = patientRepository.GetPatientById(PatientId);
			var request = patientRepository.PatientRequests(PatientId).ToList();
		
			var PatientDto = new
			{
				Patient = new
				{
					Image = patient.PhotoPath,
					FullName = patient.FirstName + " " + patient.LastName,
					Email = patient.Email,
					PhoneNumber = patient.PhoneNumber,
					Gender = patient.Gender.Value.ToString(),
					Age = patientRepository.CalculateAge(patient.BirthOfDate),
					BirthOfDate = patient.BirthOfDate.Date.ToShortDateString()
				},
				requesPatient = request.Select(requestData => new
				{
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

			return Ok(PatientDto);
		}

		[HttpGet]
		[Route("PatientsNumber")]
		public async Task<IActionResult> GetPatientsNum()
		{
			int result = await patientRepository.GetPatientsNum();
			if (result == 0)
			{
				return BadRequest("No Patients Exists");
			}
			return Ok(result);
		}

		[HttpGet]
		[Route("GetRequestsNumber")]
		public IActionResult RequestsNumber()
		{
			int AllRequests = requestRepository.GetAllRequestNum();
			int PendingRequests = requestRepository.GetPendingRequestNum();
			int CompleteRequest = requestRepository.GetCompleteRequestNum();
			int CancelRequests = requestRepository.GetCancelRequestNum();

			return Ok($"All Requests Number = {AllRequests}" +
				$"  Pending Requests Number = {PendingRequests}" +
				$"  Completed Requests Number = {CompleteRequest}" +
				$"  Canceled Requests Number = {CancelRequests}");
		}

	}
}
