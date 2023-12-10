using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using VezeetaServices.RequestServices;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IRequestRepository requestRepository;
		public PatientController (IRequestRepository requestRepository)
		{
			this.requestRepository = requestRepository;
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
			if (model.DiscoundCoupon == null && time.RequestId == null)
			{
				result.Discound = null;
				result.FinalPrice = Price;
			}
			else if (model.DiscoundCoupon != null && time.RequestId == null)
			{
				var Coupon = requestRepository.GetDiscound(model.DiscoundCoupon);
				if(Coupon.IsActive && CompletedRequestsCount > Coupon.RequestNumber)
				{
					if (Coupon.Type == DiscoundType.Value)
					{
						result.Discound = Coupon;
						result.FinalPrice = Price - Coupon.Value;
					}
					else if (model.DiscoundCoupon != null && Coupon.Type == DiscoundType.Precentage)
					{
						result.Discound = Coupon;
						result.FinalPrice = Price * (Coupon.Value/100);
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


	}
}
