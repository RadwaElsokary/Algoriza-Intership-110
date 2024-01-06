using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;

namespace VezeetaServices.AppointmentServices
{
	public interface IAppointmentRepository
	{
		
		bool AddAppointment(string DoctorId,Appointment appointment);
		bool UpdateAppointment(int TimeId, EditTimeDto editTime);
		bool DeleteAppointment( int TimeId);
		Time GetTime(int Id);
		int CalculateAge(DateTime dateOfBirth);
		string GetPatientTime(int TimeId);
		string GetPatientDay(int TimeId);
		List<Request> BookingByDoctorId(string DoctorId);
		ApplicationUser GetPatient(string PatientId);
		Task<PageRequest> GetAllPatientBooking(string? term, int? page, int limit, string DoctorId);





	}
}
