using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;
using Vezeeta.Services.DoctorServices;
using VezeetaServices.AdminSettingServices;
using VezeetaServices.PatientServices;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdministrationDoctorController : ControllerBase
	{
		private readonly IAdminDoctorRepository doctorRepository;
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly UserManager<ApplicationUser> userManager;


		public AdministrationDoctorController(IAdminDoctorRepository doctorRepository,
			 IWebHostEnvironment hostingEnvironment,
			UserManager<ApplicationUser> userManager)
		{
			this.doctorRepository = doctorRepository;
			this.hostingEnvironment = hostingEnvironment;
			this.userManager = userManager;

		}

		private string ProcessUploadFileDoctor(AddDoctor doctor)
		{
			string uniqueFileName = null;
			if (doctor.Photo != null)
			{
				string uploadFile = Path.Combine(hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + doctor.Photo.FileName;
				string filePath = Path.Combine(uploadFile, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					doctor.Photo.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

		private string ProcessUploadFileDoctor(EditDoctorDto doctor)
		{
			string uniqueFileName = null;
			if (doctor.Photo != null)
			{
				string uploadFile = Path.Combine(hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + doctor.Photo.FileName;
				string filePath = Path.Combine(uploadFile, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					doctor.Photo.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

		[HttpGet]
		[Route("GetAllDoctors")]
		public async Task<IActionResult> GetAllDoctors(string? Srearch,string? SortBy,int Page = 1,int PagesLimit = 10)
		{
			var result = await doctorRepository.GetAllDoctors(Srearch, SortBy, Page, PagesLimit);
			
			Response.Headers.Add("X-Total-Count",
				result.TotalCount.ToString());
			Response.Headers.Add("X-Total-Pages",
				result.TotalPages.ToString());

			var doctors =  result.Doctors.Select(doctor => new DoctorDto
			{
				Image = doctor.PhotoPath,
				FullName = doctor.FirstName + " " + doctor.LastName,
				Email = doctor.Email,
				PhoneNumber = doctor.PhoneNumber,
				Specialize = doctorRepository.GetSpecialization(doctor.Id).Result ,
				Gender = doctor.Gender.Value.ToString()
			}); 
			
			return Ok(doctors);
		}

		[HttpGet]
		[Route("GetDoctorById")]
		public  IActionResult GetDoctorById(string id)
		{
			var result = doctorRepository.GetDoctor(id);
			if (result == null)
			{
				return BadRequest("No Doctor Found");
			}
			else
			{
				var SpecializeName = doctorRepository.GetSpecialization(id);

				return Ok(new {
					Image = result.PhotoPath,
					FullNmae = result.FirstName+" "+result.LastName,
					Email= result.Email,
					PhoneNumber= result.PhoneNumber,
					Specialize = SpecializeName.Result,
					Gender = result.Gender.Value.ToString(),
					BirthOfDate =result.BirthOfDate.Date.ToShortDateString()
				});
			}

		}

		[HttpGet]
		[Route("GetTopDoctors")]
		public IActionResult GetTopDoctors()
		{
			var topDoctors = doctorRepository.GetTopDoctors().ToList();
			var doctors = topDoctors.Select(doctor => new TopDoctorsDto
			{
				Image = doctor.PhotoPath,
				FullName = doctor.FirstName + " " + doctor.LastName,
				Specialize = doctorRepository.GetSpecialization(doctor.Id).Result,
				RequestsNum = doctorRepository.GetNumRequestsForDoctors(doctor.Id)
			});
			return Ok(doctors);
		}

		[HttpGet]
		[Route("GetTopSpecialties")]
		public IActionResult GetTopSpecialize()
		{
			var specializeList = doctorRepository.GetTopSpecialize();
			var specializes = specializeList.Select(specialize => new TopSpecializeDto
			{
				Name = specialize.Name,
				Requests=doctorRepository.GetRequestsNumForSpecialize(specialize.Id)
			});
			return Ok(specializes);
		}

		[HttpGet]
		[Route("DoctorsNumbers")]
		public IActionResult GetDoctorsNum()
		{
			var result =  doctorRepository.GetDoctorsNum();
			if (result == 0)
			{
				return BadRequest("No Doctors Exist");
			}
			else
			{
				return Ok(result);
			}
		}

		[HttpPost]
		[Route("AddDoctor")]
		public async Task<IActionResult> AddDoctor([FromForm] AddDoctor doctor)
		{
			
				string uniqueFileName = ProcessUploadFileDoctor(doctor);

				var specialistDoctor = doctorRepository.AddSpecialization(doctor.Specialization);
				if (specialistDoctor == null)
				{

					return BadRequest("Invalid Specialization");

				}

				var doctorUser = new Doctor
				{
					FirstName = doctor.FirstName,
					LastName = doctor.LastName,
					Email = doctor.Email,
					UserName = doctor.Email,
					Gender = doctor.Gender,
					BirthOfDate = doctor.BirthOfDate,
					PhoneNumber = doctor.PhoneNumber,
					PhotoPath = uniqueFileName,
					Price = doctor.Price,
					Specialist = specialistDoctor

				};

				var result = await userManager.CreateAsync(doctorUser, doctor.Password);

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(doctorUser, "Doctor");
					return StatusCode(200,true);
				}
				else
				{
				return StatusCode(400, false); 
				}
		}

		[HttpPut]
		[Route("EditDoctor")]
		public IActionResult EditDoctor(string id,[FromForm]EditDoctorDto model)
		{
				Doctor doctor = doctorRepository.GetDoctor(id);
				var specialistDoctor = doctorRepository.AddSpecialization(model.Specialization);
				doctor.FirstName = model.FirstName;
				doctor.LastName = model.LastName;
				doctor.Email = model.Email;
				doctor.UserName = model.Email;
				doctor.PhoneNumber = model.PhoneNumber;
				doctor.Gender = model.Gender;
				doctor.BirthOfDate = model.BirthOfDate;
				doctor.Price = model.Price;
				doctor.Specialist = specialistDoctor;
		
					if(model.Photo != null)
					{
						string FilePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.Photo.ToString());
						System.IO.File.Delete(FilePath);
					}
					doctor.PhotoPath = ProcessUploadFileDoctor(model);

			 var result =  doctorRepository.UpdateDoctor(doctor);
			if (result)
			{
				
				return StatusCode(200, true);
			}
			else
			{
				return StatusCode(400, false);
			}

		}


		[HttpDelete]
		[Route("DeleteDoctor")]
		public IActionResult DeleteDoctor(string Id)
		{
			var result = doctorRepository.DeleteDoctor(Id);
			if (result)
			{	
				return StatusCode(200, true);
			}
			else
			{
				return StatusCode(400, false);
			}
		}

		
	}
}
