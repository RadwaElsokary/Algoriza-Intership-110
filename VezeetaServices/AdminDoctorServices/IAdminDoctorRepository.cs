using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;

namespace Vezeeta.Services.DoctorServices
{
	public interface IAdminDoctorRepository
	{
		Task<PageDoctor> GetAllDoctors(string? term, string? sort, int? page, int limit);
		Doctor GetDoctor(string Id);
		bool UpdateDoctor(Doctor doctor);
		bool DeleteDoctor(string Id);
	    int GetDoctorsNum();
	    Task<string> GetSpecialization(string id);
		Specialization AddSpecialization(string SpecializationDoctor);
		


	}
}
