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
		Task<Doctor> GetDoctorRequest(int RequestId);
		Task<string> GetDoctorSpecializationRequest(int RequestId);
		Task<Appointment> GetDoctorAppointmentRequest(int RequestId);
		Task<string> GetDoctorTimeRequest(int RequestId);
		Task<string> GetDoctorDiscoundRequest(int RequestId);


	}
}
