using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository.Repository;

namespace VezeetaServices.RequestServices
{
	public interface IRequestRepository
	{
		bool AddRequest(string PatientId,Request requets);
		bool CancelRequest(int RequestId,string PatientId);
		bool ConfirmRequest(int RequestId, string DoctorId);
		Task<string> GetTimeValue(int id);
		Time GetTime(string time);
		Discound GetDiscound(string Coupon);
		int GetAppointmentPrice(string time);
	    int GetAllRequestNum();
		int GetPendingRequestNum();
		int GetCompleteRequestNum();
		int GetCancelRequestNum();
		int GetCompletedRequestNumToPatient(string PatientId);
		string GetDoctorId(int RequiestId);
		


	}
}
