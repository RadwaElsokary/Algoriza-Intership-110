using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;

namespace VezeetaServices.PatientServices
{
	public interface IAdminpatientRepository
	{
		Task<PagePatient> GetAllPatients(string? term, string? sort, int? page, int limit);
		Task<int> GetPatientsNum();
		ApplicationUser GetPatientById(string PatientId);
		List<Request> PatientRequests(string PatientId);
		int CalculateAge(DateTime dateOfBirth);
		Task<Doctor> GetDoctorRequest(string PatientId);
		Task<string> GetDoctorSpecializationRequest(string PatientId);
		Task<Appointment> GetDoctorAppointmentRequest(string PatientId);
	    Task<Time> GetDoctorTimeRequest(string PatientId);
		Task<string> GetDoctorDiscoundRequest(string PatientId);


	}
}
