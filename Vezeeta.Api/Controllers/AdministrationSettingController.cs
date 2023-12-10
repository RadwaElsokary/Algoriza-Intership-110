using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Services.DoctorServices;
using VezeetaServices.AdminSettingServices;

namespace Vezeeta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdministrationSettingController : ControllerBase
	{
		private readonly IAdminSettingRepository discoundRepository;


		public AdministrationSettingController(IAdminSettingRepository discoundRepository)
		{
			this.discoundRepository = discoundRepository;
		}

		[HttpPost]
		[Route("AddDiscoundCode")]
		public IActionResult AddDiscound(DiscoundDto model)
		{
			var result = discoundRepository.AddDiscound(model);
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
		[Route("EditDiscoundCode")]
		public IActionResult EditDiscound(int id, DiscoundDto discoundModel)
		{
			var result = discoundRepository.EditDiscoud(id, discoundModel);
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
		[Route("DeleteDiscoundCode")]
		public IActionResult DeleteDiscound(int id)
		{
			var result = discoundRepository.DeleteDiscound(id);
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
		[Route("DeactiveDiscound")]
		public IActionResult DeactiveDiscound(int id)
		{
			var result = discoundRepository.DeactiveDiscound(id);
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
